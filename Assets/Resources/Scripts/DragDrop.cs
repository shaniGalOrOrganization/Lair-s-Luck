using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour
{
    #region Variables

    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private Vector2 startPosition;
    private Dictionary<string, GameObject> _unityButtonsLairChoose = new Dictionary<string, GameObject>();
    #endregion


    public GameObject bot;
    private liarsLuckBot botScript;
    

    #region MonoBehaviour

    public void Start()
    {
        bot = GameObject.Find("Bot");
        botScript = bot.GetComponent<liarsLuckBot>();
    }

    //_unityButtonsLairChoose["name"].SetActive(true);
    void Awake()
    {


        GameObject[] curGameObject = GameObject.FindGameObjectsWithTag("ButtonLairChoose");
        foreach (GameObject obj in curGameObject)
        {
            _unityButtonsLairChoose.Add(obj.name, obj);
        }
        foreach (var pair in _unityButtonsLairChoose) 
        {
            Button button = pair.Value.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = false; 
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("dropZone"))
        {
            isOverDropZone = true;
            dropZone = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("dropZone"))
        {
            isOverDropZone = false;
            dropZone = null;
        }
    }

    #endregion

    #region Logic
   

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    private void NotifyBotAboutLastDroppedCard(Transform cardTransform)
    {
        Card lastCardDataForBot = cardTransform.GetComponent<Card>();
        if (lastCardDataForBot != null)
        {
            Debug.Log($"For bot: Last card in drop zone: {lastCardDataForBot.cardNumberString} of {lastCardDataForBot.cardSuitString}");

            if (Enum.TryParse<Card.Number>(lastCardDataForBot.cardNumberString, true, out Card.Number currentNumberForBot))
            {
                int currentIntnum = (int)currentNumberForBot;
                botScript.OnPlayerDroppedCard(currentIntnum);
            }
            else
            {
                Debug.LogError($"In Notify Bot: Invalid card number string: {lastCardDataForBot.cardNumberString}");
            }
        }
    }

    public void EndDrag()
    {
        int current;
        isDragging = false;
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);

            // Get the last child of the dropZone
            int childCount = dropZone.transform.childCount;
            if (childCount > 0)
            {
                Debug.Log("--- Player dropped a card");

                Transform lastCardForBot = dropZone.transform.GetChild(childCount - 1);

                NotifyBotAboutLastDroppedCard(lastCardForBot);


                Transform lastChild = dropZone.transform.GetChild(childCount - 2);
                Card lastCardData = lastChild.GetComponent<Card>();
                if (lastCardData != null)
                {
                    Debug.Log($"Last card in drop zone: {lastCardData.cardNumberString} of {lastCardData.cardSuitString}");

                    if (Enum.TryParse<Card.Number>(lastCardData.cardNumberString, true, out Card.Number currentNumber))
                    {
                        current = (int)currentNumber;
                        int before = current - 1;
                        if (before < 1) before = 13;
                        int after = current + 1;
                        if (after > 13) after = 1;
                        Debug.Log($"Current: {current}, Before: {before}, After: {after}");
                    }
                    else
                    {
                        Debug.LogError($"Invalid card number string: {lastCardData.cardNumberString}");
                    }
                    Debug.Log(currentNumber);
                    switch ((int)currentNumber)
                    {
                        case 1:
                            _unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 2:
                            _unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 3:
                            _unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 4:
                            _unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 5:
                            _unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 6:
                            _unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 7:
                            _unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 8:
                            _unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 9:
                            _unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 10:
                            _unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 11:
                            _unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 12:
                            _unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true; //before
                            break;
                        case 13:
                            _unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true; //current
                            _unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true; //after
                            _unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true; //before
                            break;
                        default:
                            Debug.Log((int)currentNumber);
                            break;
                    }
                }

                GameManager.instance.checkchosencard();
                GameManager.instance.LairButton.interactable = false;

            }
            else
            {
                transform.position = startPosition;
            }
        }

        #endregion
    }
}
