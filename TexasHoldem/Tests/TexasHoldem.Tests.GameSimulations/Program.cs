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
            Func<IPlayer> getPlayer = () => new ColdCallPlayer();

            TestAgainstOthers(getPlayer);
            //TodorTests(getPlayer);
            //YavorTests(getPlayer);
        }

        private static void TodorTests(Func<IPlayer> getPlayer)
        {
            var p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new AlwaysAllInDummyPlayer()));
        }

        private static void YavorTests(Func<IPlayer> getPlayer)
        {
            var p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new SafeAllInState()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new RecklessAllInState()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new DummyPlayer()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new AlwaysAllInDummyPlayer()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new AlwaysCallDummyPlayer()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new AlwaysRaiseDummyPlayer()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new SmartPlayer()));
        }

        private static void TestAgainstOthers(Func<IPlayer> getPlayer)
        {
            var p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new DummyPlayer()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new AlwaysAllInDummyPlayer()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new AlwaysCallDummyPlayer()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new AlwaysRaiseDummyPlayer()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new SmartPlayer()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new SafeAllInState()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new RecklessAllInState()));
            p = getPlayer();
            SimulateGames(new DynamicSimulator(p, new NormalState()));
        }

        public static void SimulateGames(IGameSimulator gameSimulator)
        {
            //Console.WriteLine($"Running {gameSimulator.GetType().Name}...");
            Console.WriteLine($"Running {gameSimulator}...");

            var simulationResult = gameSimulator.Simulate(100);

            //Console.WriteLine(simulationResult.SimulationDuration);
            Console.WriteLine($"Total games: {simulationResult.FirstPlayerWins:0,0} - {simulationResult.SecondPlayerWins:0,0}");
            //Console.WriteLine($"Hands played: {simulationResult.HandsPlayed:0,0}");
            Console.WriteLine(new string('=', 50));
        }
    }
}
