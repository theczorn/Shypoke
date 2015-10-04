using System;
using System.Collections.Generic;
using System.Linq;
using Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker;

namespace Shypoke
{
    class Table
    {
        #region Variables
        private int startingMoney;
        private int currentMinimumBet;
        private int currentPotSize;

        private List<Card> deck;
        private List<Card> communityHand;
        private List<Card> burnDeck;
        private PlayerList activePlayers;
        private List<IRuleItem> rulesList;

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
            GenerateGameRulesStrategy();
        }

        /// <summary>
        /// By Default generates Texas No Hold 'Em Hand Analysis rules
        /// CZTODO: Build up on this later to implement future rulesets/variants
        /// </summary>
        private void GenerateGameRulesStrategy()
        {
            rulesList = new List<IRuleItem>()
            {
                new StraightFlush_Rule()
                ,new FourOfAKind_Rule()
                ,new FullHouse_Rule()
                ,new Flush_Rule()
                ,new Straight_Rule()
                ,new ThreeOfAKind_Rule()
                ,new TwoPair_Rule()
                ,new Pair_Rule()
                ,new HighCard_Rule()
            };
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
                activePlayers.Add(new PlayerNode("player"+(i+1), startingMoney));
            }

            //establish starting roles
            activePlayers.dealer = activePlayers.Find(rand.Next(0, numberOfPlayersInGame));
            if (activePlayers.Count == 2)
            {
                activePlayers.smallBlind = activePlayers.dealer;
                activePlayers.bigBlind = activePlayers.smallBlind.Left;
            }
            else
            {
                activePlayers.smallBlind = activePlayers.dealer.Left;
                activePlayers.bigBlind = activePlayers.smallBlind.Left;
            }

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

                    //Reverse ordering if heads up              //CZTODO: verify if this needs to occur here OR after flop/before flop betcycle
                    if (activePlayers.Count == 2)
                        activePlayers.SwapBetPriorityForHeadsUp();

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

        private void CleanupTableForNextRound()
        {
            activePlayers.CleanupPlayerList();

            this.communityHand.Clear();
            
            Shuffle(burnDeck);          //re-shuffle burn deck back into main deck (don't want to run out of cards)
            deck.AddRange(burnDeck);
            burnDeck.Clear();

            currentPotSize = 0;
        }


        //CZTODO: This whole chain seems overly long, fix when it's not midnight #___#
        private void DeclareWinners()
        {
            List<PlayerNode> listOfWinners = activePlayers.RetrieveWinner();

            if (listOfWinners.Count > 1)
            {
                foreach(PlayerNode winner in listOfWinners)
                {
                    winner.playerMoney += Math.Abs(this.currentPotSize / listOfWinners.Count);  //CZTODO: losing cent fractions here. Acceptable?
                }
            }
            else
            {
                listOfWinners[0].playerMoney += this.currentPotSize;  //assign winnings
            }
        }

        private void CalculateOptimalHands()
        {
            PlayerHand tempOptimalHand = null;
            foreach (PlayerNode target in activePlayers.rootIterator())
            {
                if (target.hasFolded)
                    continue;

                target.playerHand.AddRange(communityHand);      //merge hands for 7 card set
                tempOptimalHand = target.playerHand;
                target.playerHand = HandAnalysis.AnalyzeHand(tempOptimalHand, rulesList);
            }
        }


        private void BetCycle()
        {
            if (activePlayers.Count - activePlayers.NumberOfAllinOrFoldedPlayers == 1)
                return; //no need to betcycle, expedite to end of round. One guy with chips left to play

            bool validAction;

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
                                activePlayers.NumberOfAllinOrFoldedPlayers++;
                                activePlayers.current.blindBetAmount = 0;
                                validAction = true;
                                break;
                            default:
                                Console.WriteLine("Invalid Command: Please enter a valid command.");
                                validAction = false;
                                break;
                        }
                    } while (validAction == false);

                    if (activePlayers.current.playerMoney == 0)
                        activePlayers.NumberOfAllinOrFoldedPlayers++;
                }
                activePlayers.current = activePlayers.current.Left;
            } while (activePlayers.current != initiator);   //break loop if we've gone around the table and everyone has made legal actions
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
                foreach (PlayerNode p in activePlayers.smallBlindIterator())
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
        private void Shuffle(List<Card> targetDeck)
        {
            Card tempCard;
            int swapIndex;

            for (int shuffleCount = 0; shuffleCount < 5; shuffleCount++)
            {
                for (int i = 0; i < targetDeck.Count; i++)
                {
                    swapIndex = rand.Next(0, targetDeck.Count);
                    tempCard = targetDeck[swapIndex];

                    targetDeck[swapIndex] = targetDeck[i];
                    targetDeck[i] = tempCard;
                }
            }
        }
        
        #endregion
    }
}