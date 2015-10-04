using System.Linq;
using Shypoke.Extension_Library;

namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public class Flush_Rule : IRuleItem
    {
        public PlayerHand ExecuteRule(PlayerHand testHand)
        {
            PlayerHand optimal = new PlayerHand();
            optimal = testHand.GroupBy(x => x.cardSuiteName)
                    .Where(x => x.Count() >= 5)
                    .SelectMany(x => x).ToPlayerHand();

            if(optimal.Any())
            {
                optimal.Reverse();     //needed, we don't know have many of the same suite we have, flip to get top 5
                testHand = optimal.GetRange(0, 5).OrderBy(x => x.cardPointValue).ToPlayerHand();
                testHand.handRankScore = 500;
                testHand.handRankHighCardScore = testHand[4].cardPointValue;
            }
            return testHand;
        }
    }
}
