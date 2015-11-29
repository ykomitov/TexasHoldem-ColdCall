
namespace TexasHoldem.AI.TodorPlayer.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Helpers;
    using System.Collections.Generic;
    using Logic.Cards;

    [TestClass]
    public class HandStrengthTests
    {
        private static List<Card> straightFlushCommunityCards = new List<Card>
            {
                new Card(CardSuit.Club, CardType.Ace),
                new Card(CardSuit.Club, CardType.King),
                new Card(CardSuit.Club, CardType.Queen),
                new Card(CardSuit.Club, CardType.Jack),
                new Card(CardSuit.Heart, CardType.Two),
            };

        private static List<Card> straightFlushPlayerCards = new List<Card>
            {
                new Card(CardSuit.Club, CardType.Ten),
                new Card(CardSuit.Club, CardType.Nine),
            };

        private static List<Card> weakPlayerHand = new List<Card>
            {
                new Card(CardSuit.Club, CardType.Two),
                new Card(CardSuit.Heart, CardType.Three),
            };

        private static List<Card> averagePlayerHand = new List<Card>
            {
                new Card(CardSuit.Club, CardType.Queen),
                new Card(CardSuit.Club, CardType.Five),
            };

        [TestMethod]
        public void GetDeckShouldRemoveProperAmountOfCards1()
        {
            var deck = HandStrengthCalculator.GetDeck(new List<Card>(), new List<Card> { new Card(CardSuit.Club, CardType.Ace) });

            Assert.AreEqual(51, deck.Count);
        }

        [TestMethod]
        public void GetDeckShouldRemoveProperAmountOfCards2()
        {
            var deck = HandStrengthCalculator.GetDeck(straightFlushCommunityCards, straightFlushPlayerCards);

            Assert.AreEqual(45, deck.Count);
        }

        [TestMethod]
        public void CalculateShouldReturnPlausibleHandStrength1()
        {
            var handStrength = HandStrengthCalculator.Calculate(straightFlushCommunityCards, straightFlushPlayerCards);

            Assert.AreEqual(1, handStrength);
        }

        [TestMethod]
        public void CalculateShouldReturnPlausibleHandStrength2()
        {
            var handStrength = HandStrengthCalculator.Calculate(new List<Card>(), weakPlayerHand);

            //Assert.AreEqual(1, handStrength);
            Assert.IsTrue(handStrength < .34 && handStrength > .24);
        }

        [TestMethod]
        public void CalculateShouldReturnPlausibleHandStrength3()
        {
            var handStrength = HandStrengthCalculator.Calculate(new List<Card>(), averagePlayerHand);

            //Assert.AreEqual(1, handStrength);
            Assert.IsTrue(handStrength < .55 && handStrength > .45);
        }
    }
}
