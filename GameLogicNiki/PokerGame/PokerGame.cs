namespace GameLogicNiki
{
    using System;

    using ForceTexasHoldemPlayer;
    using TexasHoldem.Logic.GameMechanics;
    using SmartPlayerProject;

    public class PokerGame
    {
        private const int gamePlaied = 10;
        private const bool printing = true;

        public static void Main()
        {
            var player1 = new SmartPlayer();
            var player2 = new ForcePlayer();

            int player1WinIndex = 1;
            int player2WinIndex = 1;

            for (int j = 0; j < gamePlaied; j++)
            {
                string winnersName;

                int whoIsWinning = 0;

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
                    winnersName = player1.Name;

                    player1WinIndex++;
                }
                else if (whoIsWinning < 0)
                {
                    winnersName = player2.Name;
                    player2WinIndex++;
                }
                else
                {
                    winnersName = "No Winner!";
                }

                if (printing)
                {
                    Console.WriteLine("*");
                    Console.WriteLine("---- Game No {0}", j);
                    Console.WriteLine("Winner is: {0}", winnersName);
                    Console.WriteLine("Game ratio: {0}", whoIsWinning);
                }
                else
                {
                    Console.Write('-');
                }
            }

            Console.WriteLine();
            Console.WriteLine("Players Wining Ratio: {0} / {1}", player2.Name, player1.Name);
            Console.WriteLine(" {0:F2} ",  (float)player2WinIndex / player1WinIndex);
        }
    }
}
