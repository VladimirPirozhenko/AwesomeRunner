using System.Collections;
using TMPro;
using UnityEngine;


public class FPSView : BaseView
{
    [SerializeField] private TextMeshProUGUI fpsText;

    [SerializeField] private float pollingTime = 1f;
    private float timeCount;
    private int frameCount;
    public void Update()
    {
        timeCount += Time.deltaTime;
        frameCount++;   
        if (timeCount >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / timeCount);
            fpsText.text = frameRate.ToString() + " FPS";
            timeCount -= pollingTime;
            frameCount = 0; 
        }
    }

}
