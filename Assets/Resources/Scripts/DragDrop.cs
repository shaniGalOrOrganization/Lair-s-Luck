using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    #region Variables

    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private Vector2 startPosition;
    private static CardStack cardStack = null;
    private Card cardData;
    private Dictionary<string, GameObject> _unityButtonsLairChoose = new Dictionary<string, GameObject>();
    #endregion

    // Define the CardStack class
    public class CardStack
    {
        private Stack<Card> stack;

        public CardStack()
        {
            stack = new Stack<Card>();
        }

        // Push a card onto the stack
        public void Push(Card card)
        {
            stack.Push(card);
        }

        // Pop a card from the stack
        public Card Pop()
        {
            if (stack.Count == 0)
            {
                return null;
            }
            return stack.Pop();
        }

        // Peek at the top card without removing it
        public Card Peek()
        {
            if (stack.Count == 0)
            {
                return null;
            }
            return stack.Peek();
        }

        // Check if the stack is empty
        public bool IsEmpty()
        {
            return stack.Count == 0;
        }
    }

    #region MonoBehaviour

    //_unityButtonsLairChoose["name"].SetActive(true);
    void Awake()
    {
        GameObject[] curGameObject = GameObject.FindGameObjectsWithTag("ButtonLairChoose");
        foreach (GameObject obj in curGameObject)
        {
            _unityButtonsLairChoose.Add(obj.name, obj);
        }

        foreach (var pair in _unityButtonsLairChoose) // שימוש בזוגות מפתח-ערך
        {
            pair.Value.SetActive(false); // גישה לערך במילון
        }
    }

    void Start()
    {
        if(cardStack == null)
            cardStack = new CardStack();

        //cardData = GetComponent<Card>();
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
    public static int getTopCard()
    {
        Card c = cardStack.Peek();
        return (int)c.cardNumber;
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);

            cardStack.Push(cardData);
            cardData = cardStack.Peek();

            if (cardData != null )
            {  
                cardStack.Push(cardData);

                if (cardStack.IsEmpty())
                {
                    Debug.Log("Stack is empty");
                }
                else
                {
                    Debug.Log("Stack is full");
                }

                int current = (int)cardData.cardNumber;
                int before = current - 1;
                if (before < 1)
                    before = 13;
                int after = current + 1;
                if(after > 13)
                    after = 1;

                if (cardData != null)
                {
                    Debug.Log($"Pushing card to stack: {(int)cardData.cardNumber} of {cardData.cardSuit}");
                    cardStack.Push(cardData);
                }
                else
                {
                    Debug.LogError("cardData is null. Cannot push to stack.");
                }

                //if (cardStack.IsEmpty())
                //{
                //    Debug.LogError("The card stack is empty! Cannot use Peek.");
                //}
                //else
                //{
                //    Card topCard = cardStack.Peek();
                //    Debug.Log($"Top card is: {topCard.cardNumber}");
                //}

                switch (current)
                {
                    case 1:
                        _unityButtonsLairChoose["Button_ace"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_two"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_king"].SetActive(true); //before
                        break;
                    case 2:
                        _unityButtonsLairChoose["Button_two"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_three"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_ace"].SetActive(true); //before
                        break;
                    case 3:
                        _unityButtonsLairChoose["Button_three"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_four"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_two"].SetActive(true); //before
                        break;
                    case 4:
                        _unityButtonsLairChoose["Button_four"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_five"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_three"].SetActive(true); //before
                        break;
                    case 5:
                        _unityButtonsLairChoose["Button_five"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_six"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_four"].SetActive(true); //before
                        break;
                    case 6:
                        _unityButtonsLairChoose["Button_six"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_seven"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_five"].SetActive(true); //before
                        break;
                    case 7:
                        _unityButtonsLairChoose["Button_seven"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_eight"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_six"].SetActive(true); //before
                        break;
                    case 8:
                        _unityButtonsLairChoose["Button_eight"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_nine"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_seven"].SetActive(true); //before
                        break;
                    case 9:
                        _unityButtonsLairChoose["Button_nine"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_ten"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_eight"].SetActive(true); //before
                        break;
                    case 10:
                        _unityButtonsLairChoose["Button_ten"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_jack"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_nine"].SetActive(true); //before
                        break;
                    case 11:
                        _unityButtonsLairChoose["Button_jack"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_queen"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_ten"].SetActive(true); //before
                        break;
                    case 12:
                        _unityButtonsLairChoose["Button_queen"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_king"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_jack"].SetActive(true); //before
                        break;
                    case 13:
                        _unityButtonsLairChoose["Button_king"].SetActive(true); //current
                        _unityButtonsLairChoose["Button_ace"].SetActive(true); //after
                        _unityButtonsLairChoose["Button_queen"].SetActive(true); //before
                        break;
                    default:
                        Debug.Log(current);
                        break;
                }
            }
        }
        else
        {
            transform.position = startPosition;
        }
    }

    #endregion
}
