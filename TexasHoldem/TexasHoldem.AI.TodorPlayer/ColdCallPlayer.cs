namespace TexasHoldem.AI.ColdCallPlayer
{
    using System;
    using Logic.Players;
    using TodorPlayer.Helpers;
    using TodorPlayer.PlayerStates.Todor;

    public class ColdCallPlayer : BasePlayer
    {
        private readonly string name = "ColdCall_" + Guid.NewGuid();

        private IPlayer[] playerStates;

        private IPlayer contextPlayer;

        private RandomGenerator rand;

        public ColdCallPlayer()
            : base()
        {
            this.playerStates = new IPlayer[]
            {
                new SafeAllInState(),
            };

            this.rand = new RandomGenerator();

            var randomIndex = this.rand.GetRandomInteger(0, this.playerStates.Length);

            this.contextPlayer = this.playerStates[randomIndex];
        }

        public override string Name { get { return this.name; } }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return this.contextPlayer.GetTurn(context);
        }

        public override void StartRound(StartRoundContext context)
        {
            this.contextPlayer.StartRound(context);
            base.StartRound(context);
        }

        public override void StartHand(StartHandContext context)
        {
            this.contextPlayer.StartHand(context);
            base.StartHand(context);
        }

        public override void StartGame(StartGameContext context)
        {
            this.contextPlayer.StartGame(context);
            base.StartGame(context);
        }

        public override void EndGame(EndGameContext context)
        {
            this.contextPlayer.EndGame(context);
            base.EndGame(context);
        }

        public override void EndHand(EndHandContext context)
        {
            this.contextPlayer.EndHand(context);
            base.EndHand(context);
        }

        public override void EndRound(EndRoundContext context)
        {
            this.contextPlayer.EndRound(context);
            base.EndRound(context);
        }
    }
}