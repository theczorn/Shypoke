namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public class HighCard_Rule : IRuleItem
    {
        public PlayerHand ExecuteRule(PlayerHand testHand)
        {
            testHand.RemoveRange(0, 2);

            testHand.handRankScore = 1;
            testHand.handRankHighCardScore = testHand[4].cardPointValue;

            return testHand;
        }
    }
}
