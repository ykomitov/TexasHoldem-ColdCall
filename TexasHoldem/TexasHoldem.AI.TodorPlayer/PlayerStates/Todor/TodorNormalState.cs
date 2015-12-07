namespace TexasHoldem.AI.ColdCallPlayer.PlayerStates.Todor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using Logic.Cards;
    using TexasHoldem.Logic.Players;

    internal class TodorNormalState : BasePlayerState
    {
        private const double HandStrengthMargin = .6;

        // .6.1
        private const double BlindRatioMargin = .01;

        private int initialMoney = 1000;

        private int otherAllInsCount;
        private int handsCount;

        private bool otherIsAllInPlayer = false;

        private bool otherIsAlwaysRaise = false;

        private int otherRaiseInRowCount = 0;
        private PlayerActionType otherLastTurnAction;

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.MoneyLeft + context.CurrentPot == 2000)
            {
                this.otherAllInsCount++;
            }

            // all in protection
            if (this.otherIsAllInPlayer || this.otherAllInsCount > 9)
            {
                this.otherIsAllInPlayer = true;
                return this.AllInProtection(context);
            }

            if (context.PreviousRoundActions.Count > 0
                && context.PreviousRoundActions.Last().Action.Type == PlayerActionType.Raise
                && this.otherLastTurnAction == PlayerActionType.Raise)
            {
                this.otherRaiseInRowCount++;
            }
            else
            {
                this.otherRaiseInRowCount = 0;
            }

            if (context.PreviousRoundActions.Count > 0)
            {
                this.otherLastTurnAction = context.PreviousRoundActions.Last().Action.Type;
            }

            if (this.otherIsAlwaysRaise || this.otherRaiseInRowCount > 20)
            {
                this.otherIsAlwaysRaise = true;
                return this.AlwaysRaiseProtection(context);
            }

            // if they raised more than a certain proportion of my money and I don`t have strong hand fold
            if (context.PreviousRoundActions.Count > 0 && context.PreviousRoundActions.Last().Action.Type == PlayerActionType.Raise)
            {
                if (context.MoneyToCall >= context.MoneyLeft / 2 && this.HandStrength < .75)
                {
                    return PlayerAction.Fold();
                }
            }

            if (this.HandStrength < .42 && !context.CanCheck)
            {
                return PlayerAction.Fold();
            }

            var myMoney = context.MoneyLeft;
            if (this.HandStrength > .95 && myMoney > 0)
            {
                return PlayerAction.Raise(myMoney / 2);
            }

            if (this.HandStrength > .75 && myMoney > 0)
            {
                return PlayerAction.Raise(myMoney / 4);
            }

            return PlayerAction.CheckOrCall();
        }

        private PlayerAction AlwaysRaiseProtection(GetTurnContext context)
        {
            if (context.PreviousRoundActions.Count > 0 && context.PreviousRoundActions.Last().Action.Type == PlayerActionType.Raise)
            {
                if (context.MoneyToCall >= context.MoneyLeft / 2 && this.HandStrength < .8)
                {
                    return PlayerAction.Fold();
                }
            }

            if (this.HandStrength < .42)
            {
                return PlayerAction.Fold();
            }

            var raise = context.MoneyLeft;
            if (this.HandStrength > .95 && raise > 0)
            {
                return PlayerAction.Raise(raise);
            }

            return PlayerAction.CheckOrCall();
        }

        public override void StartRound(StartRoundContext context)
        {
            this.HandStrength = OddsCalculator.CalculateHandStrength(context.CommunityCards.ToList(), new List<Card> { this.FirstCard, this.SecondCard });

            base.StartRound(context);
        }

        public override void EndHand(EndHandContext context)
        {
            if (this.handsCount % 10 == 0)
            {
                this.otherAllInsCount = 0;
            }

            this.handsCount++;

            base.EndHand(context);
        }

        private PlayerAction AllInProtection(GetTurnContext context)
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
    }
}
