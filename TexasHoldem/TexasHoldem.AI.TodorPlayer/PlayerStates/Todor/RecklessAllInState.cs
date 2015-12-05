namespace TexasHoldem.AI.TodorPlayer.PlayerStates.Todor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TexasHoldem.AI.ColdCallPlayer.Helpers;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Players;

    internal class RecklessAllInState : IPlayer
    {
        private double handStrength = 0;

        public Card FirstCard { get; private set; }

        public string Name { get { return this.GetType().Name; } }

        public Card SecondCard { get; private set; }

        public void EndGame(EndGameContext context)
        {
        }

        public void EndHand(EndHandContext context)
        {
        }

        public void EndRound(EndRoundContext context)
        {
        }

        public PlayerAction GetTurn(GetTurnContext context)
        {
            // if they raised more than a certain proportion of my money and I don`t have strong hand fold
            if (context.PreviousRoundActions.Count > 0 && context.PreviousRoundActions.Last().Action.Type == PlayerActionType.Raise)
            {
                if (context.MoneyToCall >= context.MoneyLeft / 2 && this.handStrength < .8)
                {
                    return PlayerAction.Fold();
                }
            }

            if (this.handStrength < .42)
            {
                return PlayerAction.Fold();
            }

            var raise = context.MoneyLeft;
            if (this.handStrength > .95 && raise > 0)
            {
                return PlayerAction.Raise(raise);
            }

            return PlayerAction.CheckOrCall();
        }

        public void StartGame(StartGameContext context)
        {
        }

        public void StartHand(StartHandContext context)
        {
            this.FirstCard = context.FirstCard;
            this.SecondCard = context.SecondCard;
        }

        public void StartRound(StartRoundContext context)
        {
            this.handStrength = HandStrengthCalculator.Calculate(context.CommunityCards.ToList(), new List<Card> { this.FirstCard, this.SecondCard });
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}