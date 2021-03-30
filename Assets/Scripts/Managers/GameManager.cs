using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Spawn settings")]
    [SerializeField] float playerStartY;
    [Space]
    [SerializeField] float platformsOffsetY;
    [SerializeField] float platformStartY;
    [Space]
    [SerializeField] float coinSpawnChance;
    [SerializeField] float enemySpawnChance;


    [Header("Player")]
    [SerializeField] BirdController birdController;

    private Camera camera;
    private PoolManager poolManager;
    private UIManager uiManager;

    private int collectedCoins;
    private int spawnedPlatforms;
    private int currPlayerPlatformNum;
    private int skipDespawnPlatformsCount;
    private bool skipSpawnNextEnemy;
    private List<Platform> platformsList = new List<Platform>();

    public bool IsRun { get; private set; }
    public int CollectedCoins => collectedCoins;
    public int CurrPlayerPlatformNum => currPlayerPlatformNum;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        camera = Camera.main;
        poolManager = GetComponent<PoolManager>();
        uiManager = GetComponent<UIManager>();

        birdController.OnDie += OnPlayerDie;
        birdController.OnCoinCollected += OnPlayerCollectCoin;
        birdController.Movement.OnPlatformEvent += OnPlayerReachedPlatform;
        DataState.LoadData();
    }

    private void Start()
    {
        StartGame();
    }


    private void OnPlayerDie()
    {
        EndGame();
    }

    private void OnPlayerCollectCoin(in Vector3 coinPos)
    {
        const int addCoins = 1;

        poolManager.GetObject<IncreaseNumEffect>(coinPos, Quaternion.identity).Play();

        collectedCoins += addCoins;
        DataState.TotalCoins += addCoins;

        uiManager.GameScreen.CurrCoins = collectedCoins;
        uiManager.GameScreen.TotalCoins = DataState.TotalCoins;
    }

    private void OnPlayerReachedPlatform(Platform platform)
    {
        if (currPlayerPlatformNum < platform.PlatformNum)
        {
            DespawnLastPlatform();
            StartCoroutine(SpawnNextPlatform());

            currPlayerPlatformNum = platform.PlatformNum;
            uiManager.GameScreen.CurrPlatformNum = currPlayerPlatformNum;
            uiManager.GameScreen.SetFollowPlatform(platform.VisualTransform);
        }
    }

    private IEnumerator SpawnNextPlatform()
    {
        Vector3 pos = new Vector3(0, platformStartY + platformsOffsetY * spawnedPlatforms, 0);
        Platform platformObj = poolManager.GetObject<Platform>(pos, Quaternion.identity);

        platformsList.Add(platformObj);
        platformObj.SetPlatformNum(spawnedPlatforms);

        spawnedPlatforms++;

        if (spawnedPlatforms > 1)
        {
            if (Random.Range(0f, 100f) < coinSpawnChance)
                SpawnCoin(platformObj);

            if (skipSpawnNextEnemy)
            {
                skipSpawnNextEnemy = false;
            }
            else if (Random.Range(0f, 100f) < enemySpawnChance)
            {
                skipSpawnNextEnemy = true;

                yield return null;
                SpawnEnemy(platformObj);
            }
        }
    }

    private void SpawnEnemy(Platform platformObj)
    {
        EnemyBase enemyObj = null;
        if (Random.Range(0, 2) == 0)
        {
            enemyObj = poolManager.GetObject<StayEnemy>(Vector3.zero, Quaternion.identity).Run();
        }
        else
        {
            enemyObj = poolManager.GetObject<WalkEnemy>(Vector3.zero, Quaternion.identity).Run();
        }

        platformObj.AddFloorObject(enemyObj, setAsChild: true);
        platformObj.AlignObjectY(enemyObj.transform, enemyObj.ColliderSizeY);
        enemyObj.Run();
    }

    private void SpawnCoin(Platform platform)
    {
        Vector3 pos = new Vector3(
            Random.Range(-1.8f, 1.8f), 
            platform.transform.position.y + Random.Range(0.8f, 1.5f));

        platform.AddFloorObject(poolManager.GetObject<Coin>(pos, Quaternion.identity));
    }

    private void DespawnLastPlatform()
    {
        if (skipDespawnPlatformsCount > 0)
        {
            skipDespawnPlatformsCount--;
            return;
        }

        platformsList[0].ReturnToPool();
        platformsList.RemoveAt(0);
    }

    private void SpawnStartObjects()
    {
        for (int i = 0; i < 6; i++)
        {
            StartCoroutine(SpawnNextPlatform());
        }

        birdController.transform.position = new Vector3(0, playerStartY, 0);
    }


    private void StartGame()
    {
        collectedCoins = 0;
        spawnedPlatforms = 0;
        currPlayerPlatformNum = 0;
        skipDespawnPlatformsCount = 2;
        skipSpawnNextEnemy = false;

        SpawnStartObjects();
        birdController.ResetRotate();
        uiManager.ActivScreen(Screens.Game);

        IsRun = true;
    }

    private void EndGame()
    {
        IsRun = false;
        DataState.BestPlatform = currPlayerPlatformNum;
        DataState.SaveData();

        uiManager.ActivScreen(Screens.Result);
    }

    public void RestartGame_Click()
    {
        foreach (var item in platformsList)
            item.ReturnToPool();

        platformsList.Clear();

        StartGame();
    }
}
