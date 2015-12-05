namespace TexasHoldem.AI.ColdCallPlayer
{
    using System;
    using System.Linq;
    using Helpers;
    using Logic.Players;
    using PlayerStates;
    using PlayerStates.Todor;

    public class ColdCallPlayer : BasePlayer
    {
        private readonly string name = "ColdCall_" + Guid.NewGuid();

        private IPlayerState[] playerStates;

        private IPlayerState state;

        private RandomGenerator rand;

        private int totalGamesCount;

        public ColdCallPlayer()
            : base()
        {
            this.playerStates = new IPlayerState[]
            {
                new SafeAllInState(),
                new RecklessAllInState()
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
            this.state.StartHand(context);
            base.StartHand(context);
        }

        public override void StartGame(StartGameContext context)
        {
            this.state.StartGame(context);
            base.StartGame(context);
        }

        public override void EndGame(EndGameContext context)
        {
            this.totalGamesCount++;

            this.state.GamesPlayed++;
            if (context.WinnerName == this.Name)
            {
                this.state.GamesWon++;
            }

            if (this.totalGamesCount % 50 == 0)
            {
                var bestSuccessRate = this.playerStates.Max(x => x.SuccessRate);
                var bestState = this.playerStates.First(x => x.SuccessRate == bestSuccessRate);
                this.state = bestState;
            }

            this.state.EndGame(context);
            base.EndGame(context);
        }

        public override void EndHand(EndHandContext context)
        {
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