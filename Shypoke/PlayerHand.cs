using System.Collections.Generic;

namespace Shypoke
{
    public class PlayerHand : List<Card>
    {
        public int handRankScore { get; set; }
        public int handRankHighCardScore { get; set; }
        
        public PlayerHand() : base()
        {
            handRankScore = 0;
            handRankHighCardScore = 0;
        }

        public PlayerHand(IEnumerable<Card> targets)
            : base()
        {

        }

        /*public PlayerHand(IEnumerable<Card> targetCollection)
            : base(targetCollection)
        {
            handRankScore = 0;
        }*/
    }
}
