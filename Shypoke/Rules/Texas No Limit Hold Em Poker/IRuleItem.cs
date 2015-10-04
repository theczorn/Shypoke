namespace Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker
{
    public interface IRuleItem
    {
        PlayerHand ExecuteRule(PlayerHand testHand);
    }
}
