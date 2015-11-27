namespace GameLogicNiki
{
    using System;

    using ForceTexasHoldemPlayer;
    using TexasHoldem.Logic.GameMechanics;

    public class PokerGame
    {
        public static void Main()
        {
            int whoIsWinning = 0;

            var player1 = new SmartPlayer();
            var player2 = new ForcePlayer();

            for (int i = 0; i < 1000; i++)
            {
                var game = new TwoPlayersTexasHoldemGame(player1, player2);

                if (game.Start().Name == player1.Name)
                {
                    whoIsWinning++;
                }
                else
                {
                    whoIsWinning--;
                }
            }

            if (whoIsWinning > 0)
            {
                Console.WriteLine("Winner is: {0}", player1.Name);
            }
            else if (whoIsWinning < 0)
            {
                Console.WriteLine("Winner is: {0}", player2.Name);
            }
            else
            {
                Console.WriteLine("The game is tied!");
            }

            Console.WriteLine("Game ratio: {0}", whoIsWinning);
        }
    }
}
