
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Card : MonoBehaviour
//{
//    public enum Suit { clubs, diamonds, hearts, spades}
//    public enum Number { two, three, four, five, six, seven, eight, nine, jack , queen ,king , ace }

//    private string _cardSuit;
//    private string _cardNumber;
//    private Sprite _cardImg;
//    [SerializeField] private Image _cardImage;
//    public string cardSuit { get => _cardSuit; }
//    public string cardNumber { get => _cardNumber; }
//    public Sprite cardImg { get => _cardImg; }

//    public void SetupCard(string suit, string number, Sprite img)
//    {
//        _cardSuit = suit;
//        _cardNumber = number;
//        _cardImg = img;
//        _cardImage.sprite = _cardImg;
//        gameObject.name = cardSuit + "" + _cardNumber;
//    }
//}

//public void returnCardsToDeck()
//{
//    for (int i = 0; i < drawCards.instance.playedCards.Count; i++)
//    {
//        lastCard = drawCards.instance.playedCards[i];
//        drawCards.instance.playedCards[i].transform.SetParent(drawCards.instance.deckArray.transform, false);
//    }
//}