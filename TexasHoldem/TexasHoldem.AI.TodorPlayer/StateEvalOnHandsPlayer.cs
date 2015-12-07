namespace TexasHoldem.AI.ColdCallPlayer
{
    using System;
    using System.Linq;
    using Helpers;
    using Logic.Players;
    using PlayerStates;
    using PlayerStates.Todor;
    using PlayerStates.Yavor;

    internal class StateEvalOnHandsPlayer : BasePlayer
    {
        private readonly string name = "ColdCall_" + Guid.NewGuid();
        private readonly int stateEvaluationHandsCount = 5;

        private IPlayerState[] playerStates;
        private IPlayerState state;
        private RandomGenerator rand;
        private int totalGamesCount;
        private int prevHandMoney;
        private int currentGameTotalHandsCount;

        public StateEvalOnHandsPlayer()
            : base()
        {
            this.playerStates = new IPlayerState[]
            {
                new SafeAllInState(),
                new RecklessAllInState(),
                new NormalPlayerState(),
                new AlwaysAllInProtectionState()
            };

            this.rand = new RandomGenerator();

            var randomIndex = this.rand.GetRandomInteger(0, this.playerStates.Length);

            this.state = this.playerStates[randomIndex];
        }

        public override string Name => this.name;

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return this.state.GetTurn(context);
        }

        public override void StartRound(StartRoundContext context)
        {
            this.state.StartRound(context);
            base.StartRound(context);
        }

        public override void StartHand(StartHandContext context)
        {
            if (this.currentGameTotalHandsCount != 0)
            {
                if (context.MoneyLeft > this.prevHandMoney)
                {
                    this.state.HandsWon++;
                }
            }

            this.currentGameTotalHandsCount++;
            this.state.HandsPlayed++;
            this.prevHandMoney = context.MoneyLeft;

            this.state.StartHand(context);
            base.StartHand(context);
        }

        public override void StartGame(StartGameContext context)
        {
            this.currentGameTotalHandsCount = 0;
            this.state.StartGame(context);
            base.StartGame(context);
        }

        public override void EndGame(EndGameContext context)
        {
            this.state.EndGame(context);
            base.EndGame(context);
        }

        public override void EndHand(EndHandContext context)
        {
            if (this.state.HandsPlayed > 0 && this.state.HandsPlayed % 100 == 0)
            {
                var bestRate = this.playerStates.Max(x => x.HandsSuccessRate);
                var bestState = this.playerStates.First(x => x.HandsSuccessRate == bestRate);
                this.state = bestState;
            }

            this.state.EndHand(context);
            base.EndHand(context);
        }

        public override void EndRound(EndRoundContext context)
        {
            this.state.EndRound(context);
            base.EndRound(context);
        }
    }
}