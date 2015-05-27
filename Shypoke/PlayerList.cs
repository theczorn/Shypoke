using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shypoke
{
    class PlayerList : System.Collections.IEnumerable
    {
        public PlayerNode previous { get; set; }
        public PlayerNode root { get; set; }   //points to first item in collection
        private PlayerNode last { get; set; }
        public PlayerNode current { get; set; }
        public int ActivePlayers { get; set; }

        public PlayerList() { }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            current = root;
            do
            {
                yield return current;
                current = current.Left;     //NOTE: collection of players is to be iterated thru in a clockwise fashion, so navigate Left
            } while (current != root);
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
            ActivePlayers += 1;
        }

        public bool IsGameOver()
        {
            return ActivePlayers > 1? false:true;
        }

        public bool Remove(PlayerNode targetNode)
        {
            current = root;

            foreach (PlayerNode node in this)
            {
                if (node == targetNode)
                {
                    node.isOutOfGame = true;
                    ActivePlayers -= 1;
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

        public PlayerNode RetrieveWinner()
        {
            PlayerNode currentWinner = null;
            int bestHandScore = 0;

            foreach (PlayerNode target in this)
            {
                if (target.handScore == bestHandScore)
                {
                    //CZTODO: NEEDS TO HANDLE MULTIWAY TIES!!!
                    currentWinner = HandAnalysis.CompareEquivalentHands(currentWinner, target);
                }
                else if(target.handScore > bestHandScore){
                    currentWinner = target;
                    bestHandScore = previous.handScore;
                }
            }

            return currentWinner;
        }
    }
}
