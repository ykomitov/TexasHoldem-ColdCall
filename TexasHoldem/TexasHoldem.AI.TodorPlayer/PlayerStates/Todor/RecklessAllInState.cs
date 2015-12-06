namespace TexasHoldem.AI.ColdCallPlayer.PlayerStates.Todor
{
    using TexasHoldem.Logic.Players;

    internal class RecklessAllInState : BasePlayerState
    {
        public override PlayerAction GetTurn(GetTurnContext context)
        {
            var raise = context.MoneyLeft;
            if (this.HandStrength > .95 && raise > 0)
            {
                return PlayerAction.Raise(raise);
            }

            return PlayerAction.CheckOrCall();
        }
    }
}