using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shypoke
{
    public static class HandAnalysis
    {
        private static int handScore = 0;  //Default highcard

        //returns numeric value sum of maximal hand + type score //CZTODO: HAND MATH WRONG!?
        public static int AnalyzeHand(List<Card> testHand){
            testHand.OrderBy(x => x.cardPointValue);    //hands are analyzed with an assumed order of low-to-high 

            //CZTODO: Validate Fixed score metric...
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

        private static bool IsFour(List<Card> testHand)
        {

            List<Card> pairList = testHand.GroupBy(x => x.cardPointValue)
                .Last(x => x.Count() == 4)
                .Select(x => x).ToList();

            if (pairList.Count() < 1)
                return false;

            testHand = testHand.Except(pairList).ToList();   //drop our pair to safely cleanse set
            testHand.RemoveRange(0, 2);  //drop until all we have is the high card
            testHand.AddRange(pairList);

            handScore = testHand.Sum(x => x.cardPointValue);

            return true;
        }

        private static bool IsFullHouse(List<Card> testHand) //CZTODO: Calc Score
        {
           //CZTODO - Fix Scoring due to 3x2 needed?
            var testGroup = testHand.GroupBy(x => x.cardPointValue);

            if (
                testGroup.Where(x => x.Count() >= 3).Count() > 0
                &&
                testGroup.Where(x => x.Count() >= 2).Count() > 0
               )
            {
                       
            }
            
            return false;
        }

        private static bool IsFlush(List<Card> testHand)
        {
            testHand = testHand.GroupBy(x => x.cardSuiteName)
                    .First(x => x.Count() >= 5).Select(x => x).ToList();

            testHand.Reverse();

            if (testHand.Count() < 1)
                return false;
            else
            {
                testHand.GetRange(0, 5);
                handScore = testHand.Sum(x => x.cardPointValue);
                return true;
            }
        }

        private static bool IsStraight(List<Card> testHand)
        {
            //CZTODO: Optimize algorithm, impacted by Ace Low
            testHand = testHand.Distinct().ToList();    //dupes skew analysis, remove

            try
            {
                if (testHand[4].cardPointValue - testHand[0].cardPointValue == 4) {
                    handScore = testHand.GetRange(0, 5).Sum(x => x.cardPointValue);
                    return true;
                }

                if (testHand[5].cardPointValue - testHand[1].cardPointValue == 4)
                {
                    handScore = testHand.GetRange(1, 6).Sum(x => x.cardPointValue);
                        return true;
                }

                if (testHand[6].cardPointValue - testHand[2].cardPointValue == 4)
                {
                    handScore = testHand.GetRange(2, 7).Sum(x => x.cardPointValue);
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(IndexOutOfRangeException))
                    throw;
            }

            return false;
        }

        private static bool IsThree(List<Card> testHand) 
        {
            List<Card> highTriple = testHand.GroupBy(x => x.cardPointValue)
                    .Last(x => x.Count() == 3).Select(x => x).ToList();

            if (highTriple.Count() < 1)
                return false;
            else if (highTriple.Count() > 1)
                highTriple.RemoveRange(0, 3);   //remove lower triple

            testHand = testHand.Except(highTriple).ToList();   //drop our pair to safely cleanse set
            testHand.RemoveRange(0, 5);  //preserve highest two
            testHand.AddRange(highTriple);

            handScore = testHand.Sum(x => x.cardPointValue);

            return true;
        }

        private static bool IsTwoPair(List<Card> testHand)
        {
            List<Card> pairList = testHand.GroupBy(x => x.cardPointValue)
                .TakeWhile(x => x.Count() == 2)
                .SelectMany(x => x).ToList();

            if (pairList.Count() < 4)
                return false;
            else if (pairList.Count() > 4)
                pairList.RemoveRange(0, 2); //3 pair in List, remove first, lowest pair

            testHand = testHand.Except(pairList).ToList();   //drop our pair to safely cleanse set
            testHand.RemoveRange(0, 3);  //drop until all we have is the high card
            testHand.AddRange(pairList);

            handScore = testHand.Sum(x => x.cardPointValue);

            return true;
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
