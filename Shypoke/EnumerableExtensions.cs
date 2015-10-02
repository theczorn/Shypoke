using System.Linq;
using System.Collections.Generic;

namespace Shypoke.Extension_Library
{
    public static class EnumerableExtensions
    {
        public static PlayerHand ToPlayerHand(this IOrderedEnumerable<Card> targetCollection)
        {
            PlayerHand temp = new PlayerHand(targetCollection);
            foreach (Card c in targetCollection)
            {
                temp.Add(c);   
            }
            return temp;
        }

        public static PlayerHand ToPlayerHand(this IEnumerable<Card> targetCollection)
        {
            PlayerHand temp = new PlayerHand(targetCollection);
            foreach (Card c in targetCollection)
            {
                temp.Add(c);
            }
            return temp;
        }

         public static PlayerHand ToPlayerHand(this List<Card> targetCollection)
        {
            PlayerHand temp = new PlayerHand(targetCollection);
            foreach (Card c in targetCollection)
            {
                temp.Add(c);
            }
            return temp;
        }
    }
}
