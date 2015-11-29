namespace TexasHoldem.AI.TodorPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using Logic.Cards;
    using Logic.Players;

    public class TodorAllInPlayer : BasePlayer
    {
        private readonly string name = "TodorAllIn_" + Guid.NewGuid();
        private double handStrength = 0;

        public override string Name { get { return this.name; } }

        public override PlayerAction GetTurn(GetTurnContext context)
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

        public override void StartRound(StartRoundContext context)
        {
            base.StartRound(context);
            this.handStrength = HandStrengthCalculator.Calculate(context.CommunityCards.ToList(), new List<Card> { this.FirstCard, this.SecondCard });
        }
    }
}