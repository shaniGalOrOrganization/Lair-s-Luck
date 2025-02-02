using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    #region Variable
    public bool AnnounceLie;
    public bool isPlayerTurn = true;
    //public static bool isEnemyTurn = true;
    public bool PlayerAnnounceLie = false;
    public bool EnemyAnnounceLie = false;
    public bool EndFlag = false;
    public GameObject DeckArray;
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject RealEnemyCardArea;
    public GameObject Dropzone;
    public GameObject DropZoneStack;
    public int currentlast;
    public int currentprev;
    public int beforeprev;
    public int afterprev;
    public int beforelast;
    public int afterlast;
    public Button LairButton;
    public TMP_Text botTextMessage;
    public TMP_Text liarTextMessage;
    public int chosenNumber;
    public Deck deck;
    public List<GameObject> cards = new List<GameObject>();

    [SerializeField] private GameObject winPopupPanel; 
    [SerializeField] private TextMeshProUGUI winPopupText;
    public  static GameManager instance { get; private set; }

    //public GameObject bot;
    //private liarsLuckBot botScript;

    #endregion

    #region MonoBehaviour

    public void Start()
    {
        winPopupPanel.SetActive(false);
        DrawCards.instance.initGame();
    }

    //public void Start()
    //{
    //    //botScript = bot.GetComponent<liarsLuckBot>();

    //    //List<int> botCards = new List<int>();
    //    //// TBD:
    //    //// for ....
    //    ////    botCards.Add(cardNumber);
    //    //totalUnseenCards = 52 - 10;
    //    //botScript.InitializeBot(initialHand,totalUnseenCards);
    //}

    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
        liarTextMessage.gameObject.SetActive(false);
    }


    #endregion

    #region Logic

    public void onButtonAceClicked()
    {
        Debug.Log("Button Ace clicked");
        chosenNumber = 1;
        liarsLuckBot.Instance.OnLiarCardSelected(1);
        string cardName = GetCardNameGameManager(1);
    }

    public void onButtonTwoClicked()
    {
        Debug.Log("Button Two clicked");
        chosenNumber = 2 ;
        liarsLuckBot.Instance.OnLiarCardSelected(2);
        string cardName = GetCardNameGameManager(2);
    }

    public void onButtonThreeClicked()
    {
        Debug.Log("Button Three clicked");
        chosenNumber = 3;
        liarsLuckBot.Instance.OnLiarCardSelected(3);
        string cardName = GetCardNameGameManager(3);
    }

    public void onButtonFourClicked()
    {
        Debug.Log("Button four clicked");
        checkchosencard(4);
        chosenNumber = 4;
        liarsLuckBot.Instance.OnLiarCardSelected(4);
        string cardName = GetCardNameGameManager(4);
    }

    public void onButtonFiveClicked()
    {
        Debug.Log("Button five clicked");
        chosenNumber = 5;
        liarsLuckBot.Instance.OnLiarCardSelected(5);
        string cardName = GetCardNameGameManager(5);
    }

    public void onButtonSixClicked()
    {
        Debug.Log("Button six clicked");
        checkchosencard(6);
        chosenNumber = 6;
        liarsLuckBot.Instance.OnLiarCardSelected(6);
        string cardName = GetCardNameGameManager(6);
    }

    public void onButtonSevenClicked()
    {
        Debug.Log("Button seven clicked");
        checkchosencard(7);
        chosenNumber = 7;
        liarsLuckBot.Instance.OnLiarCardSelected(7);
        string cardName = GetCardNameGameManager(7);
    }

    public void onButtonEightClicked()
    {
        Debug.Log("Button Eight clicked");
        chosenNumber = 8;
        liarsLuckBot.Instance.OnLiarCardSelected(8);
        string cardName = GetCardNameGameManager(8);
    }

    public void onButtonNineClicked()
    {
        Debug.Log("Button nine clicked");
        checkchosencard(9);
        chosenNumber = 9;
        liarsLuckBot.Instance.OnLiarCardSelected(9);
        string cardName = GetCardNameGameManager(9);
    }

    public void onButtonTenClicked()
    {
        Debug.Log("Button ten clicked");
        checkchosencard(10);
        chosenNumber = 10;
        liarsLuckBot.Instance.OnLiarCardSelected(10);
        string cardName = GetCardNameGameManager(10);
    }

    public void onButtonJackClicked()
    {
        Debug.Log("Button Jack clicked");
        chosenNumber = 11;
        liarsLuckBot.Instance.OnLiarCardSelected(11);
        string cardName = GetCardNameGameManager(11);
    }

    public void onButtonQueenClicked()
    {
        Debug.Log("Button queen clicked");
        checkchosencard(12);
        chosenNumber = 12;
        liarsLuckBot.Instance.OnLiarCardSelected(12);
        string cardName = GetCardNameGameManager(12);
    }

    public void onButtonKingClicked()
    {
        Debug.Log("Button king clicked");
        checkchosencard(13);
        chosenNumber = 13;
        liarsLuckBot.Instance.OnLiarCardSelected(13);
        string cardName = GetCardNameGameManager(13);
    }

    public void checkchosencard(int ButtonChoosed)
    {
        //transform.SetParent(Dropzone.transform, false);
        int childCount = DropZoneStack.transform.childCount;
        if (childCount > 0)
        {
            Transform lastChild = DropZoneStack.transform.GetChild(childCount - 1);
            //Transform prevChild = Dropzone.transform.GetChild(childCount - 2);
            //Transform prevChild = Dropzone.transform.GetChild(childCount - 2);

            Card lastCardData = lastChild.GetComponent<Card>();

            if (lastCardData != null)
            {
                Debug.Log($"Last card check in drop zone: {lastCardData.cardNumberString} of {lastCardData.cardSuitString}");

                if (Enum.TryParse<Card.Number>(lastCardData.cardNumberString, true, out Card.Number currentNumber))
                {
                    currentlast = (int)currentNumber;
                    beforelast = currentlast - 1;
                    if (beforelast < 1) beforelast = 13;
                    afterlast = currentlast + 1;
                    if (afterlast > 13) afterlast = 1;
                    Debug.Log($"Current last: {currentlast}, Before last: {beforelast}, After last: {afterlast}");
                }
                else
                {
                    Debug.LogError($"Invalid card number string: {lastCardData.cardNumberString}");
                }
            }

            if ((ButtonChoosed >= 1) && (ButtonChoosed <= 13))
            {
                Debug.Log($"Prev card called in drop zone: {ButtonChoosed} ");
                currentprev = ButtonChoosed;
                beforeprev = currentprev - 1;
                if (beforeprev < 1) beforeprev = 13;
                afterprev = currentprev + 1;
                if (afterprev > 13) afterprev = 1;
                Debug.Log($"Current prev: {currentprev}, Before prev: {beforeprev}, After prev: {afterprev}");
            }

            Debug.Log($"currentlast:{currentlast}, beforeprev: {beforeprev},afterprev: {afterprev}, currentprev:{currentprev}");


            //if ((currentlast != currentprev) && (currentlast != beforeprev) && (currentlast != afterprev))
            if(currentlast != currentprev)
            {
                AnnounceLie = true;
              //  Debug.Log("Liar");
                Debug.Log("Liar");
            }
            else
            {
                AnnounceLie = false;
                //Debug.Log("Not liar");
                Debug.Log("Not liar");
            }

            Debug.Log(AnnounceLie);

            if (isPlayerTurn)
            {
                EnemyAnnounceLie = AnnounceLie;
            }
            else
            {
                PlayerAnnounceLie = AnnounceLie;
            }
        }
    }

    public void BTN_Lair()
    {
        string message = "";
        checkchosencard(chosenNumber);
        if (isPlayerTurn)
        {
            if (EnemyAnnounceLie)
            {
                //הבוט שיקר צריך להזיז את הקלפים אל היד של הבוט 
                for (int i = DropZoneStack.transform.childCount - 1; i >= 1; i--)
                {
                    Transform card = DropZoneStack.transform.GetChild(i);
                    int cardNumber = liarsLuckBot.Instance.GetCardNumber(card.GetComponent<Card>().cardNumberString);
                    liarsLuckBot.Instance.cardCounts[cardNumber]++;
                    card.SetParent(RealEnemyCardArea.transform, false);
                }
                Debug.Log("Bot was lying! Cards moved to the bot's hand.");
                message = $"Bot was lying!";
                StartCoroutine(showLairMessage(message, 3f));
                //liarsLuckBot.Instance.SyncEnemyArea();
            }
            else
            {
                for (int i = DropZoneStack.transform.childCount - 1; i >= 1; i--)
                {
                    Transform card = DropZoneStack.transform.GetChild(i);
                    int cardNumber = liarsLuckBot.Instance.GetCardNumber(card.GetComponent<Card>().cardNumberString);
                    Debug.Log($"linoy check {card.GetComponent<Card>().cardNumberString}");
                    liarsLuckBot.Instance.cardCounts[cardNumber]--;
                    card.SetParent(PlayerArea.transform, false);
                }
                Debug.Log("Bot was truthful. Cards moved to the player's hand.");
                message = $"Bot was truthful";
                StartCoroutine(showLairMessage(message, 3f));
            }
        }
        else
        {
            if (PlayerAnnounceLie)
            {
                //השחקן שיקר צריך להזיז את הקלפים אל היד של השחקן
                for (int i = DropZoneStack.transform.childCount - 1; i >= 1; i--)
                {
                    Transform card = DropZoneStack.transform.GetChild(i);
                    int cardNumber = liarsLuckBot.Instance.GetCardNumber(card.GetComponent<Card>().cardNumberString);
                    liarsLuckBot.Instance.cardCounts[cardNumber]--;
                    card.SetParent(PlayerArea.transform, false);
                   
                }
                Debug.Log("Player was lying! Cards moved to the player's hand.");
                message = $"Player was lying!";
                StartCoroutine(showLairMessage(message, 3f));
            }
            else
            {
                for (int i = DropZoneStack.transform.childCount - 1; i >= 1; i--)
                {
                    Transform card = DropZoneStack.transform.GetChild(i);
                    int cardNumber = liarsLuckBot.Instance.GetCardNumber(card.GetComponent<Card>().cardNumberString);
                    liarsLuckBot.Instance.cardCounts[cardNumber]++;
                    card.SetParent(RealEnemyCardArea.transform, false);
                }
                Debug.Log("Player was truthful.Cards moved to the bot's hand.");
                message = $"Player was truthful";
                StartCoroutine(showLairMessage(message, 3f));
               // liarsLuckBot.Instance.SyncEnemyArea();

            }
        }

        SyncEnemyArea();
    }

    public void TransferCardAndHide()
    {
        int dropzoneCount = Dropzone.transform.childCount;

        if (dropzoneCount > 0)
        {
            // Get the last card in the Dropzone
            Transform topCard = Dropzone.transform.GetChild(dropzoneCount - 1);

            // Instantiate the card back at the Dropzone
            GameObject cardBack = Instantiate(Card2, Dropzone.transform.position, Quaternion.identity);
            cardBack.transform.SetParent(Dropzone.transform, false);

            // Move the original card to the target area
            topCard.SetParent(DropZoneStack.transform, false);

            Debug.Log($"Card transferred to {DropZoneStack.name} and card back placed in Dropzone.");
        }
        else
        {
            Debug.LogWarning("Dropzone is empty. No card to transfer.");
        }
    }

    // Coroutine to show the bot's message for a few seconds
    public IEnumerator showBotMessage(string message, float duration)
    {
        botTextMessage.text = message; // Set the message
        botTextMessage.gameObject.SetActive(true); // Show the message

        yield return new WaitForSeconds(duration); // Wait for the specified duration
        botTextMessage.gameObject.SetActive(false); // Hide the message
    }

    public IEnumerator showLairMessage(string message, float duration)
    {
        liarTextMessage.text = message;
        liarTextMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);
        liarTextMessage.gameObject.SetActive(false);
    }

    private string GetCardNameGameManager(int cardNumber)
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

    public void CheckWinCondition()
    {
        int botCardCount = GameManager.instance.RealEnemyCardArea.transform.childCount;
        int playerCardCount = GameManager.instance.PlayerArea.transform.childCount;

        if (botCardCount == 0)
        {
            ShowWinPopup(false); // Bot wins
            EndFlag = true;
        }
        else if (playerCardCount == 0)
        {
            ShowWinPopup(true); // Player wins
            EndFlag = true;
        }
        EndFlag = false;
    }

    private void ShowWinPopup(bool playerWon)
    {
        if (winPopupPanel != null && winPopupText != null)
        {
            winPopupText.text = playerWon ? "Congratulations! You won!" : "Game Over - Bot wins!";
            winPopupPanel.SetActive(true);
        }

        Debug.Log(playerWon ? "Player won!" : "Bot won!");
    }

    public void BTN_Replay()
    {
        ////Debug.Log(Dropzone.transform.childCount, Dropzone.gameObject);
        //Transform firstCard = Dropzone.transform.GetChild(0);
        //firstCard.SetParent(DeckArray.transform, false);
        ////Debug.Log(Dropzone.transform.childCount, Dropzone.gameObject);
        ////Debug.Log(firstCard.name ,firstCard.gameObject);
        //returnCardsToDeck(DropZoneStack);
        //returnCardsToDeck(PlayerArea);
        //returnCardsToDeck(RealEnemyCardArea);

        //foreach (Transform child in EnemyArea.transform)
        //{
        //    Destroy(child.gameObject);
        //}

        //foreach (Transform child in Dropzone.transform)
        //{
        //    Destroy(child.gameObject);
        //}

        Debug.Log($"Dropzone : {Dropzone.transform.childCount}, DropZoneStack : {DropZoneStack.transform.childCount},PlayerArea : {PlayerArea.transform.childCount}" +
            $",RealEnemyCardArea : {RealEnemyCardArea.transform.childCount},EnemyArea : {EnemyArea.transform.childCount}, DeckArray: {DeckArray.transform.childCount}");

        destroyCards(Dropzone);
        destroyCards(DropZoneStack);
        destroyCards(PlayerArea);
        destroyCards(RealEnemyCardArea);
        destroyCards(EnemyArea);
        destroyCards(DeckArray);
        deck.cards.Clear();

        Debug.Log($"Dropzone : {Dropzone.transform.childCount}, DropZoneStack : {DropZoneStack.transform.childCount},PlayerArea : {PlayerArea.transform.childCount}" +
            $",RealEnemyCardArea : {RealEnemyCardArea.transform.childCount},EnemyArea : {EnemyArea.transform.childCount}, DeckArray: {DeckArray.transform.childCount}");
        isPlayerTurn = true;
        PlayerAnnounceLie = false;
        EnemyAnnounceLie = false;
        EndFlag = false;
        if((Dropzone.transform.childCount == 0) && (DropZoneStack.transform.childCount == 0) && (PlayerArea.transform.childCount == 0) && (RealEnemyCardArea.transform.childCount == 0) && 
            (EnemyArea.transform.childCount == 0) && (DeckArray.transform.childCount == 0))
        {
            DrawCards.instance.initGame();
            winPopupPanel.SetActive(false);
        }
            
        
    }

    public void destroyCards(GameObject area)
    {
        for (int i = area.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(area.transform.GetChild(i).gameObject);
        }
    }
    public void returnCardsToDeck(GameObject area)
    {
        for (int i = area.transform.childCount-1; i>=0; i--)
        {
            area.transform.GetChild(i).SetParent(DeckArray.transform, false);
        }
    }

    public void SyncEnemyArea()
    {
        Transform enemyArea = EnemyArea.transform;
        Transform realEnemyCardArea = RealEnemyCardArea.transform;

        int enemyCardCount = enemyArea.childCount;
        int realEnemyCardCount = realEnemyCardArea.childCount;

        // Remove excess cards from EnemyArea
        if (enemyCardCount > realEnemyCardCount)
        {
            for (int i = 0; i < (enemyCardCount - realEnemyCardCount); i++)
            {
                Destroy(enemyArea.GetChild(0).gameObject);
            }
        }

        // Add missing cards to EnemyArea
        else if (enemyCardCount < realEnemyCardCount)
        {
            for (int i = 0; i < (realEnemyCardCount - enemyCardCount); i++)
            {
                GameObject enemyCard = Instantiate(Card2, Vector3.zero, Quaternion.identity);
                enemyCard.transform.SetParent(enemyArea, false);
            }
        }
    }

    #endregion
}
