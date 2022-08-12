using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]    
public class PlayerHUDView : BaseView
{
    [SerializeField] private TMP_Text scoreText;
    public override void Init()
    {
        base.Init();
        transform.position = Vector3.zero;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
    }

    public void UpdateScore(string score)
    {
        scoreText.text = $"Score: {score} "; 
    }
}
