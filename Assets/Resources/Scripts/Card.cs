
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    #region Variables
    public enum Suit { clubs, diamonds, hearts, spades}
    public enum Number {  ace = 1, two, three, four, five, six, seven, eight, nine, ten, jack , queen ,king }
    private string[] suitNames = { "clubs", "diamonds", "hearts", "spades" };
    private string[] numberNames = { "ace", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "jack", "queen", "king" };
    private Suit _cardSuit;
    private Number _cardNumber;
    private Sprite _cardImg;
    [SerializeField] private Image _cardImage;
   // public Suit cardSuit { get => _cardSuit; }
    private string _cardSuitString;
    private string _cardNumberString;
    //public Number cardNumber { get => _cardNumber; }
    public string cardSuitString { get => _cardSuitString; }
    public string cardNumberString { get => _cardNumberString; }
    public Sprite cardImg { get => _cardImg; }
    public static Action<Card> OnClickCard;

    #endregion

    #region Logic

    public Card(Suit cardSuit, Number cardNumber, Sprite img)
    {
        _cardSuit = cardSuit;
        _cardNumber = cardNumber;
        _cardImg = img;
        _cardImage.sprite = _cardImg;
        int Cnumber = (int)cardNumber;
        gameObject.name = suitNames[Cnumber] + "" + numberNames[Cnumber];
    }

    public void SetupCard(string suit, string number, Sprite img)
    {
        _cardSuitString = suit;
        _cardNumberString = number;
        _cardImg = img;
        _cardImage.sprite = _cardImg;
        gameObject.name = cardSuitString + "" + cardNumberString;
    }

    

    /*public void returnCardsToDeck()
    {
        for (int i = 0; i < drawCards.instance.playedCards.Count; i++)
        {
            lastCard = drawCards.instance.playedCards[i];
            drawCards.instance.playedCards[i].transform.SetParent(drawCards.instance.deckArray.transform, false);
        }
    }*/
    #endregion
}
