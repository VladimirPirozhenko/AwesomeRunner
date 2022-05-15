using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHeartsUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    [SerializeField] private Vector2 heartSize;
    [SerializeField] private int gapBetweenHearts;

    private List<Image> heartImages;
    private int currentLives;
    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }
    private void Start()
    {
       // currentLives = playerHealth.MaxLives;
        heartImages = new List<Image>(playerHealth.MaxLives);
        for (int i = 0; i < playerHealth.MaxLives; i++)
        {
            GameObject heartImageGO = new GameObject("HeartImage");
            RectTransform trans = heartImageGO.AddComponent<RectTransform>();
            trans.transform.SetParent(this.transform); 
            trans.localScale = Vector3.one;  
            trans.anchoredPosition = new Vector2(i * (heartSize.x + gapBetweenHearts) , 0f) ; 
            trans.sizeDelta = new Vector2(heartSize.x, heartSize.y); 
            Image image = heartImageGO.AddComponent<Image>();
            image.sprite = heartSprite;
            image.enabled = false;
            heartImageGO.transform.SetParent(this.transform);
            if (i < playerHealth.MaxLives)
            {
                image.enabled = true;
            }
            heartImages.Add(image);
        }
    }
    public void UpdateHealthBar(object sender,HealthArgs args)
    {


       // if (currentLives < args.MaxLives)
       //{
        for (int i = 0; i < args.CurrentLives; i++)
        {
            heartImages[i].sprite = heartSprite;
        }
        for (int i = args.CurrentLives; i < args.MaxLives; i++)
        {
            heartImages[i].sprite = emptyHeartSprite;
        }
      //  }
       // else
      //  {
            //heartImages[i].sprite = emptyHeartSprite;
       // }
        
        //currentLives = args.CurrentLives;
        //if (args.CurrentLives > args.MaxLives)
        //{
        //    //args.CurrentLives = args.MaxLives;
        //    heartImages[args.CurrentLives].enabled = true;
        //}
        //if (currentLives < args.CurrentLives)
        //{
        //    //args.CurrentLives++;
        //    heartImages[args.CurrentLives].enabled = true;
        //    heartImages[args.CurrentLives].sprite = heartSprite;  
        //}
        //else if (currentLives > args.CurrentLives)
        //{
        //    //args.CurrentLives--;
        //    heartImages[args.CurrentLives].sprite = emptyHeartSprite;
        //}
        //currentLives = args.CurrentLives;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthBar;
    }
}
