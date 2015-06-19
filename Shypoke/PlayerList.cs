using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shypoke
{
    class PlayerList : System.Collections.IEnumerable
    {
        public PlayerNode root { get; set; }    //points to first item added in collection for constructor
        private PlayerNode last { get; set; }   //points to the last item added in collection for constructor
        public PlayerNode current { get; set; }

        public PlayerNode bigBlind { get; set; }
        public PlayerNode smallBlind { get; set; }
        public PlayerNode dealer { get; set; }

        public int Count { get; set; }
        public int NumberOfAllinOrFoldedPlayers { get; set; }
        private bool wasDealerDeleted = false;
        private bool enteringHeadsUpState = false;

        public PlayerList() { }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            current = smallBlind;           //any initial table iterations need to start at smallblind and rotate left (was root prior here an do/while...)
            do
            {
                yield return current;
                current = current.Left;      //NOTE: collection of players is to be iterated thru in a clockwise fashion, so navigate Left
            } while (current != smallBlind);
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
                last.Left = root; //ensure list is circular once we have 3+ players
            }
            Count += 1;
        }

        public bool IsGameOver()
        {
            return Count > 1? false:true;
        }

        public bool Remove(PlayerNode targetNode)
        {
            foreach (PlayerNode node in this)
            {
                if (node.Left == targetNode)
                {
                    node.Left.isOutOfGame = true;
                    Count -= 1;

                    if (Count == 2)
                        enteringHeadsUpState = true;

                    if (node.Left == dealer)    //target node is dealer, shift this role to preserve ordering of blind roles
                    {                    
                        dealer = node.Left.Left;
                        wasDealerDeleted = true;
                    }

                    PlayerNode newNeighbor = node.Left.Left;    //record item we want to redirect player order to
                    node.Left.Left = null;                      //decouple target tail
                    node.Left = newNeighbor;                    //redirect prior player to skip over deleted target

                    return true;
                }
            }

            throw new KeyNotFoundException("Target node doesn't exist in this table");
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

            foreach (PlayerNode target in this)
            {
                if (target.hasFolded)
                    continue;

                if (target.handScore == bestHandScore)
                {
                    currentWinner = HandAnalysis.CompareEquivalentHands(currentWinner, target);
                }
                else if(target.handScore > bestHandScore){
                    currentWinner.Clear();
                    currentWinner.Add(target);
                    bestHandScore = target.handScore;
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

            foreach (PlayerNode target in this)
            {
                target.hasFolded = false;
                target.handScore = 0;
                target.playerHand.Clear();

                if (target.playerMoney == 0)
                    this.Remove(target);
            }
            this.EstablishRoles();
        }
    }
}
