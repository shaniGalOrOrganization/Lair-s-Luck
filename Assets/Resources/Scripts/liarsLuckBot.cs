using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class liarsLuckBot : MonoBehaviour
{
    #region Variables
    private static liarsLuckBot instance;
    public int cardNumber;
    public static int[] cardCounts = new int[14]; // index=1 -> ace, index=2 -> 2, index=11 -> jack, index=12 -> queen, index=13 -> king , (index 0 is unused)
    public int CheckCheatFlag = 0;
    private Dictionary<string, GameObject> _unityButtonLair = new Dictionary<string, GameObject>();

    #endregion
    //public int[] cardCounts = new int[14]; // Tracks how many cards of each rank have been played (index 0 is unused)
    //private List<int> hand; // Bot's current hand
    //public int unseenCardsCount; // Total number of unseen cards
    //private System.Random random = new System.Random();

    void Awake()
    {
        GameObject[] curGameObject = GameObject.FindGameObjectsWithTag("unityButtonLair");
        foreach (GameObject obj in curGameObject)
        {
            _unityButtonLair.Add(obj.name, obj);
        }
    }

    public void InitializeBot()
    {
        // Initialize cardCounts with 0 - the first cell will remain 0, for simplifying the logic
        for (int i = 1; i < cardCounts.Length; i++)
        {
            cardCounts[i] = 0;
        }

        foreach (Transform cardTransform in GameManager.instance.RealEnemyCardArea.transform)
        {
            Card card = cardTransform.GetComponent<Card>(); // Assuming each card has a Card script attached
            if (card != null)
            {
                int cardNumber = GetCardNumber(card.cardNumberString);// int.Parse(card.cardNumberString); // Assuming cardNumber represents the value of the card (2 to 14)
                if (cardNumber >= 1 && cardNumber <= 13)
                {
                    cardCounts[cardNumber]++;
                }
            }
        }

        //for (int i = 1; i < cardCounts.Length; i++)
        //{
        //    Debug.Log($"Bot Card {i}: {cardCounts[i]}");
        //}
    }

    //public void UpdateCardCount(int cardRank) {
    //    cardCounts[cardRank]++; // Increment count for known cards
    //    unseenCardsCount--;       // Decrease unseen card count
    //}

    //public double CalculateBluffProbability(int declaredCard)
    //{
    //    int totalCardsPerRank = 4; // Total cards of the rank in the entire deck
    //    int knownCards = cardCounts[declaredCard]; // Known played cards + cards in the bot's hand
    //    int remainingCards = totalCardsPerRank - knownCards; // Remaining possible cards

    //    // Avoid division by zero
    //    if (unseenCardsCount == 0) return 0;

    //    return (double)remainingCards / unseenCardsCount;
    //}

    //public bool ShouldCallBluff(int declaredCard)
    //{
    //    double bluffProbability = CalculateBluffProbability(declaredCard);

    //    // Call a bluff if the probability is high enough
    //    if (bluffProbability > 0.7) // Example threshold: 70% probability
    //    {
    //        return true; // call bluff
    //    }
    //    return false; // do not call bluff
    //}

    //public void ReceiveDrawnCard(int drawnCard)
    //{
    //    hand.Add(drawnCard);         // Add the drawn card to the bot's hand
    //    UpdateCardCount(drawnCard);  // Update the known cards
    //}

    //public int playCard(int currentCard)
    //{
    //    // Find valid cards to play (currentCard +- 1)
    //    List<int> validCards = hand.Where(card => card == currentCard - 1 || card == currentCard + 1).ToList();

    //    if (validCards.Count > 0)
    //    {
    //        // Play a valid card
    //        int chosenCard = validCards[random.Next(validCards.Count)];
    //        hand.Remove(chosenCard);
    //        return chosenCard; // Return the played card
    //    }

    //    // Decide whether to bluff
    //    else if (random.NextDouble() < 0.3)
    //    {
    //        int bluffRank = random.Next(1, 14);
    //        return bluffRank; // Return a random bluff rank
    //    }

    //    // If the bot can't play and decides not to bluff, it must draw a card
    //    else if (unseenCardsCount > 0)
    //    {
    //        return 0; // Signal the Game Manager to draw a card
    //    }

    //    else
    //    {
    //        // No cards to draw, bot must bluff
    //        return random.Next(1, 14); // Bluff with a random rank
    //    }
    //}

    #region Logic
    public static liarsLuckBot Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("SC_liarsLuckBot").GetComponent<liarsLuckBot>();
            }
            return instance;
        }
    }

    public void OnLiarCardSelected(int buttonNum)
    {
        Debug.Log($"Bot received liar button: {buttonNum}");

        if (buttonNum >= 1 && buttonNum <= 13)
        {
            cardCounts[buttonNum]++;
        }

        for (int i = 1; i < cardCounts.Length; i++)
        {
            Debug.Log($"Bot Card recived {i}: {cardCounts[i]}");
        }

        GameManager.instance.isPlayerTurn = false;
        BotMoves(buttonNum);
    }

    public void BotMoves(int buttonNumPlayerChoose)
    {
        if (cardCounts[buttonNumPlayerChoose] >= 4)
        {
            GameManager.instance.BTN_Lair();
            CheckCheatFlag = 1;
        }

        if (CheckCheatFlag == 0)
        {
            foreach (Transform cardTransform in GameManager.instance.RealEnemyCardArea.transform)
            {
                Card card = cardTransform.GetComponent<Card>(); // Assuming each card has a Card script attached
                if (card != null)
                {
                    int cardNumber = GetCardNumber(card.cardNumberString);// int.Parse(card.cardNumberString); // Assuming cardNumber represents the value of the card (2 to 14)
                    if ((cardNumber == buttonNumPlayerChoose) || (cardNumber == buttonNumPlayerChoose + 1) || (cardNumber == buttonNumPlayerChoose - 1))
                    {
                        cardTransform.SetParent(GameManager.instance.DropZoneStack.transform, false);
                        break;
                        // Need to do "on" to this button
                    }
                    //else
                    //{
                    //    if ((GameManager.instance.DropZoneStack.transform.childCount <= 3) && (GameManager.instance.RealEnemyCardArea.transform.childCount >= 4))
                    //    {
                    //        int childCount = GameManager.instance.RealEnemyCardArea.transform.childCount;
                    //        Transform lastChild = GameManager.instance.RealEnemyCardArea.transform.GetChild(childCount - 1);
                    //        lastChild.SetParent(GameManager.instance.DropZoneStack.transform, false);
                    //        //Need to do "on" to one of the buttons
                    //    }
                    //    //else
                    //    //{
                    //    //    DrawCards.instance.OnClick();
                    //    //}
                    //}
                }
            }

            CheckCheatFlag = 0;
        }

        GameManager.instance.isPlayerTurn = true;
        _unityButtonLair["Button_Cheat"].GetComponent<Button>().interactable = true;

        foreach (var pair in DragDrop.Instance._unityButtonsLairChoose)
        {
            Button button = pair.Value.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    public void OnPlayerDroppedCard(int cardNum)
    {
        Debug.Log($"Bot received player dropped card: {cardNum}");
    }

    private int GetCardNumber(string cardNumberString)
    {
        switch (cardNumberString)
        {
            case "ace":
                return 1;
            case "two":
                return 2;
            case "three":
                return 3;
            case "four":
                return 4;
            case "five":
                return 5;
            case "six":
                return 6;
            case "seven":
                return 7;
            case "eight":
                return 8;
            case "nine":
                return 9;
            case "ten":
                return 10;
            case "jack":
                return 11;
            case "queen":
                return 12;
            case "king":
                return 13;
            default:
                
                // Return -1 for invalid inputs
                Debug.LogWarning($"Invalid card number string: {cardNumberString}");
                return -1;
                
        }
    }

    #endregion
}