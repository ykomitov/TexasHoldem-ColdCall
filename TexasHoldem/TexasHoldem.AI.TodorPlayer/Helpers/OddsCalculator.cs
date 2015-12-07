namespace TexasHoldem.AI.ColdCallPlayer.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Helpers;

    public static class OddsCalculator
    {
        private const int SimulationsCount = 25;

        public static double CalculateHandStrength(ICollection<Card> communityCards, ICollection<Card> playerCards)
        {
            double wins = 0;
            IList<Card> deck = GetDeck(communityCards, playerCards);

            for (int i = 0; i < SimulationsCount; i++)
            {
                deck = deck.Shuffle().ToList();
                var simulationResult = SimulateHand(communityCards.ToList(), playerCards, deck);
                wins += simulationResult;
            }

            return wins / SimulationsCount;
        }

        public static int SimulateHand(ICollection<Card> communityCards, ICollection<Card> playerCards, IList<Card> deck)
        {
            var cardIndex = deck.Count - 1;

            var opponentCards = new List<Card>();
            opponentCards.Add(deck[cardIndex--]);
            opponentCards.Add(deck[cardIndex--]);

            while (communityCards.Count < 5)
            {
                communityCards.Add(deck[cardIndex--]);
            }

            // compare hands
            var betterHand = Helpers.CompareCards(
                    playerCards.Concat(communityCards),
                    opponentCards.Concat(communityCards));
            if (betterHand > 0)
            {
                return 1;
            }

            return 0;
        }

        public static IList<Card> GetDeck(ICollection<Card> communityCards, ICollection<Card> playerCards)
        {
            var deck = Deck.AllCards.ToList();

            foreach (var card in communityCards)
            {
                deck.Remove(card);
            }

            foreach (var card in playerCards)
            {
                deck.Remove(card);
            }

            return deck;
        }

        public static double CalculatePotOdds(double pot, double moneyToCall, int moneyToRaise)
        {
            var potOdds = moneyToCall / (pot + moneyToCall);

            if (potOdds == 0)
            {
                //return 0.1; // TODO: Not sure what to return here, do more research
                potOdds = (double)moneyToRaise / (pot + moneyToRaise);
            }

            return potOdds;
        }

        public static double CalculateRateOfReturn(double handStrength, double potOdds)
        {
            return handStrength / potOdds;
        }
    }
}