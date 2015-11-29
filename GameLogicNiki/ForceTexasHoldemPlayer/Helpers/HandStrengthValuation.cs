namespace ForceTexasHoldemPlayer.Helpers
{
    using System.Collections.Generic;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Helpers;

    internal class HandStrengthValuation
    {
        private const int MaxCardTypeValue = 14;


        private static readonly int[,] StartingHandRecommendations =
            {
                { 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1 }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 3, 3, 3, 3, 3, 2, 1, 1, 1, 1, 1, 1, 1 }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 3, 3, 3, 3, 3, 2, 2, 0, 0, 0, 0, 0, 0 }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 3, 3, 2, 3, 3, 3, 2, 1, 0, 0, 0, 0, 0 }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 3, 2, 2, 2, 3, 3, 2, 1, 0, 0, 0, 0, 0 }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 1, 1, 1, 1, 1, 3, 2, 1, 1, 0, 0, 0, 0 }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 1, 0, 0, 1, 1, 1, 3, 1, 1, 0, 0, 0, 0 }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 1, 0, 0, 0, 0, 1, 1, 3, 1, 1, 0, 0, 0 }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0 }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0 }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 } // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };


        // http://www.rakebackpros.net/texas-holdem-starting-hands/
        public static CardValuationType PreFlop(Card firstCard, Card secondCard)
        {
            var value = firstCard.Suit == secondCard.Suit
                          ? (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
                          : (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

            /// "Flush" or "Straight" probability
            if (firstCard.Suit == secondCard.Suit ||
                firstCard.Type + 1 == secondCard.Type ||
                secondCard.Type + 1 == firstCard.Type)
            {
                value += 1;
            }

            // VaBank play
            if (value == 0)
            {
                var chanceToBetOnBadHand = RandomProvider.Next(1, 10);

                if (chanceToBetOnBadHand < 9)
                {
                    value += 1;
                }
            }

            switch (value)
            {
                case 1:
                    return CardValuationType.NotRecommended;
                case 2:
                    return CardValuationType.Risky;
                case 3:
                    return CardValuationType.Recommended;
                case 4:
                    return CardValuationType.HighRecommended;
                default:
                    return CardValuationType.Unplayable;
            }
        }

        internal static CardValuationType InGame(Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards)
        {
            var currentBestHandEvaluator = new HandEvaluator();

            var cardsInEvaluatedHand = new Card[5];
            
            var currentBestHand = GetHandRank(firstCard, secondCard, communityCards, currentBestHandEvaluator, cardsInEvaluatedHand);

            if (currentBestHand.RankType == HandRankType.HighCard)
            {
                return CardValuationType.NotRecommended;
            }
            else if (currentBestHand.RankType == HandRankType.Pair)
            {
                return CardValuationType.Risky;
            }
            else if (currentBestHand.RankType == HandRankType.TwoPairs ||
                     currentBestHand.RankType == HandRankType.ThreeOfAKind)
            {
                return CardValuationType.Recommended;
            }
            else if (currentBestHand.RankType == HandRankType.Straight ||
                     currentBestHand.RankType == HandRankType.Flush ||
                     currentBestHand.RankType == HandRankType.FourOfAKind ||
                     currentBestHand.RankType == HandRankType.FullHouse ||
                     currentBestHand.RankType == HandRankType.StraightFlush)
            {
                return CardValuationType.HighRecommended;
            }

            return CardValuationType.Unplayable;
        }

        private static BestHand GetHandRank(Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards, HandEvaluator currentBestHandEvaluator, Card[] cardsInEvaluatedHand)
        {
            cardsInEvaluatedHand[0] = firstCard;
            cardsInEvaluatedHand[1] = secondCard;
            
            var listOfCommunityCards = new List<Card>();

            byte fiveCardsIndex = 2;
            foreach (var card in communityCards)
            {
                listOfCommunityCards.Add(card);
                if (fiveCardsIndex < 5)
                {
                    cardsInEvaluatedHand[fiveCardsIndex] = card;
                }
                fiveCardsIndex++;
            }

            // initial value of best hand
            var bestHandEver = currentBestHandEvaluator.GetBestHand(cardsInEvaluatedHand);

            /// "Flop" round
            if (listOfCommunityCards.Count == 3)
            {
                fiveCardsIndex = 2;
                foreach (var card in listOfCommunityCards)
                {
                    cardsInEvaluatedHand[fiveCardsIndex] = card;
                    fiveCardsIndex++;
                }

                return currentBestHandEvaluator.GetBestHand(cardsInEvaluatedHand);
            }
            /// "Turn" or "River" round
            else if (listOfCommunityCards.Count > 3)
            {
                BestHand currentBestHand;
                //BestHand bestHandEver;
                int maxRank = 0;

                fiveCardsIndex = 2;
                /// check if players hand have combination with 3 cards out of 4 on the table
                for (int j = 0; j < listOfCommunityCards.Count; j++)
                {
                    fiveCardsIndex = 2;
                    for (int i = 0; i < listOfCommunityCards.Count; i++)
                    {
                        if ((j != i) && (fiveCardsIndex < 5))
                        {
                            cardsInEvaluatedHand[fiveCardsIndex] = listOfCommunityCards[i];
                        }
                        fiveCardsIndex++;
                    }

                    currentBestHand = currentBestHandEvaluator.GetBestHand(cardsInEvaluatedHand);

                    CheckIfCurrentBestHandIsBestHand(currentBestHand, ref bestHandEver, ref maxRank);
                }

                /// check only with the first card of the players hand
                cardsInEvaluatedHand[0] = firstCard;
                fiveCardsIndex = 1;
                foreach (var card in listOfCommunityCards)
                {
                    if (fiveCardsIndex < 5)
                    {
                        cardsInEvaluatedHand[fiveCardsIndex] = card;
                    }
                    fiveCardsIndex++;
                }

                currentBestHand = currentBestHandEvaluator.GetBestHand(cardsInEvaluatedHand);

                // check if better combination was found
                CheckIfCurrentBestHandIsBestHand(currentBestHand, ref bestHandEver, ref maxRank);

                /// check only with the second card of the players hand 
                cardsInEvaluatedHand[0] = secondCard;
                fiveCardsIndex = 1;
                foreach (var card in listOfCommunityCards)
                {
                    if (fiveCardsIndex < 5)
                    {
                        cardsInEvaluatedHand[fiveCardsIndex] = card;
                    }
                    fiveCardsIndex++;
                }

                currentBestHand = currentBestHandEvaluator.GetBestHand(cardsInEvaluatedHand);

                // check if better combination was found
                CheckIfCurrentBestHandIsBestHand(currentBestHand, ref bestHandEver, ref maxRank);

                return bestHandEver;
            }
            else
            {
                return currentBestHandEvaluator.GetBestHand(cardsInEvaluatedHand);
            }
        }

        private static void CheckIfCurrentBestHandIsBestHand(BestHand currentBestHand, ref BestHand bestHandEver, ref int maxRank)
        {
            if ((int)currentBestHand.RankType > maxRank)
            {
                bestHandEver = currentBestHand;
                maxRank = (int)currentBestHand.RankType;
            }
        }
    }
}
