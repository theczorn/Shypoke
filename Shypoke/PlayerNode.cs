namespace Shypoke
{
    public class PlayerNode
    {
        public PlayerHand playerHand { get; set; }
        public int playerMoney { get; set; }
        public bool isOutOfGame { get; set; }
        public bool hasFolded { get; set; }
        public int blindBetAmount { get; set; }
        public PlayerNode Left { get; set; }
        public string playerName { get; set; }

        public PlayerNode(string name, int startingMoney)
        {
            playerMoney = startingMoney;
            playerName = name;
            playerHand = new PlayerHand();
            isOutOfGame = false;
        }

        public int PlaceBet(int betPlaced)
        {
            if (betPlaced >= playerMoney + blindBetAmount)  //Go All In if betting GTE to player money balance
            {
                betPlaced = playerMoney;
                playerMoney = 0;
            }
            else
            {
                playerMoney -= betPlaced-blindBetAmount;
            }
            this.blindBetAmount = 0;
            return betPlaced;
        }

        public int RaiseBets(int raisePlaced)
        {
            return PlaceBet(raisePlaced);
        }
    }
}
