using System.Collections.Generic;
using System.Linq;
using Shypoke.Extension_Library;

namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public class FourOfAKind_Rule : IRuleItem
    {
        public PlayerHand ExecuteRule(PlayerHand testHand)
        {
            List<Card> quadList = testHand.GroupBy(x => x.cardPointValue)
                .Where(x => x.Count() == 4)
                .SelectMany(x => x).ToList();

            if (!quadList.Any())
                return testHand;

            //CZTODO - Spin off into own subfunction
            testHand = testHand.Except(quadList).ToPlayerHand();  //drop our pair to safely cleanse set
            testHand.RemoveRange(0, 2);                     //drop until all we have is the high card
            testHand.AddRange(quadList);
            testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();
            testHand.handRankHighCardScore = quadList[0].cardPointValue;
            testHand.handRankScore = 700;

            return testHand;
        }
    }
}
