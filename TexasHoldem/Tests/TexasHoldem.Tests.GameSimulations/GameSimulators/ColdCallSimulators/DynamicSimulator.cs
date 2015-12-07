namespace TexasHoldem.Tests.GameSimulations.GameSimulators.ColdCallSimulators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Logic.Players;
    using AI.ColdCallPlayer;
    using AI.DummyPlayer;
    using Bullets.Logic;
    using AI.SmartPlayer;

    public class DynamicSimulator : BaseGameSimulator
    {
        public DynamicSimulator(IPlayer first, IPlayer second)
        {
            this.firstPlayer = first;
            this.secondPlayer = second;       
        }

        public string Name { get; set; }

        private readonly IPlayer firstPlayer;
        private readonly IPlayer secondPlayer;

        protected override IPlayer GetFirstPlayer()
        {
            return this.firstPlayer;
        }

        protected override IPlayer GetSecondPlayer()
        {
            return this.secondPlayer;
        }

        public override string ToString()
        {
            return this.firstPlayer.GetType().Name + " vs " + this.secondPlayer.GetType().Name;
        }
    }
}
