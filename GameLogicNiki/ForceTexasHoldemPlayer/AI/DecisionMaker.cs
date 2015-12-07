namespace ForceTexasHoldemPlayer.AI
{
    using System.Linq;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    internal class DecisionMaker
    {
        internal PlayerAction Deside(float[] probabilityOfAllHands, GetTurnContext context, byte bestHand)
        {
            PlayerAction action;

            if (context.RoundType != GameRoundType.River)
            {
                if (bestHand < 2)
                {
                    float maximalProbability = probabilityOfAllHands.Max();

                    action = DesideByChance(maximalProbability, context);
                }
                else
                {
                    action = DesideByHand(bestHand, context);
                }
            }
            else
            {
                action = DesideByHand(bestHand, context);
            }

            return action;
        }

        private PlayerAction DesideByHand(byte bestHand, GetTurnContext context)
        {
            // don't play if chance is less than 50%
            if (bestHand <= 2)
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
            else if (bestHand < 4)
            {
                var smallBlindsTimes = RandomProvider.Next(5, 20);

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }
            else if (bestHand < 5)
            {
                var smallBlindsTimes = RandomProvider.Next(15, 30);

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }
            else
            {
                var smallBlindsTimes = RandomProvider.Next(25, 100);

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }
        }

        internal PlayerAction DesideByChance(float firstHandProbabillity, GetTurnContext context)
        {
            // don't play if chance is less than 50%
            if (firstHandProbabillity < 0.41f)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    if (context.CurrentPot < (2 * context.SmallBlind))
                    {
                        var smallBlindsTimes = RandomProvider.Next(1, 6);
                        return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                    }

                    return PlayerAction.Fold();
                }
            }
            else if (firstHandProbabillity < 0.51f)
            {
                var smallBlindsTimes = RandomProvider.Next(5, 20);

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }
            else if (firstHandProbabillity < 0.65f)
            {
                var smallBlindsTimes = RandomProvider.Next(15, 30);

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }
            else
            {
                var smallBlindsTimes = RandomProvider.Next(25, 100);

                return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            }
        }
    }
}
