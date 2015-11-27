namespace ForceTexasHoldemPlayer.Helpers
{
    internal enum CardValuationType
    {
        Unplayable = 0,
        NotRecommended = 1000,
        Risky = 2000,
        Recommended = 3000,
        HighRecommended = 4000,
        Bluffing = 10000
    }
}
