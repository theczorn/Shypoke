using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shypoke
{
    static class ValidationHelper
    {
        private static int playerCount;
        private static int startingPot;
        private static bool? parseResult;

        public static int ValidatePlayerCount(string rawPlayerCount)
        {

            parseResult = Int32.TryParse(rawPlayerCount, out playerCount);

            if (parseResult == null)
            {
                return playerCount = 0;
            }

            return playerCount;
        }

        public static int ValidateStartingPot(string rawStartingPot)
        {

            parseResult = Int32.TryParse(rawStartingPot, out startingPot);

            if (parseResult == null)
            {
                return startingPot = 0;
            }

            return startingPot;
        }
    }
}