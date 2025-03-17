
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject cardPrefab;
    public GameObject DeckButton;
    public List<Card> cards = new List<Card>();
    

    #endregion

    #region Logic
    public void createDeck()
    {
        Card card;

        foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprits/cards"))
        {
            card = createCard(sprite);
            cards.Add(card);
        }
    }

    private Card createCard(Sprite sprite)
    {
        string[] cardInfo = sprite.name.Split('_');
        string cardSuit = cardInfo[0];
        string cardNumber = cardInfo[1];
        GameObject cardObject = Instantiate(cardPrefab, GameManager.instance.DeckArray.transform);

        if (cardObject == null)
        {
            Debug.LogError("Failed to instantiate cardPrefab.");
            return null;
        }

        Card card = cardObject.GetComponent<Card>();
        card.SetupCard(cardSuit, cardNumber, sprite);
        return card;
    }

    public Card drawCard()
    {
        // Disable the button if the deck is empty
        if (cards.Count == 0)
        {
            DeckButton.GetComponent<Button>().interactable = false;
            return null;
        }

        int index = Random.Range(0, cards.Count);
        Card card = cards[index];
        cards.RemoveAt(index);
        return card;
    }


    #endregion
}
