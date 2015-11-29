namespace TexasHoldem.Tests.GameSimulations.GameSimulators.Todor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Logic.Players;
    using AI.TodorPlayer;
    using AI.SmartPlayer;

    public class TodorAllInVsSmartPlayerSimulation : BaseGameSimulator
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
