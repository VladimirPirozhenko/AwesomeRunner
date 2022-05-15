using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public class StatisticsArgs : EventArgs
//{
//    public float Score { get; private set; }
//    public float Distance { get; private set; }
//    public float CoinCount { get; private set; }
//    public StatisticsArgs(int coinCount,float distance, float score)
//    {
//        Score = score;
//        Distance = distance;
//        CoinCount = coinCount;
//    }
//    public StatisticsArgs GetUpdatedArgs(int coinCount, float distance, float score) //to avoid GC overhead
//    {
//        CoinCount = coinCount;
//        Distance = distance;
//        Score = score;
//        return this;
//    }
//}
public class PlayerStatistics : MonoBehaviour,IResettable
{
    private float distance;
    private int coinCount;
    private int coinMultiplier;
    private int score;
    [SerializeField] private GameOverPopUp gameOverPopUp;
    public event Action<int> OnCoinCountChanged = delegate { };
    public event Action<float> OnDistanceChanged = delegate { };
    public event Action<int> OnScoreCalculated = delegate { };
    //private StatisticsArgs statisticsArgs;
    //public event EventHandler<StatisticsArgs> OnStatsChanged = delegate { };

    private void Awake()
    {
        ResetToDefault();
    }
    public void IncreaseCoinCount()
    {
        coinCount++;
        OnCoinCountChanged?.Invoke(coinCount);
       // OnStatsChanged(this, statisticsArgs.GetUpdatedArgs(coinCount, distance, score));
    }
    public void IncreaseDistance(float amount)
    {
        distance += amount;
        OnDistanceChanged?.Invoke(distance);
       // OnStatsChanged(this, statisticsArgs.GetUpdatedArgs(coinCount, distance, score));
    }
    public void ShowGameOverPopUp(bool isActive)
    {
        gameOverPopUp.gameObject.SetActive(isActive);  
        CalculateScore();//SRP VIOLATION
    }

    private void CalculateScore()
    {
        score = Mathf.FloorToInt(coinCount * coinMultiplier + distance);
        OnScoreCalculated?.Invoke(score);
       // OnStatsChanged(this, statisticsArgs.GetUpdatedArgs(coinCount, distance, score));
    }

    public void ResetToDefault()
    {
        distance = 0;
        score = 0;
        coinCount = 0;
        coinMultiplier = 1;
        gameOverPopUp.gameObject.SetActive(false);
    }
}
