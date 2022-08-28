using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreboardEntriesTable
{
    public ScoreboardEntriesTable(List<ScoreboardEntry> entries)
    {
        this.entries = entries; 
    }
    public List<ScoreboardEntry> entries = new List<ScoreboardEntry>();
}

public class Scoreboard : MonoBehaviour, ICommandTranslator
{
    [SerializeField] private int maxEntries;
    private List<ScoreboardEntry> entries = new List<ScoreboardEntry>();

    public event Action<ScoreboardEntry> OnEntryAdded;

    [SerializeField] private ScoreboardView scoreboardView;

    private void Start()
    {
        GameSession.Instance.AddCommandTranslator(this);
        string jsonScoreboardEntries = PlayerPrefs.GetString("ScoreboardEntriesTableTest"); //Binary file
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
        SortScoreboardEntriesByHighscore(entries);
        SortScoreboardCardsDatasByHighscore(scoreboardCardDatas);
        scoreboardView.AddPlayerCards(scoreboardCardDatas);
    }

    public void AddScoreboardEntry(string entryName, int entryScore)
    {
        ScoreboardEntry entry = new ScoreboardEntry(entryName, entryScore);
        entries.Add(entry);
        OnEntryAdded?.Invoke(entry);
    }

    public void SortScoreboardEntriesByHighscore(List<ScoreboardEntry> entries)
    {
        entries.Sort((x,y) => y.Score.CompareTo(x.Score));
    }

    public void SortScoreboardCardsDatasByHighscore(List<PlayerScoreboardCardData> scoreboardCardDatas)
    {
        scoreboardCardDatas.Sort((x, y) => y.playerScore.CompareTo(x.playerScore));
    }

    public void AddScoreboardEntry(ScoreboardEntry entry)
    {
        entries.Add(entry);
        OnEntryAdded?.Invoke(entry);
        SaveScoreboardEntriesTable();
    }   

    public void SaveScoreboardEntriesTable()
    {
        SortScoreboardEntriesByHighscore(entries);
        ScoreboardEntriesTable scoreboardEntriesTable = new ScoreboardEntriesTable(entries);
        string jsonScoreboardEntries = JsonUtility.ToJson(scoreboardEntriesTable);
        PlayerPrefs.SetString("ScoreboardEntriesTableTest", jsonScoreboardEntries);
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
