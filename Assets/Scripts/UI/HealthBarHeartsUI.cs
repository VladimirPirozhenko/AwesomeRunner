using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHeartsUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;  
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite emptyHeartSprite;//Heart  GameObject 
    [SerializeField] private Vector2 heartSize;
    [SerializeField] private int gapBetweenHearts;
    private List<Image> heartImages;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }
    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthBar;
    }
    private void Start()
    {
        heartImages = new List<Image>(playerHealth.MaxHealth);
        for (int i = 0; i < playerHealth.MaxHealth; i++)
        {
            GameObject heartImageGO = new GameObject("HeartImage"); 
            RectTransform trans = heartImageGO.AddComponent<RectTransform>();
            trans.transform.SetParent(this.transform); 
            trans.localScale = Vector3.one;  
            trans.anchoredPosition = new Vector2(i * (heartSize.x + gapBetweenHearts) , 0f) ; //HorizontalLayout group
            trans.sizeDelta = new Vector2(heartSize.x, heartSize.y); 
            Image image = heartImageGO.AddComponent<Image>();
            image.sprite = heartSprite;
            image.enabled = false;
            heartImageGO.transform.SetParent(this.transform);
            if (i < playerHealth.MaxHealth)
            {
                image.enabled = true;
            }
            heartImages.Add(image);
        }
    }
    public void UpdateHealthBar(object sender,HealthArgs args)
    {
        for (int i = 0; i < args.CurrentLives; i++)
        {
            heartImages[i].sprite = heartSprite;
        }
        for (int i = args.CurrentLives; i < args.MaxLives; i++)
        {
            heartImages[i].sprite = emptyHeartSprite;
        }
    }
}
