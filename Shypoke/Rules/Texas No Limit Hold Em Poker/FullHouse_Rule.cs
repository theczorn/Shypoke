using System.Linq;
using Shypoke.Extension_Library;

namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public class FullHouse_Rule : IRuleItem
    {
        public PlayerHand ExecuteRule(PlayerHand testHand)
        {
            var testGroup = testHand.GroupBy(x => x.cardPointValue);

            if (testGroup.Count(x => x.Count() == 3) == 2)   //set of 3,3,1
            {
                testHand = testGroup.Where(x => x.Count() == 3)     //CZTODO: Remove ienum-->list-->playerhand conversion chain
                    .SelectMany(x => x).ToList<Card>()
                    .GetRange(1, 5).ToPlayerHand();

                testHand.handRankHighCardScore = testHand[4].cardPointValue;
                testHand.handRankScore = 600;

                return testHand;
            }
            else if (testGroup.Count(x => x.Count() == 3) == 1  //set of 2,2,3  //CZTODO: Clean Logic
                &&
                testGroup.Count(x => x.Count() == 2) == 2)
            {
                testHand = new PlayerHand();
                testHand.AddRange(                  //add triple
                    testGroup.Where(x => x.Count() == 3)
                   .SelectMany(x => x).ToList()
                );
                testHand.AddRange(                  //get high double
                    testGroup.Where(x => x.Count() == 2)
                    .SelectMany(x => x).ToList().GetRange(2, 2)
                );

                testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();
                testHand.handRankHighCardScore = testGroup.Where(x => x.Count() == 3).SelectMany(x => x).First().cardPointValue;
                testHand.handRankScore = 600;

                return testHand;
            }
            return testHand;
        }
    }
}
