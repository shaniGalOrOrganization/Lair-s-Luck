using System.Collections;
using System.Collections.Generic;
// using System.Security.Permissions;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    #region Variables

    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject RealEnemyCardArea;
    public GameObject Dropzone;
    public Deck deck;

    public static DrawCards instance { get; private set; }
    public bool isPlayerTurn = true;
    public List<GameObject> cards = new List<GameObject>();

    #endregion

    #region MonoBehaviour
    void Start()
    {
        initGame();
    }

    void Awake()
    {
        instance = this;
    }

    #endregion

    #region Logic
    public void OnClick()
    {
        Card newCard = deck.drawCard();
        if(isPlayerTurn)
        {
            newCard.transform.SetParent(PlayerArea.transform, false);
        }
        else
        {
            newCard.transform.SetParent(RealEnemyCardArea.transform, false);
        }
    }

    public void initGame()
    {
        for (var i = 0; i < 10; i++)
        {
            Card playerCard = deck.drawCard();
            playerCard.transform.SetParent(PlayerArea.transform, false);

            GameObject enemyCard = Instantiate(Card2, new Vector3(0, 0, 0), Quaternion.identity);
            enemyCard.transform.SetParent(EnemyArea.transform, false);

            Card realEnemyCard = deck.drawCard();
            realEnemyCard.transform.SetParent(RealEnemyCardArea.transform, false);

        }
        Card firstCard = deck.drawCard();
        firstCard.transform.SetParent(Dropzone.transform, false);
    }

    //private Card GetCard(GameObject currentArea)
    //{
    //    Card drawnCard = deck.drawCard();
    //    drawnCard.transform.SetParent(currentArea.transform, false);
    //    return drawnCard;
    //}

    #endregion
}
