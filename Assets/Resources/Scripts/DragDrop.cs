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
    private Vector2 startPosition;
    public Dictionary<string, GameObject> _unityButtonsLairChoose = new Dictionary<string, GameObject>();
    private GameObject botScript;
    public static DragDrop Instance { get; private set; }
    //GameObject _lastDropZone;
    #endregion


    #region MonoBehaviour

    public void Start()
    {
        //  GameManager.instance.bot = GameObject.Find("Bot");
        //botScript = GameManager.instance.bot.GetComponent<liarsLuckBot>();
    }

    //_unityButtonsLairChoose["name"].SetActive(true);
    void Awake()
    {
        Instance = this;

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
            GameManager.instance.Dropzone = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("dropZone"))
        {
            isOverDropZone = false;
            //GameManager.instance.Dropzone = _lastDropZone;
            //GameManager.instance.Dropzone = null;
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
                Debug.Log(currentIntnum);
                liarsLuckBot.Instance.OnPlayerDroppedCard(currentIntnum);
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
            transform.SetParent(GameManager.instance.Dropzone.transform, false);

            // Get the last child of the dropZone
            int childCount = GameManager.instance.Dropzone.transform.childCount;
            if (childCount > 0)
            {
                Debug.Log("--- Player dropped a card");

                Transform lastCardForBot = GameManager.instance.Dropzone.transform.GetChild(childCount - 1);

                NotifyBotAboutLastDroppedCard(lastCardForBot);

                Transform lastChild = GameManager.instance.Dropzone.transform.GetChild(childCount - 2);
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
                  //  Debug.Log($"Switch: { currentNumber}  ");
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
                //else
                //{
                //    // Bot has no cards - game should end
                //    CheckWinCondition();
                //}

                //GameManager.instance.checkchosencard();
                // Debug.Log(GameManager.instance.AnnounceLie);
                GameManager.instance.LairButton.interactable = false;
                GameManager.instance.TransferCardAndHide();
            }
            else
            {
                transform.position = startPosition;
            }

        }

       // liarsLuckBot.Instance.();
    }
    #endregion
}
