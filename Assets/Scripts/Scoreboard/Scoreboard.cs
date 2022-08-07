using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    private List<ScoreboardEntry> entries = new List<ScoreboardEntry>();

    public event Action<ScoreboardEntry> OnEntryAdded;

    private class ScoreboardEntriesTable
    {
        public ScoreboardEntriesTable(List<ScoreboardEntry> entries)
        {
            this.entries = entries; 
        }
        public List<ScoreboardEntry> entries = new List<ScoreboardEntry>();
    }
    private void Start()
    {
        string jsonScoreboardEntries = PlayerPrefs.GetString("ScoreboardEntriesTableT1"); //Binary file
        ScoreboardEntriesTable entriesTable = JsonUtility.FromJson<ScoreboardEntriesTable>(jsonScoreboardEntries);
        if (entriesTable == null)
            return;
        if (entriesTable.entries == null)
            return;
        for (int i = 0; i < entriesTable.entries.Count; i++)
        {
            entries.Add(entriesTable.entries[i]);
            OnEntryAdded?.Invoke(entriesTable.entries[i]);
        }
    }
    public void AddScoreboardEntry(string entryName, int entryScore)
    {
        ScoreboardEntry entry = new ScoreboardEntry(entryName, entryScore);
        entries.Add(entry);
        OnEntryAdded?.Invoke(entry);
    }
    public void AddScoreboardEntry(ScoreboardEntry entry)
    {
        entries.Add(entry);
        OnEntryAdded?.Invoke(entry);
    }   
    public void SaveScoreboardEntriesTable()
    {
        ScoreboardEntriesTable scoreboardEntriesTable = new ScoreboardEntriesTable(entries);
        string jsonScoreboardEntries = JsonUtility.ToJson(scoreboardEntriesTable);
        PlayerPrefs.SetString("ScoreboardEntriesTableT1", jsonScoreboardEntries);
        PlayerPrefs.Save();
    }
}
