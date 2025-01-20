
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardParent;
    public List<Card> cards = new List<Card>();

    private Card lastCard;

    #endregion

    #region MonoBehaviour
    void Awake()
    {
        createDeck();
    }

    #endregion

    #region Logic
    void createDeck()
    {
        Card card;

        foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprits/cards"))
        {
            //Debug.Log(sprite == null);
            card = createCard(sprite);
            cards.Add(card);

        }
    }

    private Card createCard(Sprite sprite)
    {
        string[] cardInfo = sprite.name.Split('_');
        string cardSuit = cardInfo[0];
        string cardNumber = cardInfo[1];
        GameObject cardObject = Instantiate(cardPrefab, cardParent);

        //Debug.Log(sprite.name);

        if (cardObject == null)
        {
            Debug.LogError("Failed to instantiate cardPrefab.");
            return null;
        }

        Card card = cardObject.GetComponent<Card>();
        card.SetupCard(cardSuit, cardNumber, sprite);
        //Debug.Log(card == null);
        return card;
    }

    public Card drawCard()
    {
        if (cards.Count == 0)
        {
        //    returnCardsToDeck();
            if (cards.Count == 0)
            {
                Debug.LogWarning("No cards left to draw.");
                return null;
            }
        }
        int index = Random.Range(0, cards.Count);
        Card card = cards[index];
        cards.RemoveAt(index);
        return card;
    }

    /*   public void returnCardsToDeck()
       {
           for (int i = 0; i < DrawCards.instance.cards.Count; i++)
           {
               lastCard = DrawCards.instance.cards[i];
               DrawCards.instance.cards[i].transform.SetParent(DrawCards.instance.DeckArray.transform, false);
           }
       }*/
    #endregion
}
