//using System.Collections;
//using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameEntry;
    [SerializeField] private TextMeshProUGUI scoreEntry;
    public void UpdateContents(ScoreboardEntry entry)
    {
        nameEntry.text = entry.Name;
        scoreEntry.text = entry.Score.ToString();
    }
}
