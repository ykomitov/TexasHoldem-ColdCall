namespace TexasHoldem.Tests.GameSimulations
{
    using System;
    using System.Collections.Generic;
    using AI.ColdCallPlayer;
    using AI.ColdCallPlayer.PlayerStates.Todor;
    using AI.ColdCallPlayer.PlayerStates.Yavor;
    using AI.DummyPlayer;
    using AI.SmartPlayer;
    using GameSimulators.ColdCallSimulators;
    using Logic.Players;
    using TexasHoldem.Tests.GameSimulations.GameSimulators;

    public static class Program
    {
        private static List<IPlayer> allOpponents = new List<IPlayer>()
        {
            new AlwaysAllInDummyPlayer(),
            new AlwaysCallDummyPlayer(),
            new AlwaysRaiseDummyPlayer(),
            new SmartPlayer(),

            //new SafeAllInState(),
            //new RecklessAllInState(),
            new NormalPlayerState(),

            //new TodorPlayer()
        };

        public static void Main()
        {
            TodorTests();

            //YavorTests();
        }

        public static GameSimulationResult SimulateGames(IGameSimulator gameSimulator)
        {
            //Console.WriteLine($"Running {gameSimulator.GetType().Name}...");
            Console.WriteLine($"Running {gameSimulator}...");

            var simulationResult = gameSimulator.Simulate(100);

            //Console.WriteLine(simulationResult.SimulationDuration);
            Console.WriteLine($"Total games: {simulationResult.FirstPlayerWins:0,0} - {simulationResult.SecondPlayerWins:0,0}");

            //Console.WriteLine($"Hands played: {simulationResult.HandsPlayed:0,0}");
            Console.WriteLine(new string('=', 50));
            return simulationResult;
        }

        private static void TodorTests()
        {
            Func<IPlayer> getPlayer = () => new StateEvalOnGamesPlayer();

            var others = new List<IPlayer>()
            {
                new NormalPlayerState()
            };

            TestAgainstOthers(getPlayer, allOpponents);
        }

        private static void YavorTests()
        {
            Func<IPlayer> getPlayer = () => new TodorPlayer();

            var others = new List<IPlayer>()
            {
                new SmartPlayer()
            };

            TestAgainstOthers(getPlayer, others);
        }

        private static void TestAgainstOthers(Func<IPlayer> getPlayer, IList<IPlayer> opponents)
        {
            var wins = 0;
            var totalPoints = 0;
            foreach (var o in opponents)
            {
                var p = getPlayer();
                var res = SimulateGames(new DynamicSimulator(p, o));
                wins += res.FirstPlayerWins;
                totalPoints += res.FirstPlayerWins + res.SecondPlayerWins;
            }

            Console.WriteLine($"Total wins: {wins}/{totalPoints}({((double)wins / totalPoints) * 100}%)");
        }
    }
}