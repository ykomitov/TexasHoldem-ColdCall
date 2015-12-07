namespace TexasHoldem.AI.ColdCallPlayer.PlayerStates.Yavor
{
    using Logic.Players;

    internal class AllInBusterState : BasePlayerState
    {
        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.CanCheck && this.HandStrength > .1)
            {
                return PlayerAction.Raise(context.MoneyLeft);
            }
            else if (context.CanCheck && this.HandStrength <= .1)
            {
                return PlayerAction.CheckOrCall();
            }
            else
            {
                return PlayerAction.CheckOrCall();
            }
        }
    }
}
