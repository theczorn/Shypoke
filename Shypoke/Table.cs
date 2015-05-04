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

        public PlayerNode bigBlind { get; set; }
        public PlayerNode smallBlind { get; set; }
        public PlayerNode dealer { get; set; }

        private Random rand = new Random();

        #endregion

        public Table(int playerCount, int startingMoney)
        {
            this.startingMoney = startingMoney;
            currentMinimumBet = (int) Math.Ceiling(startingMoney * .10);

            deck = new List<Card>(52);
            burnDeck = new List<Card>(52);
            communityHand = new List<Card>(5);

            PopulateDeck();

            activePlayers = new PlayerList();
            GeneratePlayers(startingMoney, playerCount);
        }

        private void PopulateDeck()
        {
            foreach (int value in Enumerable.Range(2, 14))
                deck.Add(new Card("Hearts", value));

            foreach (int value in Enumerable.Range(2, 14))
                deck.Add(new Card("Clubs", value));

            foreach (int value in Enumerable.Range(2, 14))
                deck.Add(new Card("Diamonds", value));

            foreach (int value in Enumerable.Range(2, 14))
                deck.Add(new Card("Spades", value));

            Shuffle(deck);
        }

        private void GeneratePlayers(int startingMoney, int playerCount)
        {
            for (int i = 0; i < playerCount; i++)
            {
                activePlayers.Add(new PlayerNode(startingMoney));
            }

            //establish starting roles
            dealer = activePlayers.Find(rand.Next(0, playerCount));
            bigBlind = dealer.Left;
            smallBlind = bigBlind.Left;
        }

        public void StartGame()
        {
            try
            {
                //CZTODO: Start with player left of Big Blind
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

                //CZTODO: Reveal Hands
                CalculateHandWinner();
            }
            catch (Exception ex)
            {
                //CZTODO: Handle overdraw here
            }
            finally{    //reset our player states
                foreach (PlayerNode target in activePlayers)
                {
                    target.hasFolded = false;
                }
            }
        }

        private void CalculateHandWinner()
        {
            List<Card> temp = new List<Card>();
            foreach (PlayerNode target in activePlayers)
            {
                //temp = target.playerHand.AddRange(communityHand);
            }
        }

        private void BetCycle()
        {
            bool validAction;
            PlayerNode initiator = smallBlind;
            activePlayers.current = smallBlind;

            Console.Clear();

            do{
                validAction = false;
                if (!activePlayers.current.hasFolded)
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
                            case "f":   //CZTODO: Break Loop if only 1 player left
                                activePlayers.current.hasFolded = true;
                                validAction = true;
                                break;
                            default:
                                Console.WriteLine("Invalid Command: Please enter a valid command.");
                                break;
                        }
                    } while (validAction == false);
                }
                activePlayers.current = activePlayers.current.Left;
            } while (activePlayers.current != initiator);
        }

        private void CommitBlinds()
        {
            smallBlind.PlaceBet((int)currentMinimumBet * (1 / 2));
            bigBlind.PlaceBet(currentMinimumBet);
        }

        #region Deck Manipulations

        private void DealHoleCards()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (PlayerNode p in activePlayers) //CZTODO: should normally start with small blind...
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