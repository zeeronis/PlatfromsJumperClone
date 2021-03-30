using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI bestPlatformText;
    [SerializeField] TextMeshProUGUI totalCoinsText;
    [SerializeField] TextMeshProUGUI currPlatformText;


    public int CurrCoins { set => coinsText.text = value.ToString(); }
    public int TotalCoins { set => totalCoinsText.text = "Total " + value.ToString(); }
    public int BestPlatform { set => bestPlatformText.text = "Best " + value.ToString(); }
    public int CurrPlatformNum { set => currPlatformText.text = value.ToString(); }


    private void OnEnable()
    {
        CurrCoins = GameManager.Instance.CollectedCoins;
        CurrPlatformNum = GameManager.Instance.CurrPlayerPlatformNum;

        TotalCoins = DataState.TotalCoins;
        BestPlatform = DataState.BestPlatform;
    }
}
