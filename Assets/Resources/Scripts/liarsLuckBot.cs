using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class liarsLuckBot : MonoBehaviour
{
    //public int[] cardCounts = new int[14]; // Tracks how many cards of each rank have been played (index 0 is unused)
    //private List<int> hand; // Bot's current hand
    //public int unseenCardsCount; // Total number of unseen cards
    //private System.Random random = new System.Random();

    //public void InitializeBot(List<int> initialHand, int totalUnseenCards)
    //{
    //    // hand = new List<int>(initialHand);
    //    unseenCardsCount = totalUnseenCards;

    //    // Initialize cardCounts with 0 - the first cell will remain 0, for simplifying the logic
    //    for (int i = 0; i < cardCounts.Length; i++)
    //    {
    //        cardCounts[i] = 0;
    //    }

    //    // Add the bot's hand to cardCounts
    //    foreach (int card in initialHand)
    //    {
    //        cardCounts[card]++;
    //    }
    //}

    //public void UpdateCardCount(int cardRank) {
    //    cardCounts[cardRank]++; // Increment count for known cards
    //    unseenCardsCount--;       // Decrease unseen card count
    //}

    //public double CalculateBluffProbability(int declaredCard)
    //{
    //    int totalCardsPerRank = 4; // Total cards of the rank in the entire deck
    //    int knownCards = cardCounts[declaredCard]; // Known played cards + cards in the bot's hand
    //    int remainingCards = totalCardsPerRank - knownCards; // Remaining possible cards

    //    // Avoid division by zero
    //    if (unseenCardsCount == 0) return 0;

    //    return (double)remainingCards / unseenCardsCount;
    //}

    //public bool ShouldCallBluff(int declaredCard)
    //{
    //    double bluffProbability = CalculateBluffProbability(declaredCard);

    //    // Call a bluff if the probability is high enough
    //    if (bluffProbability > 0.7) // Example threshold: 70% probability
    //    {
    //        return true; // call bluff
    //    }
    //    return false; // do not call bluff
    //}

    //public void ReceiveDrawnCard(int drawnCard)
    //{
    //    hand.Add(drawnCard);         // Add the drawn card to the bot's hand
    //    UpdateCardCount(drawnCard);  // Update the known cards
    //}

    //public int playCard(int currentCard)
    //{
    //    // Find valid cards to play (currentCard +- 1)
    //    List<int> validCards = hand.Where(card => card == currentCard - 1 || card == currentCard + 1).ToList();

    //    if (validCards.Count > 0)
    //    {
    //        // Play a valid card
    //        int chosenCard = validCards[random.Next(validCards.Count)];
    //        hand.Remove(chosenCard);
    //        return chosenCard; // Return the played card
    //    }

    //    // Decide whether to bluff
    //    else if (random.NextDouble() < 0.3)
    //    {
    //        int bluffRank = random.Next(1, 14);
    //        return bluffRank; // Return a random bluff rank
    //    }

    //    // If the bot can't play and decides not to bluff, it must draw a card
    //    else if (unseenCardsCount > 0)
    //    {
    //        return 0; // Signal the Game Manager to draw a card
    //    }

    //    else
    //    {
    //        // No cards to draw, bot must bluff
    //        return random.Next(1, 14); // Bluff with a random rank
    //    }
    //}

    public void OnLiarCardSelected(int buttonNum)
    {
        Debug.Log($"Bot received liar button: {buttonNum}");
    }

    public void OnPlayerDroppedCard(int cardNum)
    {
        Debug.Log($"Bot received player dropped card: {cardNum}");
    }
}