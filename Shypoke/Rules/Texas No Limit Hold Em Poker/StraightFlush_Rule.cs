using System.Linq;
using Shypoke.Extension_Library;

namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public class StraightFlush_Rule : IRuleItem
    {
        private IRuleItem FlushCheck = new Flush_Rule();
        private IRuleItem StraightCheck = new Straight_Rule();

        public PlayerHand ExecuteRule(PlayerHand testHand)
        {
            PlayerHand optimal = testHand;

            optimal = FlushCheck.ExecuteRule(optimal);
            //optimal = optimal.OrderBy(x => x.cardPointValue).ToPlayerHand();      //CZTODO: was scrubbing handScore vars, necessary?

            if (optimal.handRankScore == 500)
            {
                optimal = StraightCheck.ExecuteRule(optimal);

                if (optimal.handRankScore == 400)
                {
                    testHand = optimal;
                    testHand.handRankScore = 800;
                    testHand.handRankHighCardScore = testHand[4].cardPointValue;
                    return testHand;
                }
            }
            
            return testHand;
        }
    }
}
