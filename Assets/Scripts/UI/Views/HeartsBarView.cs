using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsBarView : BaseView
{
    [SerializeField] private HeartImage heartImage;
    private List<HeartImage> heartImages;

    public void CreateHealthBar(HealthArgs args)
    {
        heartImages = new List<HeartImage>(args.MaxLives);
        for (int i = 0; i < args.MaxLives; i++)
        {
            HeartImage imageInstance = Instantiate(heartImage); 
            imageInstance.transform.SetParent(this.transform);
            heartImages.Add(imageInstance); 
        }
        gameObject.SetActive(true);
    }
    public void UpdateHealthBar(object sender, HealthArgs args)
    {
        if (heartImages.IsEmpty())
            return;
        for (int i = 0; i < args.CurrentLives; i++)
        {
            heartImages[i].SetFullHeartSprite();
        }
        for (int i = args.CurrentLives; i < args.MaxLives; i++)
        {
            heartImages[i].SetEmptyHeartSprite();
        }
    }
}
