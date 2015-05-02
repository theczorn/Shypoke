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

            testHand.Sort();    //hands are analyzed with an assumed order of low-to-high   //CZTOOD: ENSURE SORT ORDER IS CORRECT

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

        private static bool IsStraightFlush(List<Card> testHand)
        {
            //CZTODO: Impacted by Ace Low, is this logic sound???
            if(IsFlush(testHand) && IsStraight(testHand))
            {
                return true;
            }

            return false;
        }

        private static bool IsFour(List<Card> testHand)
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 4).Count() > 0;
        }

        private static bool IsFullHouse(List<Card> testHand)
        {
           //CZTODO - Fix Scoring due to 3x2 needed?
            return (testHand.GroupBy(x => x.cardPointValue)     //find set of 3
                     .Where(x => x.Count() >= 3).Count() > 0
                     &&
                     testHand.GroupBy(x => x.cardPointValue)    //find set of 2
                     .Where(x => x.Count() >= 2).Count() > 0
                     );
        }

        private static bool IsFlush(List<Card> testHand)
        {
            return testHand.GroupBy(x => x.cardSuiteName)
                    .Where(x => x.Count() == 5).Count() > 0;
        }

        private static bool IsStraight(List<Card> testHand)
        {
            //CZTODO: Optimize algorithm, impacted by Ace Low
            testHand = testHand.Distinct().ToList();    //dupes skew analysis, remove

            if (testHand.Count >= 5)
            {
                if (testHand[4].cardPointValue - testHand[0].cardPointValue == 4)
                    return true;
            }
            else   //kill processing immediately, nothing here
            {
                return false;
            }
            
            if(testHand.Count >= 6)
            {
                if (testHand[5].cardPointValue - testHand[1].cardPointValue == 4)
                    return true;
            }

            if (testHand.Count == 7)
            {
                if (testHand[6].cardPointValue - testHand[2].cardPointValue == 4)
                    return true;
            }

            return false;
        }

        private static bool IsThree(List<Card> testHand)
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 3).Count() > 0;
        }

        private static bool IsTwoPair(List<Card> testHand)
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 2).Count() >= 2;
        }

        private static bool IsPair(List<Card> testHand)
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 2).Count() > 0;
        }
    }
}
