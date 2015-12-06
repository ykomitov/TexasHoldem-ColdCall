namespace TexasHoldem.Tests.GameSimulations
{
    using System;

    using TexasHoldem.Tests.GameSimulations.GameSimulators;
    using GameSimulators.ColdCallSimulators;
    using AI.ColdCallPlayer;
    using AI.SmartPlayer;
    using Logic.Players;
    using AI.DummyPlayer;
    using AI.ColdCallPlayer.PlayerStates.Todor;
    using AI.ColdCallPlayer.PlayerStates.Yavor;

    public static class Program
    {
        public static void Main()
        {
            var player = new ColdCallPlayer();
            //var playerNormal = new NormalPlayer();
            //var playerAggressive = new AggressivePlayer();
            //TestAgainstOthers(player);
            //TodorTests(player);
            YavorTests(player);
        }

        private static void TodorTests(IPlayer player1)
        {
            SimulateGames(new DynamicSimulator(player1, new AlwaysAllInDummyPlayer()));
        }

        private static void YavorTests(IPlayer player1)
        {
            SimulateGames(new DynamicSimulator(player1, new SafeAllInState()));
            SimulateGames(new DynamicSimulator(player1, new RecklessAllInState()));
            //SimulateGames(new DynamicSimulator(player1, new DummyPlayer()));
            //SimulateGames(new DynamicSimulator(player1, new AlwaysAllInDummyPlayer()));
            //SimulateGames(new DynamicSimulator(player1, new AlwaysCallDummyPlayer()));
            //SimulateGames(new DynamicSimulator(player1, new AlwaysRaiseDummyPlayer()));
            //SimulateGames(new DynamicSimulator(player1, new SmartPlayer()));
        }

        private static void TestAgainstOthers(IPlayer player1)
        {
            SimulateGames(new DynamicSimulator(player1, new DummyPlayer()));
            SimulateGames(new DynamicSimulator(player1, new AlwaysAllInDummyPlayer()));
            SimulateGames(new DynamicSimulator(player1, new AlwaysCallDummyPlayer()));
            SimulateGames(new DynamicSimulator(player1, new AlwaysRaiseDummyPlayer()));
            SimulateGames(new DynamicSimulator(player1, new SmartPlayer()));
        }

        public static void SimulateGames(IGameSimulator gameSimulator)
        {
            //Console.WriteLine($"Running {gameSimulator.GetType().Name}...");
            Console.WriteLine($"Running {gameSimulator}...");

            var simulationResult = gameSimulator.Simulate(1000);

            //Console.WriteLine(simulationResult.SimulationDuration);
            Console.WriteLine($"Total games: {simulationResult.FirstPlayerWins:0,0} - {simulationResult.SecondPlayerWins:0,0}");
            //Console.WriteLine($"Hands played: {simulationResult.HandsPlayed:0,0}");
            Console.WriteLine(new string('=', 50));
        }
    }
}
