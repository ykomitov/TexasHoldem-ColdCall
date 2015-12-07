namespace TexasHoldem.AI.ColdCallPlayer.PlayerStates
{
    using TexasHoldem.Logic.Players;

    internal interface IPlayerState
    {
        string Name { get; }

        int GamesPlayed { get; set; }

        int GamesWon { get; set; }

        double GamesSuccessRate { get; }

        double HandsSuccessRate { get; }

        int HandsWon { get; set; }

        int HandsPlayed { get; set; }

        void StartGame(StartGameContext context);

        void StartHand(StartHandContext context);

        void StartRound(StartRoundContext context);

        PlayerAction GetTurn(GetTurnContext context);

        void EndRound(EndRoundContext context);

        void EndHand(EndHandContext context);

        void EndGame(EndGameContext context);
    }
}