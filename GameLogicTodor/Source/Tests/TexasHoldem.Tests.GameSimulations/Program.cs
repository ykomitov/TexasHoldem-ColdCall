namespace TexasHoldem.Tests.GameSimulations
{
    using System;

    using TexasHoldem.Tests.GameSimulations.GameSimulators;
    using GameSimulators.Todor;

    public static class Program
    {
        public static void Main()
        {
            //SimulateGames(new TodorAllInVsSmartPlayerSimulation());
            //SimulateGames(new TodorAllInVsAlwaysCallSimulation());
            //SimulateGames(new TodorAllInVsDummySimulation());

            SimulateGames(new CustomSimulation());
        }

        private static void SimulateGames(IGameSimulator gameSimulator)
        {
            Console.WriteLine($"Running {gameSimulator.GetType().Name}...");

            var simulationResult = gameSimulator.Simulate(1000);

            Console.WriteLine(simulationResult.SimulationDuration);
            Console.WriteLine($"Total games: {simulationResult.FirstPlayerWins:0,0} - {simulationResult.SecondPlayerWins:0,0}");
            Console.WriteLine($"Hands played: {simulationResult.HandsPlayed:0,0}");
            Console.WriteLine(new string('=', 75));
        }
    }
}
