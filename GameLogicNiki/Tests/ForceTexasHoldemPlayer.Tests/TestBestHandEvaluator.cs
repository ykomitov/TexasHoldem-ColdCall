namespace TestBestHandEvaluator
{
    using ForceTexasHoldemPlayer.AI;
    using System.Collections.Generic;
    using TexasHoldem.Logic.Cards;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestBestHandEvaluator
    {
        Card firstCard;
        Card secondCard;
        List<Card> arrayCards = new List<Card>();
        BestHandEvaluator bestHand = new BestHandEvaluator();

        [TestInitialize]
        public void InitializePlayingCards()
        {
            this.firstCard = new Card(CardSuit.Club, CardType.Ace);
            this.secondCard = new Card(CardSuit.Spade, CardType.Ace);
            Card thirdCard = new Card(CardSuit.Heart, CardType.Ace);
            Card forthCard = new Card(CardSuit.Diamond, CardType.Ace);
            Card FifthCard = new Card(CardSuit.Diamond, CardType.Eight);

            arrayCards.Add(thirdCard);
            arrayCards.Add(forthCard);
            arrayCards.Add(FifthCard);
        }

        [TestMethod]
        public void BestHandEvaluatorShouldReturnFourOfAKind()
        {
            byte BestHandValue = bestHand.Evaluate(firstCard, secondCard, arrayCards);

            Assert.AreEqual(7, BestHandValue);
        }

        [TestMethod]
        public void BestHandEvaluatorShouldReturnTwoByTwo()
        {
            this.firstCard = new Card(CardSuit.Club, CardType.Two);
            this.secondCard = new Card(CardSuit.Spade, CardType.Two);

            byte BestHandValue = bestHand.Evaluate(firstCard, secondCard, arrayCards);

            Assert.AreEqual(2, BestHandValue);
        }

        [TestMethod]
        public void BestHandEvaluatorShouldReturnThreeOfAKind()
        {
            this.firstCard = new Card(CardSuit.Club, CardType.Two);
            this.secondCard = new Card(CardSuit.Spade, CardType.Ace);

            byte BestHandValue = bestHand.Evaluate(firstCard, secondCard, arrayCards);

            Assert.AreEqual(3, BestHandValue);
        }

        [TestMethod]
        public void BestHandEvaluatorShouldReturnStraight()
        {
            this.firstCard = new Card(CardSuit.Club, CardType.Two);
            this.secondCard = new Card(CardSuit.Spade, CardType.Three);

            Card thirdCard = new Card(CardSuit.Heart, CardType.Four);
            Card forthCard = new Card(CardSuit.Diamond, CardType.Five);
            Card fifthCard = new Card(CardSuit.Diamond, CardType.Six);

            arrayCards.Clear();

            arrayCards.Add(thirdCard);
            arrayCards.Add(forthCard);
            arrayCards.Add(fifthCard);


            byte BestHandValue = bestHand.Evaluate(firstCard, secondCard, arrayCards);

            Assert.AreEqual(4, BestHandValue);
        }

        [TestMethod]
        public void BestHandEvaluatorShouldReturnStraightFlush()
        {
            this.firstCard = new Card(CardSuit.Club, CardType.Two);
            this.secondCard = new Card(CardSuit.Club, CardType.Three);

            Card thirdCard = new Card(CardSuit.Club, CardType.Four);
            Card forthCard = new Card(CardSuit.Club, CardType.Five);
            Card fifthCard = new Card(CardSuit.Club, CardType.Six);

            arrayCards.Clear();

            arrayCards.Add(thirdCard);
            arrayCards.Add(forthCard);
            arrayCards.Add(fifthCard);

            byte BestHandValue = bestHand.Evaluate(firstCard, secondCard, arrayCards);

            Assert.AreEqual(8, BestHandValue);
        }

        [TestMethod]
        public void BestHandEvaluatorShouldReturnFlush()
        {
            this.firstCard = new Card(CardSuit.Club, CardType.Two);
            this.secondCard = new Card(CardSuit.Club, CardType.Ace);

            Card thirdCard = new Card(CardSuit.Club, CardType.Four);
            Card forthCard = new Card(CardSuit.Club, CardType.Five);
            Card fifthCard = new Card(CardSuit.Club, CardType.Six);

            arrayCards.Clear();

            arrayCards.Add(thirdCard);
            arrayCards.Add(forthCard);
            arrayCards.Add(fifthCard);


            byte BestHandValue = bestHand.Evaluate(firstCard, secondCard, arrayCards);

            Assert.AreEqual(5, BestHandValue);
        }
    }
}
