namespace ForceTexasHoldemPlayer
{
    using Helpers;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    internal class DecisionMaker
    {
        internal static PlayerAction Deside(CardValuationType playHand, GetTurnContext context)
        {
            if (playHand == CardValuationType.Unplayable)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }
            else if (playHand == CardValuationType.Risky)
            {
                var smallBlindsTimes = RandomProvider.Next(1, 8);

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }
            else if (playHand == CardValuationType.Recommended)
            {
                var smallBlindsTimes = RandomProvider.Next(6, 14);

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }
            else if (playHand == CardValuationType.HighRecommended)
            {

                var smallBlindsTimes = RandomProvider.Next(12, 50);

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }

            return PlayerAction.CheckOrCall();
        }
    }
}
