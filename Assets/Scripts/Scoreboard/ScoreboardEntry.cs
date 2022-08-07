using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreboardEntry
{
    [SerializeField] private float score;
    [SerializeField] private string name;
    public float Score { get { return score; } private set { score = value; } }
    public string Name { get { return name; } private set { name = value; } }
    public ScoreboardEntry(string name,float score)
    {
        this.name = name;
        this.score = score;
    }
}
