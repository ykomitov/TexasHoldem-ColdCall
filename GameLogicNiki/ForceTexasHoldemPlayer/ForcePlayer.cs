namespace ForceTexasHoldemPlayer
{
    using Helpers;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Players;
    
    public class ForcePlayer : BasePlayer
    {
        private readonly string name = "-- The Force Player --";

        public override string Name
        {
            get
            {
                return this.name;
            }
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            CardValuationType playHand = CardValuationType.Risky;

            if (context.RoundType == GameRoundType.PreFlop)
            {
                playHand = HandStrengthValuation.PreFlop(this.FirstCard, this.SecondCard);
            }
            else
            {
                playHand = HandStrengthValuation.InGame(this.FirstCard, this.SecondCard, this.CommunityCards);
            }

            return DecisionMaker.Deside(playHand, context);
        }
    }
}
