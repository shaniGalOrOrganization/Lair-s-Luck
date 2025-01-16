using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variable
    public static bool AnnounceLie;
    public static bool isPlayerTurn = true;
    public static bool EnemyAnnounceLie = false;
    public static bool isEnemyTurn = true;
    public static bool PlayerAnnounceLie = false;

    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject RealEnemyCardArea;
    public GameObject Dropzone;

    public int currentlast;
    public int currentprev;
    public int beforeprev;
    public int afterprev;
    public int beforelast;
    public int afterlast;


    public Deck deck;

    public List<GameObject> cards = new List<GameObject>();

    public  static GameManager instance { get; private set; }


    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }


    #endregion

    #region Logic

    public void checkchosencard()
    {
        //transform.SetParent(Dropzone.transform, false);
        int childCount = Dropzone.transform.childCount;
        if (childCount > 1)
        {
            Transform lastChild = Dropzone.transform.GetChild(childCount - 1);
            Transform prevChild = Dropzone.transform.GetChild(childCount - 2);

            Card lastCardData = lastChild.GetComponent<Card>();
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

            if (prevCardData != null)
            {
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

            if ((currentlast != currentprev) && (currentlast != beforeprev) && (currentlast != afterprev))
            {
                GameManager.AnnounceLie = true;
                Debug.Log("Liar");
            }
            else
            {
                GameManager.AnnounceLie = false;
                Debug.Log("Not liar");
            }

            if (GameManager.isPlayerTurn)
            {
                GameManager.PlayerAnnounceLie = GameManager.AnnounceLie;
            }
            else
            {
                EnemyAnnounceLie = GameManager.AnnounceLie;
            }
        }
    }

    //public void BTN_Lair()
    //{
    //    if(GameManager.isPlayerTurn)
    //    {
    //        if (GameManager.AnnounceLie)
    //        {
    //            //הבוט שיקר צריך להזיז את הקלפים אל היד של הבוט 
    //        }
    //    }
    //    else
    //    {
    //        if (GameManager.AnnounceLie)
    //        {
    //            //השחקן שיקר צריך להזיז את הקלפים אל היד של השחקן
    //        }
    //    }

    //}

    #endregion
}
