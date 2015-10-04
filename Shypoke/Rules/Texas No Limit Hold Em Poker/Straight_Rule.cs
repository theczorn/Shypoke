using System.Linq;
using Shypoke.Extension_Library;

namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public class Straight_Rule : IRuleItem
    {
        public PlayerHand ExecuteRule(PlayerHand testHand)
        {
            //CZTODO: optimize
            bool handContainsAce = testHand.Exists(x => x.cardPointValue == 14);
            PlayerHand optimal = testHand.GroupBy(x => x.cardPointValue).Select(grp => grp.First()).OrderBy(x => x.cardPointValue).ToPlayerHand();  //dupes skew analysis, remove

            if (optimal.Count() == 7
                && optimal[6].cardPointValue - optimal[2].cardPointValue == 4)
            {
                testHand = optimal.GetRange(2, 5).ToPlayerHand();
            }
            else if (optimal.Count() >= 6
                && optimal[5].cardPointValue - optimal[1].cardPointValue == 4)
            {
                testHand = optimal.GetRange(1, 5).ToPlayerHand();
            }
            else if (optimal.Count() > 4
                && optimal[4].cardPointValue - optimal[0].cardPointValue == 4)
            {
                testHand = optimal.GetRange(0, 5).ToPlayerHand();
            }
            else if (optimal.Count() > 4
                && optimal[3].cardPointValue - optimal[0].cardPointValue == 3
                && handContainsAce)
            {
                testHand = optimal.GetRange(0, 4).ToPlayerHand();
                testHand.Insert(0, optimal.Last());
            }

            if (testHand.Count == 5)
            {
                testHand.handRankScore = 400;
                testHand.handRankHighCardScore = testHand[4].cardPointValue;
            }

            return testHand;
        }
    }
}
