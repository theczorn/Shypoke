using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shypoke
{
    class Table
    {
        #region Variables
        private int startingMoney;
        private int currentMinimumBet;
        private int currentPotSize;
        private bool isTwoPlayer;

        private List<Card> deck;
        private List<Card> communityHand;
        private List<Card> burnDeck;
        private PlayerList activePlayers;

        private Random rand = new Random();

        #endregion

        public Table(int numberOfPlayersInGame, int startingMoney)
        {
            this.startingMoney = startingMoney;
            currentMinimumBet = (int) Math.Abs(startingMoney * .10);

            deck = new List<Card>(52);
            burnDeck = new List<Card>(52);
            communityHand = new List<Card>(5);

            PopulateDeck();

            activePlayers = new PlayerList();
            GeneratePlayers(startingMoney, numberOfPlayersInGame);
        }

        private void PopulateDeck()
        {
            foreach (int value in Enumerable.Range(2, 13))
                deck.Add(new Card("Hearts", value));

            foreach (int value in Enumerable.Range(2, 13))
                deck.Add(new Card("Clubs", value));

            foreach (int value in Enumerable.Range(2, 13))
                deck.Add(new Card("Diamonds", value));

            foreach (int value in Enumerable.Range(2, 13))
                deck.Add(new Card("Spades", value));

            Shuffle(deck);
        }

        private void GeneratePlayers(int startingMoney, int numberOfPlayersInGame)
        {
            for (int i = 0; i < numberOfPlayersInGame; i++)
            {
                activePlayers.Add(new PlayerNode(startingMoney));
            }

            //establish starting roles
            activePlayers.dealer = activePlayers.Find(rand.Next(0, numberOfPlayersInGame));
            activePlayers.smallBlind = activePlayers.dealer.Left;
            activePlayers.bigBlind = activePlayers.smallBlind.Left;
        }

        public void StartGame()
        {
            while (activePlayers.IsGameOver() == false)
            {
                Console.WriteLine("Starting new round");
                Console.ReadLine();
                try
                {
                    CommitBlinds();

                    DealHoleCards();
                    BetCycle();

                    //Deal Flop Cards
                    BurnTopCard();
                    communityHand.Add(DealTopCard());
                    communityHand.Add(DealTopCard());
                    communityHand.Add(DealTopCard());
                    BetCycle();

                    //Deal Fourth Street
                    BurnTopCard();
                    communityHand.Add(DealTopCard());
                    BetCycle();

                    //Deal Fifth Street
                    BurnTopCard();
                    communityHand.Add(DealTopCard());
                    BetCycle();

                    CalculateOptimalHands();
                    DeclareWinners();
                }
                catch (Exception ex)
                {
                    //CZTODO: Log Misc Issues here
                }

                CleanupTableForNextRound();
            }
        }

        //CZTODO: Subfunction this
        private void CleanupTableForNextRound()
        {
            foreach (PlayerNode target in activePlayers)
            {
                target.hasFolded = false;
                target.handScore = 0;
                target.playerHand.Clear();

                if (target.playerMoney == 0)
                    activePlayers.Remove(target);
            }
            EstablishRoles();

            this.communityHand.Clear();
            
            Shuffle(burnDeck);          //re-shuffle burndeck back into main deck (don't want to run out of cards)
            deck.AddRange(burnDeck);
            burnDeck.Clear();

            currentPotSize = 0;
        }

        private void EstablishRoles()
        {
            if (activePlayers.Count > 2)
            {
                activePlayers.dealer = activePlayers.dealer.Left;
                activePlayers.smallBlind = activePlayers.smallBlind.Left;
                activePlayers.bigBlind = activePlayers.bigBlind.Left;
            }
            else   //CZTODO: Headsup Rules here
            {

            }
            
        }


        //CZTODO: This whole chain seems overly long, fix when it's not midnight #___#
        private void DeclareWinners()
        {
            List<PlayerNode> listOfWinners = activePlayers.RetrieveWinner();

            if (listOfWinners.Count > 1)
            {
                foreach(PlayerNode winner in listOfWinners)
                {
                    winner.playerMoney += Math.Abs(this.currentPotSize / listOfWinners.Count);  //CZTODO: losing salami slices...
                }
            }
            else
            {
                listOfWinners[0].playerMoney += this.currentPotSize;  //assign winnings
            }
        }

        private void CalculateOptimalHands()
        {
            int tempScore = 0;
            List<Card> tempOptimalHand = null;

            foreach (PlayerNode target in activePlayers)
            {
                if (target.hasFolded)
                    continue;

                target.playerHand.AddRange(communityHand);      //merge hands for 7 card set
                tempOptimalHand = target.playerHand;
                HandAnalysis.AnalyzeHand(ref tempOptimalHand, ref tempScore);

                target.playerHand = tempOptimalHand;
                target.handScore = tempScore;
            }
        }


        private void BetCycle()
        {
            bool validAction;
            int numberOfFoldedPlayers = 0;

            PlayerNode initiator = activePlayers.smallBlind;
            activePlayers.current = activePlayers.smallBlind;

            Console.Clear();

            do{
                if (activePlayers.current.hasFolded == false && activePlayers.current.playerMoney > 0) //skip players in betcycle who have folded/all-in
                {
                    do
                    {
                        Console.WriteLine("Enter R to raise, C to check, or F to fold.");
                        switch (Console.ReadLine().ToLower())
                        {
                            case "r":
                                Console.WriteLine("Enter amount to raise to: ");
                                currentMinimumBet = activePlayers.current.RaiseBets(Int32.Parse(Console.ReadLine()));
                                currentPotSize += currentMinimumBet;

                                initiator = activePlayers.current;      //set our bet loop end condition to the last player to raise, everyone must match/fold
                                validAction = true;
                                break;
                            case "c":
                                currentPotSize += activePlayers.current.PlaceBet(currentMinimumBet);
                                validAction = true;
                                break;
                            case "f":   
                                activePlayers.current.hasFolded = true;
                                numberOfFoldedPlayers++;
                                activePlayers.current.blindBetAmount = 0;
                                validAction = true;
                                break;
                            default:
                                Console.WriteLine("Invalid Command: Please enter a valid command.");
                                validAction = false;
                                break;
                        }
                    } while (validAction == false);
                }
                activePlayers.current = activePlayers.current.Left;
            } while (activePlayers.current != initiator && numberOfFoldedPlayers != activePlayers.Count-1);   //break loop if all others fold or every player has bet
        }

        private void CommitBlinds()
        {
            currentPotSize += activePlayers.smallBlind.PlaceBet(Math.Abs(currentMinimumBet * (1 / 2)));
            activePlayers.smallBlind.blindBetAmount += Math.Abs(currentMinimumBet * (1 / 2));

            currentPotSize += activePlayers.bigBlind.PlaceBet(currentMinimumBet);
            activePlayers.bigBlind.blindBetAmount += currentMinimumBet;
        }

        #region Deck Manipulations

        private void DealHoleCards()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (PlayerNode p in activePlayers)
                {
                    p.playerHand.Add(DealTopCard());
                }
            }   
        }

        private void BurnTopCard()
        {
            burnDeck.Add(deck[0]);
            deck.RemoveAt(0);
        }

        private Card DealTopCard() 
        {
            Card topCard = deck[0];
            burnDeck.Add(topCard);
            deck.RemoveAt(0);
            return topCard;
        }

        //Implements Fisher-Yates shuffle x5
        private void Shuffle(List<Card> deck)
        {
            Card tempCard;
            int swapIndex;

            for (int shuffleCount = 0; shuffleCount < 5; shuffleCount++)
            {
                for (int i = 0; i < deck.Count; i++)
                {
                    swapIndex = rand.Next(0, deck.Count);
                    tempCard = deck[swapIndex];

                    deck[swapIndex] = deck[i];
                    deck[i] = tempCard;
                }
            }
        }
        
        #endregion
    }
}