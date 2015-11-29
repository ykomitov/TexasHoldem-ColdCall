namespace TexasHoldem.Tests.GameSimulations.GameSimulators.Todor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Logic.Players;
    using AI.TodorPlayer;
    using AI.DummyPlayer;
    //using Bullets.Logic;
    using AI.SmartPlayer;

    public class CustomSimulation : BaseGameSimulator
    {
        protected override IPlayer GetFirstPlayer()
        {
            return new TodorAllInPlayer();
        }

        protected override IPlayer GetSecondPlayer()
        {
            return new SmartPlayer();
        }
    }
}
