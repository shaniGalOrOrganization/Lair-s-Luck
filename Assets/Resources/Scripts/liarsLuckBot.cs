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
    public int[] cardCounts = new int[14]; // index=1 -> ace, index=2 -> 2, index=11 -> jack, index=12 -> queen, index=13 -> king , (index 0 is unused)
    public int CheckCheatFlag = 0;
    public Dictionary<string, GameObject> _unityButtonLair = new Dictionary<string, GameObject>();


    //[SerializeField] private TextMeshProUGUI announcementText; // Reference to UI text component
    //[SerializeField] private GameObject winPopupPanel; // Reference to popup panel
    //[SerializeField] private TextMeshProUGUI winPopupText; // Reference to text in popup

    //private bool isBluffing = false;
    private int lastPlayedCard = -1;

    #endregion
    //public int[] cardCounts = new int[14]; // Tracks how many cards of each rank have been played (index 0 is unused)
    //private List<int> hand; // Bot's current hand
    //public int unseenCardsCount; // Total number of unseen cards
    //private System.Random random = new System.Random();
    private GameManager gameManager; // Reference to the GameManager

    private void Start()
    {
        Invoke("InitializeBot", 0.2f);
    }

    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.botTextMessage.gameObject.SetActive(false);
        GameObject[] curGameObject = GameObject.FindGameObjectsWithTag("unityButtonLair");
        foreach (GameObject obj in curGameObject)
        {
            _unityButtonLair.Add(obj.name, obj);
        }

        // Hide win popup at start
        //if (winPopupPanel != null)
        //    winPopupPanel.SetActive(false);
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
                int cardNumber = GetCardNumber(card.cardNumberString); // Assuming cardNumber represents the value of the card (2 to 14)
                if (cardNumber >= 1 && cardNumber <= 13)
                {
                    cardCounts[cardNumber]++;
                }
            }
        }
        cardCounts[GameManager.instance.DropZoneStack.transform.GetChild(0).GetSiblingIndex()]++;

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
            Debug.Log("Im here!");
            cardCounts[buttonNum]++;
        }

        //for (int i = 1; i < cardCounts.Length; i++)
        //{
        //    Debug.Log($"Bot Card recived {i}: {cardCounts[i]}");
        //}

        GameManager.instance.isPlayerTurn = false;
        //GameManager.instance.checkchosencard(buttonNum);
        GameManager.instance.CheckWinCondition();
        if (GameManager.instance.EndFlag == false)
        {
            BotMoves(buttonNum);
        }
    }

    //public void BotMoves(int buttonNumPlayerChoose)
    //{
    //    if (cardCounts[buttonNumPlayerChoose] >= 4)
    //    {
    //        GameManager.instance.BTN_Lair();
    //        CheckCheatFlag = 1;
    //    }

    //    if (CheckCheatFlag == 0)
    //    {
    //        foreach (Transform cardTransform in GameManager.instance.RealEnemyCardArea.transform)
    //        {
    //            Card card = cardTransform.GetComponent<Card>(); // Assuming each card has a Card script attached
    //            if (card != null)
    //            {
    //                int cardNumber = GetCardNumber(card.cardNumberString);// int.Parse(card.cardNumberString); // Assuming cardNumber represents the value of the card (2 to 14)
    //                if ((cardNumber == buttonNumPlayerChoose) || (cardNumber == buttonNumPlayerChoose + 1) || (cardNumber == buttonNumPlayerChoose - 1))
    //                {
    //                    cardTransform.SetParent(GameManager.instance.DropZoneStack.transform, false);

    //                    foreach (var pair in DragDrop.Instance._unityButtonsLairChoose)
    //                    {
    //                        Button button = pair.Value.GetComponent<Button>();
    //                        if (button != null)
    //                        {
    //                            button.interactable = false;
    //                        }
    //                    }

    //                    switch (cardNumber)
    //                    {
    //                        case 1:
    //                            Debug.Log("Yes1");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 2:
    //                            Debug.Log("Yes2");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 3:
    //                            Debug.Log("Yes3");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 4:
    //                            Debug.Log("Yes4");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 5:
    //                            Debug.Log("Yes5");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 6:
    //                            Debug.Log("Yes6");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 7:
    //                            Debug.Log("Yes7");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 8:
    //                            Debug.Log("Yes8");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 9:
    //                            Debug.Log("Yes9");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 10:
    //                            Debug.Log("Yes10");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 11:
    //                            Debug.Log("Yes11");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 12:
    //                            Debug.Log("Yes12");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        case 13:
    //                            Debug.Log("Yes13");
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //current
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //after
    //                            DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //before
    //                            break;
    //                        default:
    //                            Debug.Log($"Yes{cardNumber}");
    //                            break;
    //                    }
    //                    break;
    //                }
    //                //else
    //                //{
    //                //    if ((GameManager.instance.DropZoneStack.transform.childCount <= 3) && (GameManager.instance.RealEnemyCardArea.transform.childCount >= 4))
    //                //    {
    //                //        int childCount = GameManager.instance.RealEnemyCardArea.transform.childCount;
    //                //        Transform lastChild = GameManager.instance.RealEnemyCardArea.transform.GetChild(childCount - 1);
    //                //        lastChild.SetParent(GameManager.instance.DropZoneStack.transform, false);
    //                //        //Need to do "on" to one of the buttons
    //                //    }
    //                //    //else
    //                //    //{
    //                //    //    DrawCards.instance.OnClick();
    //                //    //}
    //                //}
    //            }
    //        }

    //        CheckCheatFlag = 0;
    //    }

    //    GameManager.instance.isPlayerTurn = true;
    //    _unityButtonLair["Button_Cheat"].GetComponent<Button>().interactable = true;
    //}


    public void BotMoves(int buttonNumPlayerChoose)
    {
        // Reset bluffing status
        //isBluffing = false;
        lastPlayedCard = -1;
       // GameManager.instance.checkchosencard(cardNumber);
        // First check if player was lying
        if (cardCounts[buttonNumPlayerChoose] >= 3)
        {
            cardCounts[buttonNumPlayerChoose] = 0;
            GameManager.instance.BTN_Lair();
            CheckCheatFlag = 1;
            return;
        }

        CheckCheatFlag = 0;
        bool cardPlayed = false;

        // Try to play a valid card first
        foreach (Transform cardTransform in GameManager.instance.RealEnemyCardArea.transform)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {

                int cardNumber = GetCardNumber(card.cardNumberString);
                if ((cardNumber == buttonNumPlayerChoose) ||
                    (cardNumber == buttonNumPlayerChoose + 1) ||
                    (cardNumber == buttonNumPlayerChoose - 1))
                {
                    cardTransform.SetParent(GameManager.instance.DropZoneStack.transform, false);
                    lastPlayedCard = cardNumber;
                    cardPlayed = true;
                    GameManager.instance.SyncEnemyArea();
                    
                    foreach (var pair in DragDrop.Instance._unityButtonsLairChoose)
                    {
                        Button button = pair.Value.GetComponent<Button>();
                        if (button != null)
                        {
                            button.interactable = false;
                        }
                    }
                    
                    switch (cardNumber)
                    {
                        case 1:
                            Debug.Log("Yes1");
                            GameManager.instance.chosenNumber = 1;
                            DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 2:
                            Debug.Log("Yes2");
                            GameManager.instance.chosenNumber = 2;
                            DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 3:
                            Debug.Log("Yes3");
                            GameManager.instance.chosenNumber = 3;
                            DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 4:
                            Debug.Log("Yes4");
                            GameManager.instance.chosenNumber = 4;
                            DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 5:
                            Debug.Log("Yes5");
                            GameManager.instance.chosenNumber = 5;
                            DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 6:
                            Debug.Log("Yes6");
                            GameManager.instance.chosenNumber = 6;
                            DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 7:
                            Debug.Log("Yes7");
                            GameManager.instance.chosenNumber = 7;
                            DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 8:
                            Debug.Log("Yes8");
                            GameManager.instance.chosenNumber = 8;
                            DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 9:
                            Debug.Log("Yes9");
                            GameManager.instance.chosenNumber = 9;
                            DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 10:
                            Debug.Log("Yes10");
                            GameManager.instance.chosenNumber = 10;
                            DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 11:
                            Debug.Log("Yes11");
                            GameManager.instance.chosenNumber = 11;
                            DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 12:
                            Debug.Log("Yes12");
                            GameManager.instance.chosenNumber = 12;
                            DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 13:
                            Debug.Log("Yes13");
                            GameManager.instance.chosenNumber = 13;
                            DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //current
                            DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //after
                            DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //before
                            break;
                        default:
                            Debug.Log($"Yes{cardNumber}");
                            break;
                    }

                    break;
                }
            }
        }

        // If no valid card found, bot must bluff
        if (!cardPlayed)
        {
            PlayBluffCard(buttonNumPlayerChoose);
        }
        else
        {
            AnnounceCardPlayed(lastPlayedCard, false);
        }
        GameManager.instance.CheckWinCondition();
        if (GameManager.instance.EndFlag == false)
        {
            // Switch turns and update UI
            GameManager.instance.isPlayerTurn = true;
            _unityButtonLair["Button_Cheat"].GetComponent<Button>().interactable = true;
        }
        //EnablePlayerControls();

        
    }

    private void PlayBluffCard(int currentNumber)
    {
        //isBluffing = true;
        int childCount = GameManager.instance.RealEnemyCardArea.transform.childCount;

        if (childCount > 0)
        {
            // Choose random card from hand
            int randomIndex = UnityEngine.Random.Range(0, childCount);
            Transform randomCard = GameManager.instance.RealEnemyCardArea.transform.GetChild(randomIndex);
            randomCard.SetParent(GameManager.instance.DropZoneStack.transform, false);

            Card cardRand = randomCard.GetComponent<Card>();
            int cardNumberRand = GetCardNumber(cardRand.cardNumberString);

            // Announce a valid number even though we're bluffing
            lastPlayedCard = GetValidNumber(currentNumber);        

            foreach (var pair in DragDrop.Instance._unityButtonsLairChoose)
            {
                Button button = pair.Value.GetComponent<Button>();
                if (button != null)
                {
                    button.interactable = false;
                }
            }

            switch (lastPlayedCard)
            {
                case 1:
                    Debug.Log("Yes1");
                    DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //before
                    break;
                case 2:
                    Debug.Log("Yes2");
                    DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //before
                    break;
                case 3:
                    Debug.Log("Yes3");
                    DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //before
                    break;
                case 4:
                    Debug.Log("Yes4");
                    DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //before
                    break;
                case 5:
                    Debug.Log("Yes5");
                    DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //before
                    break;
                case 6:
                    Debug.Log("Yes6");
                    DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //before
                    break;
                case 7:
                    Debug.Log("Yes7");
                    DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //before
                    break;
                case 8:
                    Debug.Log("Yes8");
                    DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //before
                    break;
                case 9:
                    Debug.Log("Yes9");
                    DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //before
                    break;
                case 10:
                    Debug.Log("Yes10");
                    DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //before
                    break;
                case 11:
                    Debug.Log("Yes11");
                    DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //before
                    break;
                case 12:
                    Debug.Log("Yes12");
                    DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //before
                    break;
                case 13:
                    Debug.Log("Yes13");
                    DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //current
                    DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //after
                    DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //before
                    break;
                default:
                    Debug.Log($"Yes{lastPlayedCard}");
                    break;
            }

            AnnounceCardPlayed(lastPlayedCard, true);
        }
        //else
        //{
        //    // Bot has no cards - game should end
        //    CheckWinCondition();
        //}
    }

    private void AnnounceCardPlayed(int cardNumber, bool isBluff)
    {
        string cardName = GetCardName(cardNumber);
        string announcement = $"Bot played: {cardName}";
        string message = $"Bot declared: {cardName}";
        //need to output text "Bot Played {cardName}"
        //StartCoroutine(showBotMessage($"Bot declared: {cardName}", 2f));  // Show for 2 seconds
        StartCoroutine(gameManager.showBotMessage(message, 3f)); // Show message for 2 seconds

        //if (announcementText != null)
        //{
        //    announcementText.text = announcement;
        //}
        Debug.Log(announcement + (isBluff ? " (Bluff)" : ""));
    }

    //private void EnablePlayerControls()
    //{
    //    _unityButtonLair["Button_Cheat"].GetComponent<Button>().interactable = true;

    //    foreach (var pair in DragDrop.Instance._unityButtonsLairChoose)
    //    {
    //        Button button = pair.Value.GetComponent<Button>();
    //        if (button != null)
    //        {
    //            button.interactable = true;
    //        }
    //    }
    //}

    private void UpdateCardCounts(int cardNum)
    {
        if (cardNum >= 1 && cardNum <= 13)
        {
            cardCounts[cardNum]++;
        }
    }

    private int GetValidNumber(int currentNumber)
    {
        int[] validNumbers = new int[3];
        validNumbers[0] = currentNumber;
        validNumbers[1] = (currentNumber + 1 > 13) ? 1 : currentNumber + 1;
        validNumbers[2] = (currentNumber - 1 < 1) ? 13 : currentNumber - 1;
        return validNumbers[UnityEngine.Random.Range(0, 3)];
    }

    private string GetCardName(int cardNumber)
    {
        switch (cardNumber)
        {
            case 1:
                return "Ace";
            case 11:
                return "Jack";
            case 12:
                return "Queen";
            case 13:
                return "King";
            default:
                return cardNumber.ToString();
        }
    }

    public void OnPlayerDroppedCard(int cardNum)
    {
        Debug.Log($"Bot received player dropped card: {cardNum}");
    }

    public int GetCardNumber(string cardNumberString)
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

    /*public void SyncEnemyArea()
    {
        Transform enemyArea = GameManager.instance.EnemyArea.transform;
        Transform realEnemyCardArea = GameManager.instance.RealEnemyCardArea.transform;

        while (enemyArea.childCount > realEnemyCardArea.childCount)
        {
            Destroy(enemyArea.GetChild(0).gameObject);
        }

        while (enemyArea.childCount < realEnemyCardArea.childCount)
        {
            GameObject enemyCard = Instantiate(GameManager.instance.Card2, Vector3.zero, Quaternion.identity);
            enemyCard.transform.SetParent(enemyArea, false);
        }
    }*/

    #endregion
}