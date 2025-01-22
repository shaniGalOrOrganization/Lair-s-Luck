using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variable
    public bool AnnounceLie;
    public bool isPlayerTurn = true;
    public static bool isEnemyTurn = true;
    public bool PlayerAnnounceLie = false;

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

    public Deck deck;

    public List<GameObject> cards = new List<GameObject>();

    public  static GameManager instance { get; private set; }

    public GameObject bot;
    //private liarsLuckBot botScript;

    #endregion

    #region MonoBehaviour

    public void Start()
    {
        //liarsLuckBot.Instance = bot.GetComponent<liarsLuckBot>();
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
    }


    #endregion

    #region Logic

    public void onButtonAceClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(1);
        liarsLuckBot.Instance.OnLiarCardSelected(1);
    }

    public void onButtonTwoClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(2);
        liarsLuckBot.Instance.OnLiarCardSelected(2);
    }

    public void onButtonThreeClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(3);
        liarsLuckBot.Instance.OnLiarCardSelected(3);
    }

    public void onButtonFourClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(4);
        liarsLuckBot.Instance.OnLiarCardSelected(4);
    }

    public void onButtonFiveClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(5);
        liarsLuckBot.Instance.OnLiarCardSelected(5);
    }

    public void onButtonSixClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(6);
        liarsLuckBot.Instance.OnLiarCardSelected(6);
    }

    public void onButtonSevenClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(7);
        liarsLuckBot.Instance.OnLiarCardSelected(7);
    }

    public void onButtonEightClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(8);
        liarsLuckBot.Instance.OnLiarCardSelected(8);
    }

    public void onButtonNineClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(9);
        liarsLuckBot.Instance.OnLiarCardSelected(9);
    }

    public void onButtonTenClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(10);
        liarsLuckBot.Instance.OnLiarCardSelected(10);
    }

    public void onButtonJackClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(11);
        liarsLuckBot.Instance.OnLiarCardSelected(11);
    }

    public void onButtonQueenClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(12);

        liarsLuckBot.Instance.OnLiarCardSelected(12);
    }

    public void onButtonKingClicked()
    {
        Debug.Log("Button five clicked");
        checkchosencard(13);
        liarsLuckBot.Instance.OnLiarCardSelected(13);
    }

    public void checkchosencard(int ButtonChoosed)
    public void checkchosencard()
    {
        //transform.SetParent(Dropzone.transform, false);
        int childCount = Dropzone.transform.childCount;
        if (childCount > 0)
        {
            Transform lastChild = Dropzone.transform.GetChild(childCount - 1);
            //Transform prevChild = Dropzone.transform.GetChild(childCount - 2);
            Transform prevChild = Dropzone.transform.GetChild(childCount - 2);

            Card lastCardData = lastChild.GetComponent<Card>();
            //Card prevCardData = prevChild.GetComponent<Card>();
            Card prevCardData = prevChild.GetComponent<Card>();

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
            if (prevCardData != null)
            {
                Debug.Log($"Prev card called in drop zone: {ButtonChoosed} ");
                currentprev = ButtonChoosed;
                Debug.Log($"Prev card check in drop zone: {prevCardData.cardNumberString} of {prevCardData.cardSuitString}");

                if (Enum.TryParse<Card.Number>(prevCardData.cardNumberString, true, out Card.Number currentNumber))
                {
                    currentprev = (int)currentNumber;
                    beforeprev = currentprev - 1;
                    if (beforeprev < 1) beforeprev = 13;
                    afterprev = currentprev + 1;
                    if (afterprev > 13) afterprev = 1;
                    Debug.Log($"Current prev: {currentprev}, Before prev: {beforeprev}, After prev: {afterprev}");
                }
                else
                {
                    Debug.LogError($"Invalid card number string: {prevCardData.cardNumberString}");
                }
            }

            //debug printing
            // debug printing
            Debug.Log($"currentlast:{currentlast}, beforeprev: {beforeprev},afterprev: {afterprev}, currentprev:{currentprev}");


            if ((currentlast != currentprev) && (currentlast != beforeprev) && (currentlast != afterprev))
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
            }
            else
            {
            }
        }
    }

    public void BTN_Lair()
    {
        checkchosencard();
        if (isPlayerTurn)
        {
            if (EnemyAnnounceLie)
            {
                //הבוט שיקר צריך להזיז את הקלפים אל היד של הבוט 
                for (int i = DropZoneStack.transform.childCount - 1; i >= 0; i--)
                {
                    Transform card = DropZoneStack.transform.GetChild(i);
                    card.SetParent(RealEnemyCardArea.transform, false);
                }
                Debug.Log("Bot was lying! Cards moved to the bot's hand.");
            }
            else
            {
                for (int i = DropZoneStack.transform.childCount - 1; i >= 0; i--)
                {
                    Transform card = DropZoneStack.transform.GetChild(i);
                    card.SetParent(PlayerArea.transform, false);
                }
                Debug.Log("Bot was truthful. Cards moved to the player's hand.");
            }
        }
        else
        {
            if (PlayerAnnounceLie)
            {
                //השחקן שיקר צריך להזיז את הקלפים אל היד של השחקן
                for (int i = DropZoneStack.transform.childCount - 1; i >= 0; i--)
                {
                    Transform card = DropZoneStack.transform.GetChild(i);
                    card.SetParent(PlayerArea.transform, false);
                }
                Debug.Log("Player was lying! Cards moved to the player's hand.");
            }
            else
            {
                for (int i = DropZoneStack.transform.childCount - 1; i >= 0; i--)
                {
                    Transform card = DropZoneStack.transform.GetChild(i);
                    card.SetParent(RealEnemyCardArea.transform, false);
                }
                Debug.Log("Player was truthful.Cards moved to the bot's hand.");
                Debug.Log("Player was truthful. No action taken.");
            }
        }

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


    #endregion
}
