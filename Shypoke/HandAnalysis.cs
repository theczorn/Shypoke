using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shypoke
{
    static class HandAnalysis
    {
        //returns numeric value sum of maximal hand + type score
        public static int AnalyzeHand(List<Card> testHand){
            int handScore = 0;  //Default highcard


            //CZTODO: FUCK ACES CAN BE LOW OR HIGH
            testHand = SortHand(testHand);  //hands are analyzed with an assumed order of low-to-high

            if (IsStraightFlush(testHand))
                handScore = 256;
            else if (IsFour(testHand))
                handScore = 128;
            else if (IsFullHouse(testHand))
                handScore = 64;
            else if (IsFlush(testHand))
                handScore = 32;
            else if (IsStraight(testHand))      //Any 5 of cards in a row
                handScore = 16;
            else if (IsThree(testHand))
                handScore = 8;
            else if (IsTwoPair(testHand))
                handScore = 4;
            else if (IsPair(testHand))
                handScore = 2;

            handScore += CalculateBestHand(testHand);   //find best 5 card hand out of seven

            return handScore;
        }

        private static bool IsStraight(List<Card> testHand)
        {
            //CZTODO: Optimize algorithm
            testHand = testHand.Distinct().ToList();    //dupes skew analysis, remove

            if (testHand.Count >= 5)
            {
                testHand.FindAll(); //CZTODO: Craft predicate to find the finite ranges (remember ace low)
            }

            return false;
        }

        private static bool IsTwoPair(List<Card> testHand)
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 2).Count() >= 2;
        }

        private static bool IsFour(List<Card> testHand)
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 4).Count() > 0;
        }

        private static bool IsThree(List<Card> testHand)
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 3).Count() > 0;
        }

        private static bool IsPair(List<Card> testHand)
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 2).Count() > 0;
        }
    }
}
