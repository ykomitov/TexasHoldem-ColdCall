namespace TexasHoldem.AI.ColdCallPlayer.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RandomGenerator
    {
        private static Random rand = new Random();

        public int GetRandomInteger(int min = int.MinValue, int max = int.MaxValue)
        {
            return rand.Next(min, max);
        }

        public double GetRandomDouble()
        {
            return rand.NextDouble();
        }

        public string GetRandomString(int minLen = 5, int maxLen = 10)
        {
            int stringLen = this.GetRandomInteger(minLen, maxLen);
            string word = string.Empty;

            for (int i = 0; i < stringLen; i++)
            {
                var asciiIndex = this.GetRandomInteger(97, 122);
                var letter = (char)asciiIndex;
                word += letter;
            }

            return word;
        }

        public DateTime GetRandomDate(DateTime minDate, DateTime maxDate)
        {
            TimeSpan timeDifference = maxDate - minDate;
            int daysDifference = timeDifference.Days;

            int randomDaysToAdd = this.GetRandomInteger(0, daysDifference);
            TimeSpan randomTimeToAdd = new TimeSpan(randomDaysToAdd, 0, 0, 0);

            DateTime randomDate = minDate + randomTimeToAdd;
            return randomDate;
        }
    }
}
