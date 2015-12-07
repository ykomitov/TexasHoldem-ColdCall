namespace ForceTexasHoldemPlayer
{
    using TexasHoldem.Logic.Players;

    public class ForcePlayer : BasePlayer
    {
        private readonly string name = "TheForcePlayer_" + System.Guid.NewGuid();
        private PlayersAI ai;

        public ForcePlayer()
        {
            this.ai = new PlayersAI();
        }

        public override string Name
        {
            get
            {
                return this.name;
            }
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            ai.ProcessCurrentRound(context, this.FirstCard, this.SecondCard, this.CommunityCards);

            return ai.Action;
        }
    }
}
