using System;
using System.Collections.Generic;
using System.Linq;

namespace Shypoke
{
    /// <summary>
    /// <CZTODO>
    ///     -Minimize usage of REFS
    ///     -optimize overall algo
    ///     -check Ace Low card straights
    /// </CZTODO>
    /// </summary>
    public static class HandAnalysis
    {
        public static void AnalyzeHand(ref List<Card> testHand, ref int handScore)
        {
            testHand = testHand.OrderBy(x => x.cardPointValue).ToList();    //hands are analyzed with an assumed order of low-to-high 

            if (IsStraightFlush(ref testHand))
            {
                handScore = 800;
                return;
            }
            else if (IsFour(ref testHand))
            {
                handScore = 700;
                return;
            }
            else if (IsFullHouse(ref testHand))
            {
                handScore = 600;
                return;
            }
            else if (IsFlush(ref testHand))
            {
                handScore = 500;
                return;
            }
            else if (IsStraight(ref testHand))
            {
                handScore = 400;
                return;
            }
            else if (IsThree(ref testHand))
            {
                handScore = 300;
                return;
            }
            else if (IsTwoPair(ref testHand))
            {
                handScore = 200;
                return;
            }
            else if (IsPair(ref testHand))
            {
                handScore = 100;
                return;
            }
            else if (IsHighCard(ref testHand))
            {
                handScore = 1;
                return;
            }
            else
                throw new Exception("Testhand has no valid ranking.");
        }

        private static bool IsStraightFlush(ref List<Card> testHand)
        {
            List<Card> optimal = testHand;

            if (IsFlush(ref optimal))
            {
                optimal = optimal.OrderBy(x => x.cardPointValue).ToList();
                if (IsStraight(ref optimal))
                {
                    testHand = optimal;
                    return true;
                }
            }

            return false;
        }

        private static bool IsFour(ref List<Card> testHand)
        {

            List<Card> pairList = testHand.GroupBy(x => x.cardPointValue)
                .Where(x => x.Count() == 4)
                .SelectMany(x => x).ToList();

            if (!pairList.Any())
                return false;

            testHand = testHand.Except(pairList).ToList();  //drop our pair to safely cleanse set
            testHand.RemoveRange(0, 2);                     //drop until all we have is the high card
            testHand.AddRange(pairList);

            return true;
        }

        private static bool IsFullHouse(ref List<Card> testHand)
        {
            var testGroup = testHand.GroupBy(x => x.cardPointValue).ToList();

            if (testGroup.Count(x => x.Count() == 3) == 2)   //set of 3,3,1
            {
                testHand = testGroup.Select(x => x)
                    .Where(x => x.Count() == 3)
                    .SelectMany(x => x)
                    .ToList().GetRange(1, 5);

                return true;
            }
            else if (testGroup.Count(x => x.Count() == 3) == 1  //set of 2,2,3
                &&
                testGroup.Count(x => x.Count() == 2) == 2)
            {
                testHand = testGroup.SelectMany(x => x)
                    .Skip(2)            //skip low pair
                    .Take(5).ToList();  //retrieve highest flush
                return true;
            }
            return false;
        }

        private static bool IsFlush(ref List<Card> testHand)
        {
            List <Card> optimal = new List<Card>();
            optimal = testHand.GroupBy(x => x.cardSuiteName)
                    .Where(x => x.Count() >= 5)
                    .SelectMany(x => x).ToList();

            if (!optimal.Any())
                return false;
            else
            {
                optimal.Reverse();     //needed, we don't know have many of the same suite we have, flip to get top 5
                testHand = optimal.GetRange(0, 5);
                return true;
            }
        }

        private static bool IsStraight(ref List<Card> testHand)
        {
            List<Card> optimal = testHand.Distinct().ToList();    //dupes skew analysis, remove

            if (optimal.Count() == 7
                && optimal[6].cardPointValue - optimal[2].cardPointValue == 4)
            {
                testHand = optimal.GetRange(2, 5);
                return true;
            }
            else if (optimal.Count() >= 6
                && optimal[5].cardPointValue - optimal[1].cardPointValue == 4)
            {
                testHand = optimal.GetRange(1, 5);
                return true;
            }
            else if (optimal.Count() > 4
                && optimal[4].cardPointValue - optimal[0].cardPointValue == 4)
            {
                testHand = optimal.GetRange(0, 5);
                return true;
            }

            return false;
        }

        private static bool IsThree(ref List<Card> testHand)
        {
            List<Card> highTriple = testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 3)
                    .SelectMany(x => x).ToList();

            if (!highTriple.Any())
                return false;

            testHand = testHand.Except(highTriple).ToList();   //drop our triple to safely cleanse set
            testHand.RemoveRange(0, 2);         //preserve highest two
            testHand.AddRange(highTriple);

            return true;
        }

        private static bool IsTwoPair(ref List<Card> testHand)
        {
            List<Card> pairList = testHand.GroupBy(x => x.cardPointValue)
                .TakeWhile(x => x.Count() == 2)
                .SelectMany(x => x).ToList();

            if (pairList.Count() < 4)
                return false;
            else if (pairList.Count() > 4)
                pairList.RemoveRange(0, 2); //3 pair in List, remove first, lowest pair

            testHand = testHand.Except(pairList).ToList();   //drop our pair to safely cleanse set
            testHand.RemoveRange(0, 2);  //drop until all we have is the high card
            testHand.AddRange(pairList);

            return true;
        } 

        private static bool IsPair(ref List<Card> testHand)
        {
            List<Card> highestPair = testHand.GroupBy(x => x.cardPointValue)
                     .Where(x => x.Count() == 2)
                     .SelectMany(x => x).ToList();

            if (!highestPair.Any())
                return false;

            testHand = testHand.Except(highestPair).ToList();   //drop our pair to safely cleanse set
            testHand.RemoveRange(0,2);  //drop two lowest cards
            testHand.AddRange(highestPair);

            return true;
        }

        private static bool IsHighCard(ref List<Card> testHand)
        {
            testHand.RemoveRange(0,2);
            return true;
        }
    }
}