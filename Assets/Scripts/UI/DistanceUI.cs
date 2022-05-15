using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DistanceUI : TextUIElement
{
    [SerializeField] private PlayerStatistics playerStatistics;
    private void OnEnable()
    {
        playerStatistics.OnDistanceChanged += UpdateDistanceText;
    }
    private void OnDisable()
    {
        playerStatistics.OnDistanceChanged -= UpdateDistanceText;
    }
    private void UpdateDistanceText(float distance)
    {
        stringBuilder.Length = originalStringLength;
        stringBuilder.Append(distance.ToString("F2"));
        textMeshUI.text = stringBuilder.ToString();
    }

}
