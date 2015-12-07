namespace ForceTexasHoldemPlayer.AI
{
    using System.Collections.Generic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;

    public class BestHandEvaluator
    {
        private HandEvaluator currentBestHandEvaluator;
        private Card[] cardsInEvaluatedHand;
        private byte bestHandValue;

        public BestHandEvaluator()
        {
            this.currentBestHandEvaluator = new HandEvaluator();
            this.cardsInEvaluatedHand = new Card[5];
        }

        internal byte BestHandValue
        {
            get
            {
                return this.bestHandValue;
            }

            private set
            {
                this.bestHandValue = value;
            }
        }

        public byte Evaluate(Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards)
        {
            var currentBestHand = this.GetHandRank(firstCard, secondCard, communityCards, currentBestHandEvaluator, cardsInEvaluatedHand);

            this.BestHandValue = (byte)((int)currentBestHand.RankType / 1000);

            return BestHandValue;
        }

        private BestHand GetHandRank(Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards, HandEvaluator currentBestHandEvaluator, Card[] cardsInEvaluatedHand)
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

        private void CheckIfCurrentBestHandIsBestHand(BestHand currentBestHand, ref BestHand bestHandEver, ref int maxRank)
        {
            if ((int)currentBestHand.RankType > maxRank)
            {
                bestHandEver = currentBestHand;
                maxRank = (int)currentBestHand.RankType;
            }
        }
    }
}
