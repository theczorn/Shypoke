using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shypoke
{
    class PlayerNode
    {
        public List<Card> playerHand { get; set; }
        public int playerMoney { get; set; }
        public bool isOutOfGame { get; set; }
        public bool hasFolded { get; set; }
        public int moneyBetInRound { get; set; }
        public PlayerNode Left { get; set; }
        public int maxHandValue { get; set; }

        public PlayerNode(int startingMoney)
        {
            playerMoney = startingMoney;
            playerHand = new List<Card>();
            isOutOfGame = false;
        }

        public int PlaceBet(int betPlaced)
        {
            if (betPlaced > playerMoney)
            {
                betPlaced = playerMoney;
                playerMoney = 0;
            }
            else
            {
                playerMoney -= betPlaced;
            }
            return betPlaced;
        }

        internal int RaiseBets(int raisePlaced)
        {
            return PlaceBet(raisePlaced);
        }
    }
}
