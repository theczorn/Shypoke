using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shypoke
{
    class PlayerList : System.Collections.IEnumerable
    {
        public PlayerNode root { get; set; }    //points to first item added in collection   //CZTODO: is this needed?
        private PlayerNode last { get; set; }   //points to the last item added in collection for growth
        public PlayerNode current { get; set; }

        public PlayerNode bigBlind { get; set; }
        public PlayerNode smallBlind { get; set; }
        public PlayerNode dealer { get; set; }

        public int Count { get; set; }

        public PlayerList() { }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            current = smallBlind;           //any inital table iterations need to start at smallblind and rotate left (was root prior here an do/while...)
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
                last.Left = root; //ensure list is circular once we have 2+ players
            }
            Count += 1;
        }

        public bool IsGameOver()
        {
            return Count > 1? false:true;
        }

        public bool Remove(PlayerNode targetNode)
        {
            current = root;

            foreach (PlayerNode node in this)
            {
                if (node.Left == targetNode)
                {
                    node.Left.isOutOfGame = true;
                    Count -= 1;

                    PlayerNode newNeighbor = node.Left.Left;
                    node.Left.Left = null;
                    node.Left = newNeighbor;
                    
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
    }
}
