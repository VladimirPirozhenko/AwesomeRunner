using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardUI : MonoBehaviour
{
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private EntryUI entryUiPrefab;
    [SerializeField] private Transform entryList;
    [SerializeField] private ScrollRect scrollRect;

    private ObjectPool<EntryUI> entryUIPool;
    public void CreateEntryUIPool() // ¬€Õ≈—“» ¬  À¿——€
    {
        Func<EntryUI> createEntryUI = () =>
        {
            EntryUI entryUI = Instantiate(entryUiPrefab);
            entryUI.gameObject.SetActive(false);
            entryUI.transform.SetParent(gameObject.transform, false);
            return entryUI;
        };
        Action<EntryUI> getEntryUI = (EntryUI entryUI) =>
        {
            entryUI.gameObject.SetActive(true);
        };
        Action<EntryUI> releaseEntryUI = (EntryUI entryUI) =>
        {
            entryUI.transform.SetParent(gameObject.transform, false);
            entryUI.gameObject.SetActive(false);
        };
        entryUIPool = new ObjectPool<EntryUI>(createEntryUI, getEntryUI, releaseEntryUI, 1000);
    }
    //private RectTransform scrollTransform;
    //private const float DistanceToRecalcVisibility = 400.0f;
    //private const float DistanceMarginForLoad = 600.0f;

    //private float lastPos = Mathf.Infinity;
    //private CanvasGroup canvas;
    private List<EntryUI> entries = new List<EntryUI>();    
    private void Awake()
    {
        scoreboard.OnEntryAdded += UpdateScoreboard;
        //canvas = GetComponent<CanvasGroup>();
       // scrollTransform = this.scrollRect.GetComponent<RectTransform>();
        Show(false);
        CreateEntryUIPool();
    }
    private void Start()
    {
        //this.scrollRect.onValueChanged.AddListener((newValue) => {
        //    if (Mathf.Abs(this.lastPos - this.scrollRect.content.transform.localPosition.y) >= DistanceToRecalcVisibility)
        //    {
        //        lastPos = this.scrollRect.content.transform.localPosition.y;

        //        //scrollTransform = this.scrollRect.GetComponent<RectTransform>();
        //        float checkRectMinY = scrollTransform.rect.yMin - DistanceMarginForLoad;
        //        float checkRectMaxY = scrollTransform.rect.yMax + DistanceMarginForLoad;

        //        foreach (Transform child in this.scrollRect.content)
        //        {
        //            RectTransform childTransform = child.GetComponent<RectTransform>();
        //            Vector3 positionInWord = childTransform.parent.TransformPoint(childTransform.localPosition);
        //            Vector3 positionInScroll = scrollTransform.InverseTransformPoint(positionInWord);
        //            float childMinY = positionInScroll.y + childTransform.rect.yMin;
        //            float childMaxY = positionInScroll.y + childTransform.rect.yMax;

        //            if (childMaxY >= checkRectMinY && childMinY <= checkRectMaxY)
        //            {
        //                // child.GetComponent<Image>().color = Color.blue;
        //                child.gameObject.SetActive(true);
        //            }
        //            else
        //            {
        //               // child.GetComponent<Image>().color = Color.green;
        //                child.gameObject.SetActive(false);
        //            }
        //        }
        //    }
        //});
        
    }
    private void OnDestroy()
    {
        scoreboard.OnEntryAdded -= UpdateScoreboard;
    }
    public void Show(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
        if (isVisible)
        {
            //canvas.alpha = 1f;    
        }
        else
        {
            //canvas.alpha = 0f;
        }
    }
    public void UpdateScoreboard(ScoreboardEntry entry)
    {
        //EntryUI entryInstance = entryUIPool.Get();
        EntryUI entryInstance = Instantiate(entryUiPrefab,transform);
        entryInstance.transform.SetParent(entryList);
        entryInstance.gameObject.SetActive(true);
        entryInstance.UpdateContents(entry);
        entries.Add(entryInstance);
    }

}
