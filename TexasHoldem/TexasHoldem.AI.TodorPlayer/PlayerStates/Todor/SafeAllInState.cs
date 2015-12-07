namespace TexasHoldem.AI.ColdCallPlayer.PlayerStates.Todor
{
    using System;
    using System.Linq;
    using TexasHoldem.Logic.Players;

    internal class SafeAllInState : BasePlayerState
    {
        public override PlayerAction GetTurn(GetTurnContext context)
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