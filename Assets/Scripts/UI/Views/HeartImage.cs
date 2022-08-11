using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class HeartImage : MonoBehaviour
{
    public Image image { get; private set; }   
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private void Awake()
    {
        image = GetComponent<Image>();
        SetFullHeartSprite();
    }

    public void SetFullHeartSprite()
    {
        if (image != null)
        {
            image.sprite = heartSprite;
        }
    }

    public void SetEmptyHeartSprite()
    {
        if (image != null)
        {
            image.sprite = emptyHeartSprite;    
        }
    }
}
