using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private EntryUI entryUIprefab;
    [SerializeField] private Transform entryList;
    private void Awake()
    {
        scoreboard.OnEntryAdded += UpdateScoreboard;
        Show(false);
    }
    private void OnDestroy()
    {
        scoreboard.OnEntryAdded -= UpdateScoreboard;
    }
    public void Show(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
    }
    public void UpdateScoreboard(ScoreboardEntry entry)
    {
        EntryUI entryInstance = Instantiate(entryUIprefab,transform);
        entryInstance.transform.SetParent(entryList);
        entryInstance.UpdateContents(entry);
    }

}
