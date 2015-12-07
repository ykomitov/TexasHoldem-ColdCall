namespace TexasHoldem.AI.ColdCallPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using Logic.Cards;
    using TexasHoldem.Logic.Players;

    internal class TodorPlayer : BasePlayer
    {
        private const int StatesCount = 2;
        private readonly string name = "ColdCall_" + Guid.NewGuid();

        private int totalGamesCount = 0;
        private PlayerStateType state;
        private int[] gamesCount;
        private int[] winsCount;
        private double[] successRate;
        private int stateEvaluationGamesCount = 5;

        public TodorPlayer()
        {
            this.gamesCount = new int[StatesCount];
            this.winsCount = new int[StatesCount];
            this.successRate = Enumerable.Repeat(1d, StatesCount).ToArray();
            this.state = (PlayerStateType)new Random().Next(0, StatesCount);
        }

        public override string Name { get { return this.name; } }

        protected double HandStrength { get; set; }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            switch (this.state)
            {
                case PlayerStateType.SafeAllIn:
                    return this.SafeAllInState(context);
                case PlayerStateType.AllInProtection:
                    return this.AllInProtectionState(context);
                default:
                    break;
            }

            throw new InvalidOperationException("No such state found.");
        }

        private PlayerAction AllInProtectionState(GetTurnContext context)
        {
            const double HandStrengthMargin = .6; // .6.1
            double blindRatioMargin = .01;
            int initialMoney = 1000;

            var bigBlind = context.SmallBlind * 2;
            var blindRatio = bigBlind / initialMoney;
            var moneyLeft = context.MoneyLeft;

            if (this.HandStrength < HandStrengthMargin
                && blindRatio < blindRatioMargin
                && moneyLeft != 0)
            {
                return PlayerAction.Fold();
            }

            return PlayerAction.CheckOrCall();
        }

        public override void StartRound(StartRoundContext context)
        {
            this.HandStrength = OddsCalculator.CalculateHandStrength(context.CommunityCards.ToList(), new List<Card> { this.FirstCard, this.SecondCard });

            base.StartRound(context);
        }

        public override void EndGame(EndGameContext context)
        {
            var stateIndex = (int)this.state;
            this.totalGamesCount++;
            this.gamesCount[stateIndex]++;

            if (context.WinnerName == this.Name)
            {
                this.winsCount[stateIndex]++;
            }

            if (this.totalGamesCount % this.stateEvaluationGamesCount == 0)
            {
                this.successRate[stateIndex] =
                    (double)this.winsCount[stateIndex] / this.gamesCount[stateIndex];

                int bestState = 0;
                double bestSuccessRate = 0;
                for (int i = 0; i < this.successRate.Length; i++)
                {
                    if (this.successRate[i] >= bestSuccessRate)
                    {
                        bestState = i;
                        bestSuccessRate = this.successRate[i];
                    }
                }

                this.state = (PlayerStateType)bestState;
            }
        }

        private PlayerAction SafeAllInState(GetTurnContext context)
        {
            // if they raised more than a certain proportion of my money and I don`t have strong hand fold
            if (context.PreviousRoundActions.Count > 0 && context.PreviousRoundActions.Last().Action.Type == PlayerActionType.Raise)
            {
                if (context.MoneyToCall >= context.MoneyLeft / 2 && this.HandStrength < .8)
                {
                    return PlayerAction.Fold();
                }
            }

            if (this.HandStrength < .42 && !context.CanCheck)
            {
                return PlayerAction.Fold();
            }

            var raise = context.MoneyLeft;
            if (this.HandStrength > .95 && raise > 0)
            {
                return PlayerAction.Raise(raise / 2);
            }

            return PlayerAction.CheckOrCall();
        }
    }
}