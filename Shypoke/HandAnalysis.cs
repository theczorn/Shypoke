using System;
using System.Collections.Generic;
using System.Linq;
using Shypoke.Extension_Library;

namespace Shypoke
{
    /// <summary>
    /// <CZTODO>
    ///     -optimize overall algo/minimize REFS
    ///     -order once and then done (LINQ's IOrderedEnumerable?)
    ///     -refactor AnalyzeHand with a Strategy Pattern
    /// </CZTODO>
    /// </summary>
    public static class HandAnalysis
    {
        private static bool handContainsAce = false;

        public static PlayerHand AnalyzeHand(PlayerHand testHand)
        {
            testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();    //hands are analyzed with an assumed order of low-to-high 
            handContainsAce = testHand.Exists(x => x.cardPointValue == 14);

            if (IsStraightFlush(ref testHand))
            {
                testHand.handRankScore = 800;
                testHand.handRankHighCardScore = testHand[4].cardPointValue;
            }
            else if (IsFour(ref testHand))
            {
                testHand.handRankScore = 700;
            }
            else if (IsFullHouse(ref testHand))
            {
                testHand.handRankScore = 600;
            }
            else if (IsFlush(ref testHand))
            {
                testHand.handRankScore = 500;
                testHand.handRankHighCardScore = testHand[4].cardPointValue;
            }
            else if (IsStraight(ref testHand))
            {
                testHand.handRankScore = 400;
                testHand.handRankHighCardScore = testHand[4].cardPointValue;
            }
            else if (IsThree(ref testHand))
            {
                testHand.handRankScore = 300;
            }
            else if (IsTwoPair(ref testHand))
            {
                testHand.handRankScore = 200;
            }
            else if (IsPair(ref testHand))
            {
                testHand.handRankScore = 100;
            }
            else if (IsHighCard(ref testHand))
            {
                testHand.handRankScore = 1;
                testHand.handRankHighCardScore = testHand[4].cardPointValue;
            }
            else
                throw new Exception("User's hand has no valid ranking.");

            return testHand;
        }

        private static bool IsStraightFlush(ref PlayerHand testHand)
        {
            PlayerHand optimal = testHand;

            if (IsFlush(ref optimal))
            {
                optimal = optimal.OrderBy(x => x.cardPointValue).ToPlayerHand();
                if (IsStraight(ref optimal))
                {
                    testHand = optimal;
                    return true;
                }
            }

            return false;
        }

        private static bool IsFour(ref PlayerHand testHand)
        {
            List<Card> quadList = testHand.GroupBy(x => x.cardPointValue)
                .Where(x => x.Count() == 4)
                .SelectMany(x => x).ToList();

            if (!quadList.Any())
                return false;

            testHand = testHand.Except(quadList).ToPlayerHand();  //drop our pair to safely cleanse set
            testHand.RemoveRange(0, 2);                     //drop until all we have is the high card
            testHand.AddRange(quadList);
            testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();
            testHand.handRankHighCardScore = quadList[0].cardPointValue;

            return true;
        }

        private static bool IsFullHouse(ref PlayerHand testHand)
        {
            var testGroup = testHand.GroupBy(x => x.cardPointValue);

            if (testGroup.Count(x => x.Count() == 3) == 2)   //set of 3,3,1
            {
                testHand = testGroup.Where(x => x.Count() == 3)     //CZTODO: Remove ienum-->list-->playerhand conversion chain
                    .SelectMany(x => x).ToList<Card>()
                    .GetRange(1, 5).ToPlayerHand();

                testHand.handRankHighCardScore = testHand[4].cardPointValue;

                return true;
            }
            else if (testGroup.Count(x => x.Count() == 3) == 1  //set of 2,2,3  //CZTODO: Clean Logic
                &&
                testGroup.Count(x => x.Count() == 2) == 2)
            {
                testHand = new PlayerHand();
                testHand.AddRange(                  //add triple
                    testGroup.Where(x => x.Count() == 3)
                   .SelectMany(x => x).ToList()
                );
                testHand.AddRange(                  //get high double
                    testGroup.Where(x => x.Count() == 2)
                    .SelectMany(x => x).ToList().GetRange(2,2)
                );

                testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();
                testHand.handRankHighCardScore = testGroup.Where(x => x.Count() == 3).SelectMany(x => x).First().cardPointValue;

                return true;
            }
            return false;
        }

