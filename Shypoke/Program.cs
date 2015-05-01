using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shypoke
{
    class Program
    {
        static void Main(string[] args)
        {
            int playerCount = 0;
            int startingMoney = 0;
            Console.WriteLine("Welcome to Poker Player");

            while (playerCount == 0 && startingMoney == 0)
            {
                Console.WriteLine("Enter Number of players: ");
                string rawPlayerCount = Console.ReadLine();

                Console.WriteLine("Enter starting pot size: ");
                string rawStartingMoney = Console.ReadLine();

                playerCount = ValidationHelper.ValidatePlayerCount(rawPlayerCount);
                startingMoney = ValidationHelper.ValidateStartingPot(rawStartingMoney);
            }

            Table currentTable = new Table(playerCount, startingMoney);
            currentTable.StartGame();    
        }
    }
}