namespace ForceTexasHoldemPlayer
{
    using System.Collections.Generic;

    using AI;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Players;

    internal class PlayersAI
    {
        /// <summary>
        /// Index of strength of the current hand. Between 0 (worst) and 8 (best).
        /// </summary>
        private byte handIndex;
        private InitialHandStrengthEvaluator initialHandStrength;
        private BestHandEvaluator bestHandStrenght;
        private ProbabilityEvaluator probabilityEvaluator;
        private DecisionMaker decisionMaker;

        public PlayersAI()
        {
            this.Action = PlayerAction.CheckOrCall();
            this.initialHandStrength = new InitialHandStrengthEvaluator();
            this.bestHandStrenght = new BestHandEvaluator();
            this.probabilityEvaluator = new ProbabilityEvaluator();
            this.decisionMaker = new DecisionMaker();
        }

        public PlayerAction Action { get; internal set; }

        internal void ProcessCurrentRound(GetTurnContext context, Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards)
        {
            float[] probabilityOfAllHands = new float[9];
 
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var firstHandProbability = initialHandStrength.EvaluateHand(firstCard, secondCard);

                this.Action = decisionMaker.DesideByChance(firstHandProbability, context);
            }
            else
            {
                this.handIndex = bestHandStrenght.Evaluate(firstCard, secondCard, communityCards);
                probabilityOfAllHands = probabilityEvaluator.Evaluate(firstCard, secondCard, communityCards, context.RoundType);

                this.Action = decisionMaker.Deside(probabilityOfAllHands, context, handIndex);
            }
        }
    }
}
