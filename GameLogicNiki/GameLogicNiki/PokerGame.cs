namespace GameLogicNiki
{
    using System;

    using TexasHoldem.Logic.GameMechanics;

    public class PokerGame
    {
        public static void Main()
        {
            var player1 = new SmartPlayer();
            var player2 = new SmartPlayer();

            var game = new TwoPlayersTexasHoldemGame(player1, player2);

            Console.WriteLine("Winner is: {0}", game.Start().Name);
            
            Console.WriteLine("Rounds played: {0}", game.HandsPlayed);
        }
    }
}
