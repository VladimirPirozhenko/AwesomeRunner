using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : TextUIElement
{
    [SerializeField] private Statistics playerStatistics;
    private int scoreIncrement = 0;
    private void OnEnable()
    {
        playerStatistics.OnScoreCalculated += UpdateScoreText;
    }
    private void OnDisable()
    {
        playerStatistics.OnScoreCalculated -= UpdateScoreText;
    }
    private void UpdateScoreText(int score)
    {
        StartCoroutine(ScoreCalculationAnimation(score));
    }
    public IEnumerator ScoreCalculationAnimation(int score)
    {
        while (scoreIncrement < score)
        {
            stringBuilder.Length = originalStringLength;
            stringBuilder.Append(scoreIncrement.ToString());
            textMeshUI.text = stringBuilder.ToString();
            scoreIncrement++;
            yield return null;
        } 
    }
}
