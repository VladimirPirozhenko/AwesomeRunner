using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour,IResettable
{
    private float distance;
    private int coinCount;
    private int coinMultiplier;
    public int Score { get; private set; }
    public event Action<int> OnCoinCountChanged = delegate { };
    public event Action<float> OnDistanceChanged = delegate { };
    public event Action<int> OnScoreCalculated = delegate { };
    [SerializeField] private PlayerHUDView PlayerHUD;
    private void Awake()
    {
        ResetToDefault();
    }
    private void OnEnable()
    {
        Coin.OnCoinCollected += UpdateCoinCount;
    }
    private void OnDisable()
    {
        Coin.OnCoinCollected -= UpdateCoinCount;
    }
    private void Update()
    {
        CalculateScore();
    }
    public void UpdateCoinCount(int amount)
    {
        coinCount += amount;
        OnCoinCountChanged?.Invoke(coinCount);
    }
    public void UpdateDistance(float amount)
    {
        distance += amount;
        OnDistanceChanged?.Invoke(distance);
    }
    public void CalculateScore()
    {
        Score = Mathf.FloorToInt(coinCount * coinMultiplier + distance);
        OnScoreCalculated?.Invoke(Score);
        PlayerHUD.UpdateScore(Score.ToString());
    }

    public void ResetToDefault()
    {
        distance = 0;
        Score = 0;
        coinCount = 0;
        coinMultiplier = 1;
        //gameOverPopUp.gameObject.SetActive(false);
    }
}
