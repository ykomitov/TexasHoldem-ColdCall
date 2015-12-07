namespace ForceTexasHoldemPlayer.AI
{
    using System.Collections.Generic;
    using System.Linq;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;

    public class ProbabilityEvaluator
    {
        // all hands probabilities for River game
        private float[] allCombinationsProbabillity;
        private List<Card> allCards;

        public ProbabilityEvaluator()
        {
            this.allCards = new List<Card>();
        }

        public float[] Evaluate(
            Card firstCard,
            Card secondCard,
            IReadOnlyCollection<Card> communityCards,
            GameRoundType round
            )
        {
            InitializeAllCardsToEvaluate(firstCard, secondCard, communityCards);

            byte maxSameSuits = 0;
            byte maxSameTypes = 0;
            byte maxStraight = 0;

            GetPresentCombinations(ref maxSameSuits, ref maxSameTypes, ref maxStraight);

            // edited chance for 2, 3 and 4 of a kind
            DrawSameType(maxSameTypes);

            // edited chance for Flush
            allCombinationsProbabillity[5] = DrawSameSuit(maxSameSuits);

            // edited chance for Straight
            allCombinationsProbabillity[4] = DrawStraight(maxStraight);

            return allCombinationsProbabillity;
        }

        private float DrawStraight(byte maxStraight)
        {
            float chance = 0f;

            // flop game
            if (allCards.Count == 5)
            {
                switch (maxStraight)
                {
                    case 3:
                        // to get straight in 2 draws
                        chance = 2f / (52f - 5f) * (52f - 6f);
                        break;
                    case 4:
                        // to get straight in 2 draws
                        chance = 4f / (52f - 5f);
                        break;
                    default:
                        chance = 0f;
                        break;
                }
            }
            // turn game
            else if (allCards.Count == 6)
            {
                switch (maxStraight)
                {
                    case 4:
                        // to get straight in one draw
                        chance = 4f / (52f - 6f);
                        break;
                    default:
                        chance = 0f;
                        break;
                }
            }

            return chance;
        }

        private float DrawSameSuit(byte maxSameSuits)
        {
            float chance = 0f;

            // flop game
            if (allCards.Count == 5)
            {
                switch (maxSameSuits)
                {
                    case 3:
                        // to get flush in 2 draws
                        chance = (9f + 10f) / ((52f - 5f) * (52f - 6f));
                        break;
                    case 4:
                        // to get flush in 2 draws
                        chance = 2 * (9f + 10f) / ((52f - 5f) * (52f - 6f));
                        break;
                    default:
                        chance = 0f;
                        break;
                }
            }
            // turn game
            else if (allCards.Count == 6)
            {
                switch (maxSameSuits)
                {
                    case 4:
                        // to get flush in one draw
                        chance = 9f / (52f - 6f);
                        break;
                    default:
                        chance = 0f;
                        break;
                }
            }

            return chance;
        }

        private void DrawSameType(byte maxSameTypes)
        {
            // flop game
            if (allCards.Count == 5)
            {
                switch (maxSameTypes)
                {
                    case 1:
                        // to get second of a type in 2 draws
                        allCombinationsProbabillity[1] = 2 * ((52f - 6f) + (52f - 5f)) / ((52f - 6f) * (52f - 5f));
                        break;
                    case 2:
                        // to get third of a type in 2 draws
                        allCombinationsProbabillity[3] = ((52f - 6f) + (52f - 5f)) / ((52f - 6f) * (52f - 5f));
                        break;
                    case 3:
                        // to get forth of a type in 2 draws
                        allCombinationsProbabillity[7] = 1f / ((52f - 6f) * (52f - 5f));
                        break;
                    default:
                        allCombinationsProbabillity[1] = 0;
                        allCombinationsProbabillity[3] = 0;
                        allCombinationsProbabillity[7] = 0;
                        break;
                }
            }
            // turn game
            else if (allCards.Count == 6)
            {
                switch (maxSameTypes)
                {
                    case 1:
                        // to get second of a type in one draw
                        allCombinationsProbabillity[1] = 3f / (52f - 6f);
                        break;
                    case 2:
                        // to get third of a type in one draw
                        allCombinationsProbabillity[3] = 2f / (52f - 6f);
                        break;
                    case 3:
                        // to get forth of a type in one draw
                        allCombinationsProbabillity[7] = 1f / (52f - 6f);
                        break;
                    default:
                        allCombinationsProbabillity[1] = 0;
                        allCombinationsProbabillity[3] = 0;
                        allCombinationsProbabillity[7] = 0;
                        break;
                }
            }
        }

        private void GetPresentCombinations(ref byte maxSameSuits, ref byte maxSameTypes, ref byte maxStraight)
        {
            byte sameSuit = 0;
            byte sameType = 0;


            for (int i = 0; i < allCards.Count; i++)
            {
                /// Loop over suits
                for (int j = 0; j < 4; j++)
                {
                    /// Loop over types
                    for (int k = 0; k < 14; k++)
                    {
                        if ((int)allCards[i].Suit == j)
                        {
                            sameSuit++;
                        }

                        if ((int)allCards[i].Type == k)
                        {
                            sameType++;
                        }
                    }
                }

                if (sameSuit > maxSameSuits)
                {
                    maxSameSuits = sameSuit;
                }

                if (sameType > maxSameTypes)
                {
                    maxSameTypes = sameType;
                }
            }

            /// find max sequence
            allCards.OrderBy(c => c.Type);
            for (int i = 0; i < allCards.Count - 1; i++)
            {
                if (allCards[i].Type + 1 == allCards[i + 1].Type)
                {
                    maxStraight++;
                }
            }
        }

        private void InitializeAllCardsToEvaluate(Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards)
        {
            this.allCombinationsProbabillity = new float[]
            {
                // http://wizardofodds.com/games/poker/
                 0.1741f,   //0  higher card 
                 0.4382f,   //1  pair
                 0.2349f,   //2  2 pairs
                 0.0482f,   //3  3 of a kind
                 0.04619f,  //4  Straight
                 0.03025f,  //5  Flush
                 0.02596f,  //6  Full house
                 0.00168f,  //7  Four of a kind
                 0.000278f  //8  Straight flush
            };

            allCards.Clear();

            allCards.Add(firstCard);
            allCards.Add(secondCard);

            foreach (var card in communityCards)
            {
                allCards.Add(card);
            }
        }
    }
}