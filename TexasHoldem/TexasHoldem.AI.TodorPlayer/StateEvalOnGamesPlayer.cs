namespace TexasHoldem.AI.ColdCallPlayer
{
    using System;
    using System.Linq;
    using Helpers;
    using Logic.Players;
    using PlayerStates;
    using PlayerStates.Todor;
    using PlayerStates.Yavor;

    public class StateEvalOnGamesPlayer : IPlayer
    {
        private readonly string name = "ColdCall_" + Guid.NewGuid();
        private readonly int stateEvaluationGamesCount = 5;

        private IPlayerState[] playerStates;
        private IPlayerState state;
        private RandomGenerator rand;
        private int totalGamesCount;

        public StateEvalOnGamesPlayer()
            : base()
        {
            this.playerStates = new IPlayerState[]
            {
                new SafeAllInState(),
                //new NormalPlayerState(),
                //new AlwaysAllInProtectionState(),
                //new TodorNormalState(),
            };

            this.rand = new RandomGenerator();

            var randomIndex = this.rand.GetRandomInteger(0, this.playerStates.Length);

            this.state = this.playerStates[randomIndex];
        }

        public string Name => this.name;

        public PlayerAction GetTurn(GetTurnContext context)
        {
            return this.state.GetTurn(context);
        }

        public void StartRound(StartRoundContext context)
        {
            this.state.StartRound(context);
        }

        public void StartHand(StartHandContext context)
        {
            this.state.StartHand(context);
        }

        public void StartGame(StartGameContext context)
        {
            this.state.StartGame(context);
        }

        public void EndGame(EndGameContext context)
        {
            this.totalGamesCount++;

            this.state.GamesPlayed++;
            if (context.WinnerName == this.Name)
            {
                this.state.GamesWon++;
            }

            if (this.totalGamesCount % this.stateEvaluationGamesCount == 0)
            {
                var bestSuccessRate = this.playerStates.Max(x => x.GamesSuccessRate);
                var bestState = this.playerStates.First(x => x.GamesSuccessRate == bestSuccessRate);
                this.state = bestState;
            }

            this.state.EndGame(context);
        }

        public void EndHand(EndHandContext context)
        {
            this.state.EndHand(context);
        }

        public void EndRound(EndRoundContext context)
        {
            this.state.EndRound(context);
        }
    }
}