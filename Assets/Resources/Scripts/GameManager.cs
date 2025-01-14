using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    //public static bool PlayerAnnounceLie;
    //public static bool EnemyAnnounceLie;
    public static bool AnnounceLie;
    public bool isPlayerTurn = true;

    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject RealEnemyCardArea;
    public GameObject Dropzone;

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



    /*public void checkchosencard(Card card)// בודק בהכרזה אם הקלף ששמת שונה ממה שאמרת.
    {
        if ((card.cardNumber != TempCardValue) || (card.cardNumber != TempCardValue - 1) || card.cardNumber != TempCardValue + 1) // בודק אם הקלף ששמת הוא ולידי
        {
            AnnounceLie = true;
        }
        else
        {
            AnnounceLie = false;
        }

        if (isPlayerTurn)
        {
            PlayerAnnounceLie = AnnounceLie;
            Debug.Log(PlayerAnnounceLie + " (true = your Openning card was a lie , false =  Openning card wasn't a lie)");
        }

        else
        {
            EnemyAnnounceLie = AnnounceLie;
        }
    }*/

    public void BTN_Lair()
    {
        if(isPlayerTurn)
        {
            if (AnnounceLie)
            {
                //הבוט שיקר צריך להזיז את הקלפים אל היד של הבוט 
            }
        }
        else
        {
            if (AnnounceLie)
            {
                //השחקן שיקר צריך להזיז את הקלפים אל היד של השחקן
            }
        }

    }

    #endregion
}
