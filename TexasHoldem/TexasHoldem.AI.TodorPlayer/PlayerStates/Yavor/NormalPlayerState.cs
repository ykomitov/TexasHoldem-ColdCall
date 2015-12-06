namespace TexasHoldem.AI.ColdCallPlayer.PlayerStates.Yavor
{
    using Helpers;
    using Logic.Players;

    internal class NormalPlayerState : BasePlayerState
    {
        private const double NotPlayableThreshold = 0.8;
        private const double NotReccomendedThreshold = 1.0;
        private const double PlayableThreshold = 1.3;

        private RandomGenerator random = new RandomGenerator();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            // Calculate pot odds & rate of return in the beginning
            this.PotOdds = OddsCalculator.CalculatePotOdds(
                context.CurrentPot,
                context.MoneyToCall,
                context.SmallBlind * 2);

            this.RateOfReturn = OddsCalculator.CalculateRateOfReturn(this.HandStrength, this.PotOdds);

            /*If RR < 0.8 then 95% fold, 0 % call, 5% raise (bluff)   // Not playable threshold
              If RR < 1.0 then 80% fold, 5% call, 15% raise (bluff)   // Not reccomended threshold
              If RR <1.3 then 0% fold, 60% call, 40% raise            // Playable threshold
              Else (RR >= 1.3) 0% fold, 30% call, 70% raise
              If fold and amount to call is zero, then call.*/

            // If we can check or call without paying any money, do so
            if (this.HandStrength < .4 && context.CanCheck == true)
            {
                return PlayerAction.CheckOrCall();
            }

            // Fold in all win scenarios with weak hand
            double allWinPlayHandStrength = 0.95;
            bool opponentIsAllIn = (context.MoneyLeft + context.CurrentPot) == 2000;
            if (opponentIsAllIn && this.HandStrength <= allWinPlayHandStrength)
            {
                return PlayerAction.Fold();
            }

            if (opponentIsAllIn && this.HandStrength > allWinPlayHandStrength)
            {
                return PlayerAction.CheckOrCall();
            }

            // Calculate random between 0.0 and 1.0 for bluff scenarios
            double decisionCoefficient = this.random.GetRandomDouble();

            if (this.RateOfReturn <= NotPlayableThreshold)
            {
                if (decisionCoefficient <= .95)
                {
                    return PlayerAction.Fold();
                }
                else
                {
                    return PlayerAction.Raise(context.SmallBlind * 2);
                }
            }
            else if (this.RateOfReturn > NotPlayableThreshold && this.RateOfReturn <= NotReccomendedThreshold)
            {
                if (decisionCoefficient <= .8) // 0.8
                {
                    return PlayerAction.Fold();
                }
                else if (decisionCoefficient > .8 && decisionCoefficient <= .85) // 0.80 - 0.85
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Raise(context.SmallBlind * 2);
                }
            }
            else if (this.RateOfReturn > NotReccomendedThreshold && this.RateOfReturn < PlayableThreshold)
            {
                if (decisionCoefficient <= .6)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Raise(context.SmallBlind * 2);
                }
            }
            else
            {
                if (decisionCoefficient <= .3)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Raise(context.SmallBlind * 2);
                }
            }
        }
    }
}
