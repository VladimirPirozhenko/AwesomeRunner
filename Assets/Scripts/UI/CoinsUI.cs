using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CoinsUI : TextUIElement
{
    [SerializeField] private PlayerStatistics playerStatistics;
    private void OnEnable()
    {
        playerStatistics.OnCoinCountChanged += UpdateCoinsText;
    }
    private void OnDisable()
    {
        playerStatistics.OnCoinCountChanged -= UpdateCoinsText;
    }
    private void UpdateCoinsText(int coinCount)
    {
        textMeshUI.text = coinCount.ToString();
        stringBuilder.Length = originalStringLength;
        stringBuilder.Append(coinCount.ToString());
        textMeshUI.text = stringBuilder.ToString();
    }
}
