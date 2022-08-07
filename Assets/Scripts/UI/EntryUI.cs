//using System.Collections;
//using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class RenderExtentions
{
    public static bool IsPartlyVisible(this RectTransform rectTransform, Camera camera)
    {

        bool result = false;
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
        //Vector2 pos = rectTransform.GetPosition(CoordinateSystem.ScreenSpacePixels, true);
        //Vector2 size = rectTransform.GetSize(CoordinateSystem.ScreenSpacePixels) * 0.5f;
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);   
        //Vector2[] objectCorners = new Vector2[4];
        //objectCorners[0] = new Vector2(pos.x - size.x, pos.y - size.y);
        //objectCorners[1] = new Vector2(pos.x - size.x, pos.y + size.y);
        //objectCorners[2] = new Vector2(pos.x + size.x, pos.y - size.y);
        //objectCorners[3] = new Vector2(pos.x + size.x, pos.y + size.y);

       // Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);

        int visibleCorners = 0;

        for (var i = 0; i < objectCorners.Length; i++)
        {
            if (screenBounds.Contains(objectCorners[i]))
            {
                visibleCorners++;
            }
        }

        if (visibleCorners > 0) // If at least one corner is inside the screen
        {
            result = true;
        }

        return result;
    }
}
[RequireComponent(typeof(RectTransform))]  
public class EntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameEntry;
    [SerializeField] private TextMeshProUGUI scoreEntry;
    private RectTransform rectTransform;
    Camera cameraMain;
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        enabled = false;
    }
    private void OnBecameVisible()
    {
        gameObject.SetActive(true);
        enabled = true;
    }
    public void OnEnable()
    {
        //gameObject.SetActive(true);
        nameEntry.alpha = 1f;
        scoreEntry.alpha = 1f;
    }
    public void OnDisable()
    {
       // gameObject.SetActive(false);
        nameEntry.alpha = 0f;
        scoreEntry.alpha = 0f;
    }
    private void Awake()
    {
        cameraMain = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(true);
        nameEntry.alpha = 0f;
        scoreEntry.alpha = 0f;
    }
    private void Update()
    {
        //if (rectTransform.IsPartlyVisible(cameraMain))
        //{
        //    gameObject.SetActive(true);
        //}
        //else
        //{
        //    gameObject.SetActive(false);
        //}
    }
    public void UpdateContents(ScoreboardEntry entry)
    {
        nameEntry.text = entry.Name;
        scoreEntry.text = entry.Score.ToString(); 
    }
}
