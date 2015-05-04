using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shypoke
{
    static class HandAnalysis
    {
        private static int handScore = 0;  //Default highcard

        //returns numeric value sum of maximal hand + type score
        public static int AnalyzeHand(List<Card> testHand){
            testHand.Sort();    //hands are analyzed with an assumed order of low-to-high   //CZTOOD: ENSURE SORT ORDER IS CORRECT

            if (IsStraightFlush(testHand))
                return handScore += 256;
            else if (IsFour(testHand))
                return handScore += 128;
            else if (IsFullHouse(testHand))     //High Group rank of 3, Low group rank of 2
                return handScore += 64;
            else if (IsFlush(testHand))         //Any 5 cards of same suite
                return handScore += 32;
            else if (IsStraight(testHand))      //Any 5 of cards in a row by rank
                return handScore += 16;
            else if (IsThree(testHand))
                return handScore += 8;
            else if (IsTwoPair(testHand))
                return handScore += 4;
            else if (IsPair(testHand))
                return handScore += 2;
            else
                return handScore = testHand.GetRange(2, 5).Sum(x => x.cardPointValue);
        }

        private static bool IsStraightFlush(List<Card> testHand) //CZTODO: Calc score
        {
            //CZTODO: Optimize, Impacted by Ace Low
            testHand = testHand.Distinct().ToList();
            //Find straight first, then check for flush

            if(testHand.GroupBy(x => x.cardSuiteName)
                .Where(x => x.Count() >= 5)
                .Where(x => IsStraight(x.ToList()) == true)
                .ToList().Count() > 0)
            {
                return true;
            }

            return false;
        }

        private static bool IsFour(List<Card> testHand)  //CZTODO: Calc score
        {
            handScore = 0;
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 4).Count() > 0;
        }

        private static bool IsFullHouse(List<Card> testHand) //CZTODO: Calc Score
        {
            handScore = 0;
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

        private static bool IsStraight(List<Card> testHand)  //CZTODO: Calc score
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

        private static bool IsThree(List<Card> testHand) //CZTODO: Calc score
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 3).Count() > 0;
        }

        private static bool IsTwoPair(List<Card> testHand)   //CZTODO: Calc score
        {
            return testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 2).Count() >= 2;
        }

        private static bool IsPair(List<Card> testHand)
        {
            List<Card> highestPair = testHand.GroupBy(x => x.cardPointValue)
                     .Last(x => x.Count() == 2).Select(x => x).ToList();

            if (highestPair.Count < 1) return false;

            testHand = testHand.Except(highestPair).ToList();   //drop our pair to safely cleanse set
            testHand.RemoveRange(0,2);  //drop two lowest cards
            testHand.AddRange(highestPair);

            handScore = testHand.Sum(x => x.cardPointValue);

            return true;
        }
    }
}
