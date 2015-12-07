namespace ForceTexasHoldemPlayer.AI
{
    using TexasHoldem.Logic.Cards;

    internal class InitialHandStrengthEvaluator
    {
        private const byte MaxCardTypeValue = 14;

        // http://wizardofodds.com/games/texas-hold-em/2-player-game/
        private static readonly float[,] StartingHandWinningProbabilities =
            {
                { 84.93f, 64.47f, 63.51f, 62.54f, 61.57f, 59.45f, 58.37f, 57.17f, 55.87f, 55.74f, 54.73f, 53.86f, 52.95f }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 64.47f, 82.12f, 60.43f, 59.44f, 58.49f, 56.41f, 54.43f, 53.42f, 52.30f, 51.25f, 50.23f, 49.33f, 48.42f }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 63.51f, 60.43f, 79.63f, 56.91f, 55.95f, 53.86f, 51.93f, 49.90f, 49.00f, 47.96f, 46.92f, 46.02f, 45.11f }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 62.54f, 59.44f, 56.91f, 77.15f, 53.83f, 51.64f, 49.71f, 47.73f, 45.71f, 44.90f, 43.87f, 42.97f, 42.05f }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 61.57f, 58.49f, 55.95f, 53.83f, 74.66f, 49.82f, 47.82f, 45.83f, 43.85f, 41.86f, 41.06f, 40.16f, 39.24f }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 59.45f, 56.41f, 53.86f, 51.64f, 49.82f, 71.67f, 46.07f, 44.07f, 42.10f, 40.14f, 38.09f, 37.43f, 36.52f }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 58.37f, 54.43f, 51.93f, 49.71f, 47.82f, 46.07f, 68.72f, 42.69f, 40.70f, 38.74f, 36.71f, 34.75f, 34.09f }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 57.17f, 53.42f, 49.90f, 47.73f, 45.83f, 44.07f, 42.69f, 66.22f, 39.65f, 37.67f, 35.66f, 33.72f, 31.71f }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 55.87f, 52.30f, 49.00f, 45.71f, 43.85f, 42.10f, 40.70f, 39.65f, 62.70f, 37.01f, 35.00f, 33.07f, 31.08f }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 55.74f, 51.25f, 47.96f, 44.90f, 41.86f, 40.14f, 38.74f, 37.67f, 37.01f, 59.64f, 35.07f, 33.16f, 31.19f }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 54.73f, 50.23f, 46.92f, 43.87f, 41.06f, 38.09f, 36.71f, 35.66f, 35.00f, 35.07f, 56.26f, 32.07f, 30.12f }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 53.86f, 49.33f, 46.02f, 42.97f, 40.16f, 37.43f, 34.75f, 33.72f, 33.07f, 33.16f, 32.07f, 52.84f, 29.24f }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 52.95f, 48.42f, 45.11f, 42.05f, 39.24f, 36.52f, 34.09f, 31.71f, 31.08f, 31.19f, 30.12f, 29.24f, 49.39f }  // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };
 
        internal float EvaluateHand(Card firstCard, Card secondCard)
        {
            float handStrengthIndex = firstCard.Suit == secondCard.Suit
                          ? (firstCard.Type > secondCard.Type
                                 ? StartingHandWinningProbabilities[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
                                 : StartingHandWinningProbabilities[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
                          : (firstCard.Type > secondCard.Type
                                 ? StartingHandWinningProbabilities[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
                                 : StartingHandWinningProbabilities[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

            float totalProbabillity = handStrengthIndex / 100f;

            /// "Flush" probability
            if (firstCard.Suit == secondCard.Suit)
            {
                // adding ~2.5% for same color
                totalProbabillity += 0.025f;
            }

            return totalProbabillity;
        }
    }
}