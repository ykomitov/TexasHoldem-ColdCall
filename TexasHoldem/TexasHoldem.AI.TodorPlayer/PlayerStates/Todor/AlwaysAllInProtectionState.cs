namespace TexasHoldem.AI.ColdCallPlayer.PlayerStates.Todor
{
    using TexasHoldem.Logic.Players;

    internal class AlwaysAllInProtectionState : BasePlayerState
    {
        private const double HandStrengthMargin = .6; // .6.1
        private const double BlindRatioMargin = .01;
        private int initialMoney = 1000;

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            var bigBlind = context.SmallBlind * 2;
            var blindRatio = bigBlind / this.initialMoney;
            var moneyLeft = context.MoneyLeft;

            if (this.HandStrength < HandStrengthMargin
                && blindRatio < BlindRatioMargin
                && moneyLeft != 0)
            {
                return PlayerAction.Fold();
            }

            return PlayerAction.CheckOrCall();
        }

        public override void StartGame(StartGameContext context)
        {
            //this.initialMoney = context.StartMoney;
            base.StartGame(context);
        }
    }
}