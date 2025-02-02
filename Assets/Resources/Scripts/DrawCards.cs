using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Awake()
    {
        instance = this;
    }
    #endregion

    #region Logic
    public void OnClick()
    {
        try
        {
            Card newCard = GameManager.instance.deck.drawCard();
            if (newCard == null)
                return;

            if (GameManager.instance.isPlayerTurn)
            {
                newCard.transform.SetParent(GameManager.instance.PlayerArea.transform, false);
                GameManager.instance.isPlayerTurn = false;
                liarsLuckBot.Instance.BotMoves(GameManager.instance.chosenNumber);
            }
            else
            {
                newCard.transform.SetParent(GameManager.instance.RealEnemyCardArea.transform, false);
                GameManager.instance.isPlayerTurn = true;
                liarsLuckBot.Instance._unityButtonLair["Button_Cheat"].GetComponent<Button>().interactable = true;
                GameManager.instance.SyncEnemyArea();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in OnClick: {e.Message}");
        }
    }

    public void initGame()
    {
        try
        {
            GameManager.instance.deck.createDeck();
            for (var i = 0; i < 10; i++)
            {
                Card playerCard = GameManager.instance.deck.drawCard();
                if (playerCard != null)
                {
                    playerCard.transform.SetParent(GameManager.instance.PlayerArea.transform, false);
                }

                GameObject enemyCard = Instantiate(GameManager.instance.Card2, new Vector3(0, 0, 0), Quaternion.identity);
                if (enemyCard != null)
                {
                    enemyCard.transform.SetParent(GameManager.instance.EnemyArea.transform, false);
                }

                Card realEnemyCard = GameManager.instance.deck.drawCard();
                if (realEnemyCard != null)
                {
                    realEnemyCard.transform.SetParent(GameManager.instance.RealEnemyCardArea.transform, false);
                }
            }

            Card firstCard = GameManager.instance.deck.drawCard();
            if (firstCard != null)
            {
                firstCard.transform.SetParent(GameManager.instance.Dropzone.transform, false);
                Card firstCardDup = Instantiate(firstCard, GameManager.instance.DropZoneStack.transform, false);
            }

            liarsLuckBot.Instance.InitializeBot();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in initGame: {e.Message}");
        }
    }
    #endregion
}