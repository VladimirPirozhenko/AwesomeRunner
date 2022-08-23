using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardView : BaseView
{
    [SerializeField] private Button backButton;
    [SerializeField] private PlayerScoreboardCard cardPrefab;
    private VerticalLayoutGroup layoutGroup;
    //private readonly Dictionary<string,PlayerScoreboardCard> playerCards = new Dictionary<string, PlayerScoreboardCard>();
    private readonly List<PlayerScoreboardCard> playerCards = new List<PlayerScoreboardCard>();
    public override void Init()
    {
        base.Init();
        layoutGroup = GetComponentInChildren<VerticalLayoutGroup>();    
        backButton.onClick.AddListener(() =>
        {
            Show(false);
            ViewManager.Instance.Show<PausedView>(true);
        });
    }
    public void AddPlayerCards(List<PlayerScoreboardCardData> cardsData)
    {
        foreach (var cardData in cardsData)
        {
            AddPlayerCard(cardData);
        }
    }
       
    private void AddPlayerCard(PlayerScoreboardCardData cardData)
    {
       //if (playerCards.ContainsKey(cardData.playerName))
        //    return;
        PlayerScoreboardCard playerScoreboardCard = Instantiate(cardPrefab);
        playerScoreboardCard.transform.SetParent(layoutGroup.transform, false);
        playerScoreboardCard.UpdateCard(cardData);   
        playerCards.Add(playerScoreboardCard);
    }

    public void RemovePlayerCard(string cardTag)
    {
        //if (playerCards.ContainsKey(cardTag))
        //{
        //playerCards.TryGetValue(cardTag, out PlayerScoreboardCard playerScoreboardCard);
        //playerScoreboardCard.gameObject.SetActive(false); //TODO: Pooling
        //playerCards.Remove(cardTag);
        //}
        foreach (var playerCard in playerCards)
        {
            if (cardTag == playerCard.name)
            {
                playerCard.gameObject.SetActive(false);
                playerCards.Remove(playerCard);
            }
        }
    }

    public void RefreshPlayerCard(PlayerScoreboardCardData cardData)
    {
        //if (playerCards.TryGetValue(cardData.playerName, out PlayerScoreboardCard card))
        foreach (var playerCard in playerCards)
        {
            if (cardData.playerName == playerCard.name)
                playerCard.UpdateCard(cardData);
        }               
    }
}
