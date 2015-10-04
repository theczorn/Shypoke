using System.Collections.Generic;
using System.Linq;
using Shypoke.Extension_Library;

namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public class Pair_Rule : IRuleItem
    {
        public PlayerHand ExecuteRule(PlayerHand testHand)
        {
            List<Card> highestPair = testHand.GroupBy(x => x.cardPointValue)
                     .Where(x => x.Count() == 2)
                     .SelectMany(x => x).ToList();

            if (highestPair.Any())
            {
                testHand = testHand.Except(highestPair).ToPlayerHand();   //drop our pair to safely cleanse set
                testHand.RemoveRange(0, 2);                          //drop two lowest cards
                testHand.AddRange(highestPair);
                testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();

                testHand.handRankHighCardScore = highestPair[0].cardPointValue;
                testHand.handRankScore = 100;
            }
            return testHand;
        }
    }
}
