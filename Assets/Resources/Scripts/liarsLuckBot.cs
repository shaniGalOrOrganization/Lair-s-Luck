using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class liarsLuckBot : MonoBehaviour
{
    #region Variables
    private static liarsLuckBot instance;
    public int cardNumber;
    public int[] cardCounts = new int[14];
    public int CheckCheatFlag = 0;
    public Dictionary<string, GameObject> _unityButtonLair = new Dictionary<string, GameObject>();

    // Game state tracking
    private int consecutiveLies = 0;
    private int drawsThisGame = 0;
    private int cardsInDeck;
    private int lastPlayedCard = -1;
    private GameManager gameManager;

    // Advanced Strategy
    private static Dictionary<int, float> cardSuccessRates = new Dictionary<int, float>();
    private static Dictionary<int, int> cardUsageCount = new Dictionary<int, int>();
    private static Dictionary<int, float> playerBluffPatterns = new Dictionary<int, float>();
    private static int totalGamesPlayed = 0;
    private const int MAX_DRAWS_PER_GAME = 1;
    private const float AGGRESSIVE_THRESHOLD = 0.7f;

    private class MoveHistory
    {
        public int CardNumber { get; set; }
        public bool WasLie { get; set; }
        public bool WasCaught { get; set; }
        public int RemainingCards { get; set; }
    }

    private List<MoveHistory> gameHistory = new List<MoveHistory>();
    #endregion

    #region Initialization
    void Start()
    {
        Invoke("InitializeBot", 0.2f);
    }

    void Awake()
    {
        try
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.botTextMessage.gameObject.SetActive(false);
            GameObject[] curGameObject = GameObject.FindGameObjectsWithTag("unityButtonLair");
            foreach (GameObject obj in curGameObject)
            {
                _unityButtonLair.Add(obj.name, obj);
            }

            if (cardSuccessRates.Count == 0)
            {
                InitializeLearningSystem();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in Awake: {e.Message}");
        }
    }

    private void InitializeLearningSystem()
    {
        for (int i = 1; i <= 13; i++)
        {
            cardSuccessRates[i] = 0.5f;
            cardUsageCount[i] = 0;
            playerBluffPatterns[i] = 0.5f;
        }
    }

    public static liarsLuckBot Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("SC_liarsLuckBot").GetComponent<liarsLuckBot>();
            }
            return instance;
        }
    }

    public void InitializeBot()
    {
        try
        {
            ResetGameState();
            CountInitialCards();
            cardsInDeck = 52 - (GameManager.instance.PlayerArea.transform.childCount * 2) - 1;
            drawsThisGame = 0;
            gameHistory.Clear();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in InitializeBot: {e.Message}");
        }
    }

    private void ResetGameState()
    {
        for (int i = 1; i < cardCounts.Length; i++)
        {
            cardCounts[i] = 0;
        }
        consecutiveLies = 0;
        lastPlayedCard = -1;
    }

    private void CountInitialCards()
    {
        foreach (Transform cardTransform in GameManager.instance.RealEnemyCardArea.transform)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                int cardNum = GetCardNumber(card.cardNumberString);
                if (cardNum >= 1 && cardNum <= 13)
                {
                    cardCounts[cardNum]++;
                }
            }
        }
    }
    #endregion

    #region Core Game Logic
    public void OnLiarCardSelected(int buttonNum)
    {
        try
        {
            if (buttonNum >= 1 && buttonNum <= 13)
            {
                cardCounts[buttonNum]++;
                UpdatePlayerBluffPattern(buttonNum);
            }

            GameManager.instance.isPlayerTurn = false;
            GameManager.instance.CheckWinCondition();
            if (!GameManager.instance.EndFlag)
            {
                BotMoves(buttonNum);
            }
            else
            {
                UpdateEndGameStats();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in OnLiarCardSelected: {e.Message}");
        }
    }

    private void UpdatePlayerBluffPattern(int cardNum)
    {
        if (cardCounts[cardNum] >= 4)
        {
            float oldRate = playerBluffPatterns[cardNum];
            playerBluffPatterns[cardNum] = oldRate * 0.9f + 0.1f;
        }
    }

    public void BotMoves(int buttonNumPlayerChoose)
    {
        try
        {
            lastPlayedCard = -1;

            // First priority - call bluff if highly likely
            if (ShouldCallBluff(buttonNumPlayerChoose))
            {
                HandleBluffCall(buttonNumPlayerChoose);
                return;
            }

            CheckCheatFlag = 0;
            bool hasValidCard = FindValidCard(buttonNumPlayerChoose, out Transform validCardTransform, out int validCardNumber);
            int botHandSize = GameManager.instance.RealEnemyCardArea.transform.childCount;

            // Decision making priority
            if (hasValidCard && !ShouldSaveValidCard(validCardNumber))
            {
                PlayCard(validCardTransform, validCardNumber);
            }
            else if (ShouldDrawCard(botHandSize))
            {
                DrawCards.instance.OnClick();
                drawsThisGame++;
            }
            else
            {
                PlayStrategicBluff(buttonNumPlayerChoose);
            }

            CompleteMove();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in BotMoves: {e.Message}");
        }
    }

    private bool ShouldSaveValidCard(int cardNumber)
    {
        // Save card if:
        // 1. We're close to winning (2-3 cards left)
        // 2. This card has high success rate
        // 3. We have alternative moves
        int botHandSize = GameManager.instance.RealEnemyCardArea.transform.childCount;
        return botHandSize <= 3 &&
               botHandSize > 1 &&
               cardSuccessRates[cardNumber] > AGGRESSIVE_THRESHOLD &&
               !IsLastValidCard(cardNumber);
    }

    private bool IsLastValidCard(int cardNumber)
    {
        int validCards = 0;
        foreach (Transform cardTransform in GameManager.instance.RealEnemyCardArea.transform)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                int num = GetCardNumber(card.cardNumberString);
                if (IsValidPlay(num, lastPlayedCard))
                {
                    validCards++;
                    if (validCards > 1)
                        return false;
                }
            }
        }
        return validCards == 1;
    }

    private bool ShouldDrawCard(int botHandSize)
    {
        // Add new check for existing valid cards
        if (HasAnyValidCard())
            return false;  // Never draw if we have valid cards to play

        if (botHandSize > 1)
            return false;
        if (drawsThisGame >= MAX_DRAWS_PER_GAME)
            return false;
        if (cardsInDeck <= 0)
            return false;
        if (IsOptimalWinningPosition())
            return false;

        return true;
    }

    private bool HasAnyValidCard()
    {
        return FindValidCard(lastPlayedCard, out _, out _);
    }

    private bool IsOptimalWinningPosition()
    {
        int playerHandSize = GameManager.instance.PlayerArea.transform.childCount;
        int botHandSize = GameManager.instance.RealEnemyCardArea.transform.childCount;
        return botHandSize < playerHandSize || botHandSize <= 2;
    }
    #endregion

    #region Bluffing Logic
    private bool ShouldCallBluff(int declaredCard)
    {
        // Basic bluff detection
        if (cardCounts[declaredCard] >= 4)
            return true;

        // Advanced bluff detection
        int playerHandSize = GameManager.instance.PlayerArea.transform.childCount;
        float bluffProbability = CalculateBluffProbability(declaredCard);
        float playerPattern = playerBluffPatterns[declaredCard];

        // Call bluff if:
        // 1. High probability of bluff and player has few cards
        // 2. Player has history of bluffing with this card
        // 3. Critical game state (near end)
        return (bluffProbability > 0.7f && playerHandSize <= 3) ||
               //(playerPattern > 0.8f && cardCounts[declaredCard] > 1) ||
               (playerHandSize <= 2 && cardCounts[declaredCard] > 1);
    }

    private float CalculateBluffProbability(int cardNumber)
    {
        float visibleCards = cardCounts[cardNumber];
        float playerHandSize = GameManager.instance.PlayerArea.transform.childCount;
        float deckFactor = cardsInDeck / 52f;
        float historicalPattern = playerBluffPatterns[cardNumber];

        // Complex probability calculation
        float baseProbability = (visibleCards / 4f) * (1 - deckFactor);
        float playerFactor = 1 + (playerHandSize < 3 ? 0.3f : 0);
        float historyFactor = 1 + historicalPattern;

        return baseProbability * playerFactor * historyFactor;
    }

    private void PlayStrategicBluff(int currentNumber)
    {
        try
        {
            if (GameManager.instance.RealEnemyCardArea.transform.childCount > 0)
            {
                Transform cardToPlay;

                // Choose card based on game state
                if (IsOptimalWinningPosition())
                {
                    cardToPlay = ChooseLeastValuableCard();
                }
                else
                {
                    cardToPlay = ChooseBestBluffCard();
                }

                cardToPlay.SetParent(GameManager.instance.DropZoneStack.transform, false);
                lastPlayedCard = currentNumber;
                consecutiveLies++;

                EnableRelevantButtons(currentNumber);
                AnnounceCardPlayed(currentNumber, true);
                GameManager.instance.SyncEnemyArea();

                // Update statistics
                RecordMove(currentNumber, true);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in PlayStrategicBluff: {e.Message}");
        }
    }

    private Transform ChooseLeastValuableCard()
    {
        float lowestValue = float.MaxValue;
        Transform chosenCard = null;

        foreach (Transform cardTransform in GameManager.instance.RealEnemyCardArea.transform)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                int cardNum = GetCardNumber(card.cardNumberString);
                float cardValue = EvaluateCardValue(cardNum);
                if (cardValue < lowestValue)
                {
                    lowestValue = cardValue;
                    chosenCard = cardTransform;
                }
            }
        }

        return chosenCard ?? GameManager.instance.RealEnemyCardArea.transform.GetChild(0);
    }

    private Transform ChooseBestBluffCard()
    {
        float highestPotential = float.MinValue;
        Transform chosenCard = null;

        foreach (Transform cardTransform in GameManager.instance.RealEnemyCardArea.transform)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                int cardNum = GetCardNumber(card.cardNumberString);
                float potential = EvaluateBluffPotential(cardNum);
                if (potential > highestPotential)
                {
                    highestPotential = potential;
                    chosenCard = cardTransform;
                }
            }
        }

        return chosenCard ?? GameManager.instance.RealEnemyCardArea.transform.GetChild(0);
    }

    private float EvaluateCardValue(int cardNumber)
    {
        float successRate = cardSuccessRates[cardNumber];
        float usageCount = cardUsageCount[cardNumber];
        float visibilityFactor = 1f - (cardCounts[cardNumber] / 4f);

        return (successRate * 0.5f + visibilityFactor * 0.3f + (usageCount / 10f) * 0.2f);
    }

    private float EvaluateBluffPotential(int cardNumber)
    {
        float successRate = cardSuccessRates[cardNumber];
        float playerBluffRate = playerBluffPatterns[cardNumber];
        float visibilityFactor = 1f - (cardCounts[cardNumber] / 4f);

        return (successRate * 0.4f + (1 - playerBluffRate) * 0.3f + visibilityFactor * 0.3f);
    }

    #endregion

    #region Card Playing and Move Management
    private void PlayCard(Transform cardTransform, int cardNumber)
    {
        try
        {
            cardTransform.SetParent(GameManager.instance.DropZoneStack.transform, false);
            lastPlayedCard = cardNumber;
            consecutiveLies = 0;

            EnableRelevantButtons(cardNumber);
            AnnounceCardPlayed(cardNumber, false);
            GameManager.instance.SyncEnemyArea();

            // Update statistics and learning
            cardUsageCount[cardNumber]++;
            UpdateSuccessRate(cardNumber, true);
            RecordMove(cardNumber, false);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in PlayCard: {e.Message}");
        }
    }

    private void HandleBluffCall(int buttonNum)
    {
        try
        {
            cardCounts[buttonNum] = 0;
            GameManager.instance.BTN_Lair();
            CheckCheatFlag = 1;

            // Update learning when calling bluff
            MoveHistory lastMove = gameHistory.LastOrDefault();
            if (lastMove != null)
            {
                lastMove.WasCaught = true;
                UpdateSuccessRate(lastMove.CardNumber, false);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in HandleBluffCall: {e.Message}");
        }
    }

    private void UpdateSuccessRate(int cardNumber, bool success)
    {
        float currentRate = cardSuccessRates[cardNumber];
        float learningFactor = 1.0f / (cardUsageCount[cardNumber] + 1);
        cardSuccessRates[cardNumber] = currentRate * (1 - learningFactor) + (success ? learningFactor : 0);
    }

    private void RecordMove(int cardNumber, bool wasLie)
    {
        gameHistory.Add(new MoveHistory
        {
            CardNumber = cardNumber,
            WasLie = wasLie,
            WasCaught = false,
            RemainingCards = GameManager.instance.RealEnemyCardArea.transform.childCount
        });
    }

    private void CompleteMove()
    {
        GameManager.instance.CheckWinCondition();
        if (!GameManager.instance.EndFlag)
        {
            GameManager.instance.isPlayerTurn = true;
            _unityButtonLair["Button_Cheat"].GetComponent<Button>().interactable = true;
        }
        else
        {
            UpdateEndGameStats();
        }
    }

    private void UpdateEndGameStats()
    {
        totalGamesPlayed++;
        bool won = GameManager.instance.RealEnemyCardArea.transform.childCount == 0;

        foreach (var move in gameHistory)
        {
            float successFactor = won ? 1.2f : 0.8f;
            if (!move.WasCaught)
            {
                UpdateSuccessRate(move.CardNumber, true);
                cardSuccessRates[move.CardNumber] *= successFactor;
            }
        }
    }

    #endregion

    #region Helper and UI Methods
    private bool FindValidCard(int currentNumber, out Transform cardTransform, out int cardNumber)
    {
        cardTransform = null;
        cardNumber = -1;
        int bestValue = -1;
        float bestScore = float.MinValue;

        foreach (Transform transform in GameManager.instance.RealEnemyCardArea.transform)
        {
            Card card = transform.GetComponent<Card>();
            if (card != null)
            {
                int num = GetCardNumber(card.cardNumberString);
                if (IsValidPlay(num, currentNumber))
                {
                    float score = EvaluateCardForPlay(num);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestValue = num;
                        cardTransform = transform;
                    }
                }
            }
        }

        if (bestValue != -1)
        {
            cardNumber = bestValue;
            return true;
        }
        return false;
    }

    private float EvaluateCardForPlay(int cardNumber)
    {
        int botHandSize = GameManager.instance.RealEnemyCardArea.transform.childCount;
        float winningBonus = IsOptimalWinningPosition() ? 0.3f : 0;
        float endGameBonus = (botHandSize <= 2) ? 0.2f : 0;

        return cardSuccessRates[cardNumber] + winningBonus + endGameBonus;
    }

    private bool IsValidPlay(int cardNumber, int currentNumber)
    {
        if (cardNumber < 1 || cardNumber > 13)
            return false;

        int next = (currentNumber + 1) > 13 ? 1 : currentNumber + 1;
        int prev = (currentNumber - 1) < 1 ? 13 : currentNumber - 1;

        return cardNumber == currentNumber || cardNumber == next || cardNumber == prev;
    }

    public int GetCardNumber(string cardNumberString)
    {
        switch (cardNumberString.ToLower())
        {
            case "ace":
                return 1;
            case "two":
                return 2;
            case "three":
                return 3;
            case "four":
                return 4;
            case "five":
                return 5;
            case "six":
                return 6;
            case "seven":
                return 7;
            case "eight":
                return 8;
            case "nine":
                return 9;
            case "ten":
                return 10;
            case "jack":
                return 11;
            case "queen":
                return 12;
            case "king":
                return 13;
            default:
                Debug.LogWarning($"Invalid card number string: {cardNumberString}");
                return -1;
        }
    }

    private void AnnounceCardPlayed(int cardNumber, bool isBluff)
    {
        string cardName = GetCardDisplayName(cardNumber);
        string message = $"Bot declared: {cardName}";
        StartCoroutine(GameManager.instance.showBotMessage(message, 3f));
    }

    private string GetCardDisplayName(int cardNumber)
    {
        switch (cardNumber)
        {
            case 1:
                return "Ace";
            case 11:
                return "Jack";
            case 12:
                return "Queen";
            case 13:
                return "King";
            default:
                return cardNumber.ToString();
        }
    }

    public void OnPlayerDroppedCard(int cardNum)
    {
        try
        {
            if (cardNum >= 1 && cardNum <= 13)
            {
                //cardCounts[cardNum]++;
                UpdatePlayerCardStats(cardNum);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in OnPlayerDroppedCard: {e.Message}");
        }
    }

    private void UpdatePlayerCardStats(int cardNum)
    {
        int playerHandSize = GameManager.instance.PlayerArea.transform.childCount;
        if (playerHandSize <= 3)
        {
            playerBluffPatterns[cardNum] = playerBluffPatterns[cardNum] * 0.7f + 0.3f;
        }
    }

    private void EnableRelevantButtons(int cardNumber)
    {
        try
        {
            foreach (var pair in DragDrop.Instance._unityButtonsLairChoose)
            {
                pair.Value.GetComponent<Button>().interactable = false;
            }

            GameManager.instance.chosenNumber = cardNumber;

            switch (cardNumber)
            {
                case 1:
                    DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true;
                    break;
                case 2:
                    DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true;
                    break;
                case 3:
                    DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_two"].GetComponent<Button>().interactable = true;
                    break;
                case 4:
                    DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_three"].GetComponent<Button>().interactable = true;
                    break;
                case 5:
                    DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_four"].GetComponent<Button>().interactable = true;
                    break;
                case 6:
                    DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_five"].GetComponent<Button>().interactable = true;
                    break;
                case 7:
                    DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_six"].GetComponent<Button>().interactable = true;
                    break;
                case 8:
                    DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_seven"].GetComponent<Button>().interactable = true;
                    break;
                case 9:
                    DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_eight"].GetComponent<Button>().interactable = true;
                    break;
                case 10:
                    DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_nine"].GetComponent<Button>().interactable = true;
                    break;
                case 11:
                    DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_ten"].GetComponent<Button>().interactable = true;
                    break;
                case 12:
                    DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_jack"].GetComponent<Button>().interactable = true;
                    break;
                case 13:
                    DragDrop.Instance._unityButtonsLairChoose["Button_king"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_ace"].GetComponent<Button>().interactable = true;
                    DragDrop.Instance._unityButtonsLairChoose["Button_queen"].GetComponent<Button>().interactable = true;
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in EnableRelevantButtons: {e.Message}");
        }
    }
    #endregion
}