using System.Collections.Generic;
using System.Linq;
using Shypoke.Extension_Library;

namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public class TwoPair_Rule : IRuleItem
    {
        public PlayerHand ExecuteRule(PlayerHand testHand)
        {
            List<Card> pairsList = testHand.GroupBy(x => x.cardPointValue)
                .Where(x => x.Count() == 2)
                .SelectMany(x => x).ToList();

            if (pairsList.Count() < 4)
                return testHand;
            else if (pairsList.Count() > 4)
                pairsList.RemoveRange(0, 2); //3 pair in List, remove first, lowest pair

            testHand = testHand.Except(pairsList).ToPlayerHand();   //drop our pair to safely cleanse set
            testHand.RemoveRange(0, 2);                             //filter for high card
            testHand.AddRange(pairsList);
            testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();

            testHand.handRankHighCardScore = pairsList[3].cardPointValue;   //value our rank by the highest pair
            testHand.handRankScore = 200;

            return testHand;
        }
    }
}
