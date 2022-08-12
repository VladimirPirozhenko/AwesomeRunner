using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour, ICommandTranslator
{
    private List<ScoreboardEntry> entries = new List<ScoreboardEntry>();

    public event Action<ScoreboardEntry> OnEntryAdded;

    [SerializeField] private ScoreboardView scoreboardView;
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
        GameSession.Instance.AddCommandTranslator(this);
        string jsonScoreboardEntries = PlayerPrefs.GetString("ScoreboardEntriesTableT1"); //Binary file
        ScoreboardEntriesTable entriesTable = JsonUtility.FromJson<ScoreboardEntriesTable>(jsonScoreboardEntries);
        if (entriesTable == null)
            return;
        if (entriesTable.entries == null)
            return;
        List<PlayerScoreboardCardData> scoreboardCardDatas = new List<PlayerScoreboardCardData>();  
        for (int i = 0; i < entriesTable.entries.Count; i++)
        {
            entries.Add(entriesTable.entries[i]);
            OnEntryAdded?.Invoke(entriesTable.entries[i]);
            PlayerScoreboardCardData cardData = new PlayerScoreboardCardData(entriesTable.entries[i].Name, entriesTable.entries[i].Score.ToString());
            scoreboardCardDatas.Add(cardData);
        }  
        scoreboardView.AddPlayerCards(scoreboardCardDatas);
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

    public void TranslateCommand(ECommand command, PressedState state)
    {
        switch (command)
        {
            case ECommand.OPEN_SCOREBOARD:
                if (state.IsPressed == true)
                    scoreboardView.Show(true);
                if (state.IsReleased == true)
                    scoreboardView.Show(false);
                break;
            default:
                scoreboardView.Show(false);
                break;
        }
    }
}
