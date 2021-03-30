using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI bestPlatformText;
    [SerializeField] TextMeshProUGUI totalCoinsText;
    [SerializeField] TextMeshProUGUI currPlatformText;
    [Space]
    [SerializeField] TextMeshProUGUI floatingPlatformNum;
    [SerializeField] Animation floatingPlatformAnimation;

    private Camera camera;
    private Transform currPlatformTransform;


    public int CurrCoins { set => coinsText.text = value.ToString(); }
    public int TotalCoins { set => totalCoinsText.text = "Total " + value.ToString(); }
    public int BestPlatform { set => bestPlatformText.text = "Best " + value.ToString(); }
    public int CurrPlatformNum
    {
        set
        {
            currPlatformText.text = value.ToString();
            floatingPlatformNum.text = value.ToString();
        } 
    }


    private void Awake()
    {
        camera = Camera.main;
    }

    private void OnEnable()
    {
        CurrCoins = 0;
        CurrPlatformNum = 0;

        TotalCoins = DataState.TotalCoins;
        BestPlatform = DataState.BestPlatform;

        floatingPlatformNum.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (currPlatformTransform != null)
        {
            const float offsetY = -17f;

            floatingPlatformNum.transform.position = new Vector3(
                floatingPlatformNum.transform.position.x,
                camera.WorldToScreenPoint(currPlatformTransform.position).y + offsetY,
                floatingPlatformNum.transform.position.z);
        }
    }

    public void SetFollowPlatform(Transform platform)
    {
        currPlatformTransform = platform;

        floatingPlatformNum.gameObject.SetActive(true);
        floatingPlatformAnimation.Play();
    }
}
