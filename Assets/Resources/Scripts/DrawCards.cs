using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public Deck deck;

    public static DrawCards instance { get; private set; }

    public List<GameObject> cards = new List<GameObject>();

    void Start()
    {
        cards.Add(Card1);
        cards.Add(Card2);
    }

    void Awake()
    {
        instance = this;
    }

    public void OnClick()
    {
        for (var i = 0; i < 10; i++)
        {
            Card playerCard = deck.drawCard();
            playerCard.transform.SetParent(PlayerArea.transform, false);

            GameObject enemyCard = Instantiate(Card2, new Vector3(0, 0, 0), Quaternion.identity);
            enemyCard.transform.SetParent(EnemyArea.transform, false);
        }

        //Card newcard = GetCard(PlayerArea);
    }

    //private Card GetCard(GameObject currentArea)
    //{
    //    Card drawnCard = deck.drawCard();
    //    drawnCard.transform.SetParent(currentArea.transform, false);
    //    return drawnCard;
    //}
}