        private static bool IsFlush(ref PlayerHand testHand)
        {
            PlayerHand optimal = new PlayerHand();
            optimal = testHand.GroupBy(x => x.cardSuiteName)
                    .Where(x => x.Count() >= 5)
                    .SelectMany(x => x).ToPlayerHand();

            if (!optimal.Any())
                return false;
            else
            {
                optimal.Reverse();     //needed, we don't know have many of the same suite we have, flip to get top 5
                testHand = optimal.GetRange(0, 5).OrderBy(x => x.cardPointValue).ToPlayerHand();
                return true;
            }
        }

        private static bool IsStraight(ref PlayerHand testHand)
        {
            //CZTODO: optimize
            PlayerHand optimal = testHand.GroupBy(x => x.cardPointValue).Select(grp => grp.First()).ToPlayerHand();  //dupes skew analysis, remove

            if (optimal.Count() == 7
                && optimal[6].cardPointValue - optimal[2].cardPointValue == 4)
            {
                testHand = optimal.GetRange(2, 5).ToPlayerHand();
                return true;
            }
            else if (optimal.Count() >= 6
                && optimal[5].cardPointValue - optimal[1].cardPointValue == 4)
            {
                testHand = optimal.GetRange(1, 5).ToPlayerHand();
                return true;
            }
            else if (optimal.Count() > 4
                && optimal[4].cardPointValue - optimal[0].cardPointValue == 4)
            {
                testHand = optimal.GetRange(0, 5).ToPlayerHand();
                return true;
            }
            else if(optimal.Count() > 4
                && optimal[3].cardPointValue - optimal[0].cardPointValue == 3
                && handContainsAce)
            {
                testHand = optimal.GetRange(0, 4).ToPlayerHand();
                testHand.Insert(0, optimal.Last());
                return true;
            }

            return false;
        }

        private static bool IsThree(ref PlayerHand testHand) {
            List<Card> highTriple = testHand.GroupBy(x => x.cardPointValue)
                    .Where(x => x.Count() == 3)
                    .SelectMany(x => x).ToList();

            if (!highTriple.Any())
                return false;

            testHand = testHand.Except(highTriple).ToPlayerHand();   //drop our triple to safely cleanse set
            testHand.RemoveRange(0, 2);         //preserve highest two
            testHand.AddRange(highTriple);
            testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();

            testHand.handRankHighCardScore = highTriple[0].cardPointValue;

            return true;
        }

        private static bool IsTwoPair(ref PlayerHand testHand)
        {
            List<Card> pairsList = testHand.GroupBy(x => x.cardPointValue)
                .Where(x => x.Count() == 2)
                .SelectMany(x => x).ToList();

            if (pairsList.Count() < 4)
                return false;
            else if (pairsList.Count() > 4)
                pairsList.RemoveRange(0, 2); //3 pair in List, remove first, lowest pair

            testHand = testHand.Except(pairsList).ToPlayerHand();   //drop our pair to safely cleanse set
            testHand.RemoveRange(0, 2);                             //filter for high card
            testHand.AddRange(pairsList);
            testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();

            testHand.handRankHighCardScore = pairsList[3].cardPointValue;   //value our rank by the highest pair

            return true;
        } 

        private static bool IsPair(ref PlayerHand testHand)
        {
            List<Card> highestPair = testHand.GroupBy(x => x.cardPointValue)
                     .Where(x => x.Count() == 2)
                     .SelectMany(x => x).ToList();

            if (!highestPair.Any())
                return false;

            testHand = testHand.Except(highestPair).ToPlayerHand();   //drop our pair to safely cleanse set
            testHand.RemoveRange(0,2);                          //drop two lowest cards
            testHand.AddRange(highestPair);
            testHand = testHand.OrderBy(x => x.cardPointValue).ToPlayerHand();

            testHand.handRankHighCardScore = highestPair[0].cardPointValue;

            return true;
        }

        private static bool IsHighCard(ref PlayerHand testHand)
        {
            testHand.RemoveRange(0,2);
            return true;
        }

        //CZTODO: Logic too deep, re-org to reduce returns
        public static List<PlayerNode> CompareEquivalentHands(List<PlayerNode> currentWinnerList, PlayerNode target)
        {
            for (int i = 4; i > 0; i--)
            {
                if (currentWinnerList[0].playerHand[i].cardPointValue > target.playerHand[i].cardPointValue){
                    return currentWinnerList;
                }
                else if (currentWinnerList[0].playerHand[i].cardPointValue < target.playerHand[i].cardPointValue)
                {
                    currentWinnerList.Clear();
                    currentWinnerList.Add(target);
                    return currentWinnerList;
                }
            }
            currentWinnerList.Add(target);
            return currentWinnerList;
        }
    }
}