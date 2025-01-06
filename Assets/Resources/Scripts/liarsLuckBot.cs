using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class liarsLuckBot : MonoBehaviour
{
    private int[] cardCounts = new int[14]; // Tracks how many cards of each rank have been played (index 0 is unused)
    private List<int> hand; // Bot's current hand
    private List<int> unseenDeck; // Cards remaining in the deck for drawing
    private System.Random random = new System.Random();

    public void InitializeBot(List<int> initialHand, List<int> initialUnseenDeck)
    {
        hand = new List<int>(initialHand);
        unseenDeck = new List<int>(initialUnseenDeck);

        // Initialize cardCounts with 0
        for (int i = 0; i < cardCounts.Length; i++)
        {
            cardCounts[i] = 0;
        }

        // Add the bot's hand to cardCounts
        foreach (int card in hand)
        {
            cardCounts[card]++;
        }
    }

    public void UpdateCardCount(int cardRank) {
        cardCounts[cardRank]++; // Increment count for known cards
        unseenDeck.Remove(cardRank); // Remove from unseen deck
    }

    public double CalculateBluffProbability(int declaredCard)
    {
        int totalCardsPerRank = 4; // Total cards of the rank in the entire deck
        int knownCards = cardCounts[declaredCard]; // Known played cards + cards in the bot's hand
        int remainingCards = totalCardsPerRank - knownCards; // Remaining cards of the declared rank
        int unseenCards = unseenDeck.Count; // Total unseen cards

        return (double)(remainingCards / unseenCards);
    }

    public bool ShouldCallBluff(int declaredCard)
    {
        double bluffProbability = CalculateBluffProbability(declaredCard);

        // Call a bluff if the probability is high enough
        if (bluffProbability > 0.7) // Example threshold: 70% probability
        {
            return true; // call bluff
        }
        return false; // do not call bluff
    }

    public int playCard(int currentCard)
    {
        // Find valid cards to play (currentCard +- 1)
        List<int> validCards = hand.Where(card => card == currentCard - 1 || card == currentCard + 1).ToList();

        if (validCards.Count > 0)
        {
            // Play a valid card
            int chosenCard = validCards[random.Next(validCards.Count)];
            hand.Remove(chosenCard);
            return chosenCard;
        }

        // Decide whether to bluff
        else if (random.NextDouble() < 0.3)
        {
            int bluffRank = random.Next(1, 14);
            return bluffRank; // Return a random bluff rank
        }

        else if (unseenDeck.Count > 0)
        {
            // Draw a card from the unseen deck
            int drawnCard = unseenDeck[random.Next(unseenDeck.Count)];
            hand.Add(drawnCard);
            UpdateCardCount(drawnCard);
            return 0; // the bot draw card
        }

        else
        {
            // No cards to draw, bot must bluff
            return random.Next(1, 14); // Bluff with a random rank
        }
    }
}
