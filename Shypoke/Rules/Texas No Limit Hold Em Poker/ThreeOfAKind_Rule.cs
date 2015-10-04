using System.Collections.Generic;
using System.Linq;
using Shypoke.Extension_Library;

namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public class ThreeOfAKind_Rule : IRuleItem
    {
        public PlayerHand ExecuteRule(PlayerHand testHand)
        {
            List<Card> highTriple = testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 3)
                    .SelectMany(x => x).ToList();

            if (highTriple.Any())
            {
                testHand = testHand.Except(highTriple).ToPlayerHand();   //drop our triple to safely cleanse set
                testHand.RemoveRange(0, 2);         //preserve highest two
                testHand.AddRange(highTriple);
                testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();

                testHand.handRankHighCardScore = highTriple[0].cardPointValue;
                testHand.handRankScore = 300;
            }
            return testHand;
        }
    }
}
