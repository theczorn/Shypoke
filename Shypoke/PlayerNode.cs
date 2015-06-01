using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shypoke
{
    public class PlayerNode
    {
        public List<Card> playerHand { get; set; }
        public int playerMoney { get; set; }
        public bool isOutOfGame { get; set; }
        public bool hasFolded { get; set; }
        public int blindBetAmount { get; set; }
        public PlayerNode Left { get; set; }
        public int handScore { get; set; }

        public PlayerNode(int startingMoney)
        {
            playerMoney = startingMoney;
            playerHand = new List<Card>();
            isOutOfGame = false;
        }

        public int PlaceBet(int betPlaced)
        {
            if (betPlaced >= playerMoney + blindBetAmount)    //All In modifier
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
