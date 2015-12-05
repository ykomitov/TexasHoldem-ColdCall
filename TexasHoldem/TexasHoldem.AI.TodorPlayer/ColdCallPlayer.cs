namespace TexasHoldem.AI.ColdCallPlayer
{
    using System;
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

        public ColdCallPlayer()
            : base()
        {
            this.playerStates = new IPlayerState[]
            {
                new SafeAllInState(),
            };

            this.rand = new RandomGenerator();

            var randomIndex = this.rand.GetRandomInteger(0, this.playerStates.Length);

            this.state = this.playerStates[randomIndex];
        }

        public override string Name { get { return this.name; } }

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