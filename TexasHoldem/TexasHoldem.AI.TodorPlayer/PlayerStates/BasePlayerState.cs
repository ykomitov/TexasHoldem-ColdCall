namespace TexasHoldem.AI.ColdCallPlayer.PlayerStates
{
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using Logic.Cards;
    using Logic.Players;

    public abstract class BasePlayerState : IPlayerState
    {
        public string Name => this.GetType().Name;

        protected double HandStrength { get; set; }

        protected IReadOnlyCollection<Card> CommunityCards { get; private set; }

        protected Card FirstCard { get; private set; }

        protected Card SecondCard { get; private set; }

        public virtual void StartGame(StartGameContext context)
        {
        }

        public virtual void StartHand(StartHandContext context)
        {
            this.FirstCard = context.FirstCard;
            this.SecondCard = context.SecondCard;
        }

        public virtual void StartRound(StartRoundContext context)
        {
            this.HandStrength = HandStrengthCalculator.Calculate(context.CommunityCards.ToList(), new List<Card> { this.FirstCard, this.SecondCard });
            this.CommunityCards = context.CommunityCards;
        }

        public abstract PlayerAction GetTurn(GetTurnContext context);

        public virtual void EndRound(EndRoundContext context)
        {
        }

        public virtual void EndHand(EndHandContext context)
        {
        }

        public virtual void EndGame(EndGameContext context)
        {
        }
    }
}