using System.Collections;
using System.Collections.Generic;
// using System.Security.Permissions;
using UnityEngine;
using static GameManager;

public class DrawCards : MonoBehaviour
{
    #region Variables
    public static DrawCards instance { get; private set; }
    #endregion

    #region MonoBehaviour
    void Start()
    {
        initGame();
    }

    #endregion

    #region Logic

    void Awake()
    {
        instance = this;
    }

    public void OnClick()
    {
        Card newCard = GameManager.instance.deck.drawCard();
        if(GameManager.instance.isPlayerTurn)
        {
            newCard.transform.SetParent(GameManager.instance.PlayerArea.transform, false);
        }
        else
        {
            newCard.transform.SetParent(GameManager.instance.RealEnemyCardArea.transform, false);
        }
    }

    public void initGame()
    {
        for (var i = 0; i < 10; i++)
        {
            Card playerCard = GameManager.instance.deck.drawCard();
            playerCard.transform.SetParent(GameManager.instance.PlayerArea.transform, false);

            GameObject enemyCard = Instantiate(GameManager.instance.Card2, new Vector3(0, 0, 0), Quaternion.identity);
            enemyCard.transform.SetParent(GameManager.instance.EnemyArea.transform, false);

            Card realEnemyCard = GameManager.instance.deck.drawCard();
            realEnemyCard.transform.SetParent(GameManager.instance.RealEnemyCardArea.transform, false);

        }
        Card firstCard = GameManager.instance.deck.drawCard();
        firstCard.transform.SetParent(GameManager.instance.Dropzone.transform, false);
    }

    //private Card GetCard(GameObject currentArea)
    //{
    //    Card drawnCard = deck.drawCard();
    //    drawnCard.transform.SetParent(currentArea.transform, false);
    //    return drawnCard;
    //}

    #endregion
}
