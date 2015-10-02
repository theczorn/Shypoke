using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shypoke
{
    class PlayerList
    {
        public PlayerNode root { get; set; }    //for constructor
        private PlayerNode last { get; set; }   //for constructor
        public PlayerNode current { get; set; }

        public PlayerNode bigBlind { get; set; }
        public PlayerNode smallBlind { get; set; }
        public PlayerNode dealer { get; set; }

        public int Count { get; set; }
        private int deltaCount = 0;
        public int NumberOfAllinOrFoldedPlayers { get; set; }
        private bool wasDealerDeleted = false;
        private bool enteringHeadsUpState = false;

        public PlayerList() { }

        public IEnumerable<PlayerNode> smallBlindIterator()     //iterator for dealing cards, etc. starting on Small Blind Role
        {
            current = smallBlind;
            do
            {
                yield return current;
                current = current.Left;
            } while (current != smallBlind);
        }

        public IEnumerable<PlayerNode> rootIterator()           //iterator for playerlist cleanup and organization, begins at Root of PlayerList
        {
            current = root;
            Console.Clear();
            for(int i = 0; i < this.Count; i++)
            {
                Console.WriteLine(current.playerName);
                yield return current;
                current = current.Left;
            }
        }

        public void Add(PlayerNode newNode)
        {  
            if (root == null)
            {
                root = newNode;
                last = newNode;
            }
            else
            {
                last.Left = newNode;
                last = newNode;
                last.Left = root; //ensure list is circular once we have 2+ players
            }
            Count += 1;
        }

        public bool IsGameOver()
        {
            return Count > 1? false:true;
        }

        public bool Remove(PlayerNode targetNode)   //CZTODO: optimize this with Find(int)...
        {
            foreach (PlayerNode node in this.rootIterator())
            {
                if (node.Left == targetNode)
                {
                    node.Left.isOutOfGame = true;
                    deltaCount += 1;

                    if (Count-deltaCount == 2)
                        enteringHeadsUpState = true;

                    if (node.Left == dealer)    //target node is dealer, shift this role to preserve ordering of blind roles
                    {                    
                        dealer = node.Left.Left;
                        wasDealerDeleted = true;
                    }

                    if (node.Left == root)
                        root = root.Left;

                    PlayerNode newNeighbor = node.Left.Left;
                    node.Left.Left = null;
                    node.Left = newNeighbor;

                    return true;
                }
            }

            throw new KeyNotFoundException("Target node doesn't exist at this table");
        }

         public PlayerNode Find(int randomNumber)
         {
             current = root;
             for (int i = 0; i < randomNumber - 1; i++)
             {
                 current = current.Left;
             }
             return current;
         }

        public List<PlayerNode> RetrieveWinner()
        {
            List<PlayerNode> currentWinner = new List<PlayerNode>();
            int bestHandScore = 0;
            int bestHandRankHighCardScore = 0;

            foreach (PlayerNode target in this.rootIterator())
            {
                if (target.hasFolded)
                    continue;

                if (target.playerHand.handRankScore > bestHandScore         //hand rank is better
                    || (target.playerHand.handRankScore == bestHandScore    //OR hand ranks are equivalent, but one has a better "set"
                        && target.playerHand.handRankHighCardScore == bestHandRankHighCardScore)
                    ){
                    currentWinner.Clear();
                    currentWinner.Add(target);
                    bestHandScore = target.playerHand.handRankScore;
                    bestHandRankHighCardScore = target.playerHand.handRankHighCardScore;
                }
                else if (target.playerHand.handRankScore == bestHandScore
                    && target.playerHand.handRankHighCardScore == bestHandRankHighCardScore) //players have same hand rank and top card, iterate to find kicker that is our winning player(s)
                {
                    currentWinner = HandAnalysis.CompareEquivalentHands(currentWinner, target);
                }
            }

            return currentWinner;
        }

        //CZTODO: logic bleeding everywhere, tidy this up
        public void EstablishRoles()
        {
            if (!wasDealerDeleted && !enteringHeadsUpState)
                //we haven't deleted our dealer, manually shift it
                dealer = dealer.Left;
            else if (wasDealerDeleted && enteringHeadsUpState)
            {
                //we've deleted a dealer out of 3 players, and shifted the dealer, assign roles
                smallBlind = dealer;
                bigBlind = dealer.Left;
            }
            else if (enteringHeadsUpState)
            {
                //non-dealer deleted, first time in headsup mode
                dealer = dealer.Left;
                smallBlind = dealer;
                bigBlind = dealer.Left;
            }
            wasDealerDeleted = false;
            enteringHeadsUpState = false;

            if (Count > 2) {
                smallBlind = dealer.Left;
                bigBlind = smallBlind.Left;
            }
            //When Count == 2 after the Flop we switch the betting priority ahead of time so there's no need to repeat ourselves here
        }

        public void SwapBetPriorityForHeadsUp()
        {
            bigBlind = dealer;
            smallBlind = dealer.Left;
        }

        public void CleanupPlayerList()
        {
            this.NumberOfAllinOrFoldedPlayers = 0;

            foreach (PlayerNode target in this.rootIterator())
            {
                if (target.playerMoney == 0)
                    this.Remove(target);
                else
                {
                    target.hasFolded = false;
                    target.playerHand.Clear();
                }
            }
            this.Count -= deltaCount;
            deltaCount = 0;

            this.EstablishRoles();
        }
    }
}
