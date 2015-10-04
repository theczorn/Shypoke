using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shypoke;
using Shypoke.Rules.Texas_No_Limit_Hold_Em_Poker;
using System.Collections.Generic;

namespace Shypoke_TDD
{
    [TestClass]
    public class HandAnalysis_Tests
    {
        public PlayerHand Hand = new PlayerHand();
        public PlayerHand Expected = new PlayerHand();

        public List<IRuleItem> ruleList = new List<IRuleItem>(){
                new StraightFlush_Rule()
                ,new FourOfAKind_Rule()
                ,new FullHouse_Rule()
                ,new Flush_Rule()
                ,new Straight_Rule()
                ,new ThreeOfAKind_Rule()
                ,new TwoPair_Rule()
                ,new Pair_Rule()
                ,new HighCard_Rule()
        };

        public int HandScore = 0;

        [TestMethod]
        public void DetectStraightFlush_Test()
        {
            Hand.Add(new Card("Hearts", 8));
            Hand.Add(new Card("Hearts", 9));
            Hand.Add(new Card("Hearts", 10));
            Hand.Add(new Card("Hearts", 11));
            Hand.Add(new Card("Hearts", 12));
            Hand.Add(new Card("Hearts", 13));
            Hand.Add(new Card("Hearts", 14));

            Expected.InsertRange(0, Hand.GetRange(2, 5));
            Hand = ruleList[0].ExecuteRule(Hand);


            Assert.AreEqual(14, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectFour_Test()
        {
            Hand.Add(new Card("Hearts", 2));
            Hand.Add(new Card("Spades", 2));
            Hand.Add(new Card("Hearts", 3));
            Hand.Add(new Card("Spades", 12));
            Hand.Add(new Card("Diamonds", 12));
            Hand.Add(new Card("Spades", 12));
            Hand.Add(new Card("Clubs", 12));

            Expected.InsertRange(0, Hand.GetRange(2, 5));
            Hand = ruleList[1].ExecuteRule(Hand);

            Assert.AreEqual(12, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectFullHouseTriples_Test()  //3,3,1
        {
            Hand.Add(new Card("Hearts", 2));
            Hand.Add(new Card("Hearts", 3));
            Hand.Add(new Card("Spades", 3));
            Hand.Add(new Card("Hearts", 3));
            Hand.Add(new Card("Diamonds", 4));
            Hand.Add(new Card("Spades", 4));
            Hand.Add(new Card("Clubs", 4));

            Expected.InsertRange(0, Hand.GetRange(2, 5));
            Hand = ruleList[2].ExecuteRule(Hand);

            Assert.AreEqual(4, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectFullHouseTwoPair_Test()  //2,2,3
        {
            Hand.Add(new Card("Hearts", 2));
            Hand.Add(new Card("Spades", 2));
            Hand.Add(new Card("Hearts", 3));
            Hand.Add(new Card("Spades", 3));
            Hand.Add(new Card("Diamonds", 4));
            Hand.Add(new Card("Spades", 4));
            Hand.Add(new Card("Clubs", 4));

            Expected.InsertRange(0, Hand.GetRange(2, 5));
            Hand = ruleList[2].ExecuteRule(Hand);

            Assert.AreEqual(4, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectFlush_Test()  
        {
            Hand.Add(new Card("Hearts", 2));
            Hand.Add(new Card("Hearts", 3));
            Hand.Add(new Card("Hearts", 4));
            Hand.Add(new Card("Hearts", 10));
            Hand.Add(new Card("Hearts", 11));
            Hand.Add(new Card("Hearts", 12));
            Hand.Add(new Card("Hearts", 13));

            Expected.InsertRange(0, Hand.GetRange(2, 5));
            Hand = ruleList[3].ExecuteRule(Hand);

            Assert.AreEqual(13, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectStraightLowPosition_Test()
        {
            Hand.Add(new Card("Diamonds", 2));
            Hand.Add(new Card("Hearts", 3));
            Hand.Add(new Card("Spades", 4));
            Hand.Add(new Card("Hearts", 5));
            Hand.Add(new Card("Spades", 6));
            Hand.Add(new Card("Hearts", 8));
            Hand.Add(new Card("Hearts", 9));

            Expected.InsertRange(0, Hand.GetRange(0,5));
            Hand = ruleList[4].ExecuteRule(Hand);

            Assert.AreEqual(6, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectStraightMidPosition_Test()
        {
            Hand.Add(new Card("Diamonds", 2));
            Hand.Add(new Card("Hearts", 4));
            Hand.Add(new Card("Spades", 5));
            Hand.Add(new Card("Hearts", 6));
            Hand.Add(new Card("Spades", 7));
            Hand.Add(new Card("Hearts", 8));
            Hand.Add(new Card("Hearts", 14));

            Expected.InsertRange(0, Hand.GetRange(1, 5));
            Hand = ruleList[4].ExecuteRule(Hand);

            Assert.AreEqual(8, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectStraightHighPosition_Test()
        {
            Hand.Add(new Card("Diamonds", 3));
            Hand.Add(new Card("Hearts", 3));
            Hand.Add(new Card("Spades", 4));
            Hand.Add(new Card("Hearts", 5));
            Hand.Add(new Card("Spades", 6));
            Hand.Add(new Card("Hearts", 7));
            Hand.Add(new Card("Hearts", 8));

            Expected.InsertRange(0, Hand.GetRange(2, 5));
            Hand = ruleList[4].ExecuteRule(Hand);

            Assert.AreEqual(8, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectStraightAceLow_Test()
        {
            Hand.Add(new Card("Hearts", 14));
            Hand.Add(new Card("Diamonds", 2));
            Hand.Add(new Card("Hearts", 3));
            Hand.Add(new Card("Spades", 4));
            Hand.Add(new Card("Hearts", 5));
            Hand.Add(new Card("Spades", 11));
            Hand.Add(new Card("Hearts", 12));

            Expected.InsertRange(0, Hand.GetRange(0, 5));
            Hand = ruleList[4].ExecuteRule(Hand);

            Assert.AreEqual(5, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectThree_Test()
        {
            Hand.Add(new Card("Hearts", 2));
            Hand.Add(new Card("Spades", 3));
            Hand.Add(new Card("Hearts", 4));
            Hand.Add(new Card("Hearts", 10));
            Hand.Add(new Card("Spades", 10));
            Hand.Add(new Card("Diamonds", 10));
            Hand.Add(new Card("Hearts", 13));

            Expected.InsertRange(0, Hand.GetRange(2, 5));
            Hand = ruleList[5].ExecuteRule(Hand);

            Assert.AreEqual(10, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectTwoPair_Test()   //2, 2, X
        {
            Hand.Add(new Card("Hearts", 2));
            Hand.Add(new Card("Spades", 2));
            Hand.Add(new Card("Hearts", 4));
            Hand.Add(new Card("Diamonds", 4));
            Hand.Add(new Card("Spades", 11));
            Hand.Add(new Card("Clubs", 12));
            Hand.Add(new Card("Hearts", 13));

            Expected.InsertRange(0, Hand.GetRange(0, 4));
            Expected.Insert(4, Hand[6]);
            Hand = ruleList[6].ExecuteRule(Hand);

            Assert.AreEqual(4, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectTwoPairInTriple_Test()   //2, 2, 2, 1
        {
            Hand.Add(new Card("Hearts", 2));
            Hand.Add(new Card("Spades", 2));
            Hand.Add(new Card("Hearts", 4));
            Hand.Add(new Card("Diamonds", 4));
            Hand.Add(new Card("Spades", 11));
            Hand.Add(new Card("Clubs", 11));
            Hand.Add(new Card("Hearts", 13));

            Expected.InsertRange(0, Hand.GetRange(2,5));
            Hand = ruleList[6].ExecuteRule(Hand);

            Assert.AreEqual(11, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectPair_Test()   
        {
            Hand.Add(new Card("Hearts", 2));
            Hand.Add(new Card("Spades", 2));
            Hand.Add(new Card("Hearts", 4));
            Hand.Add(new Card("Diamonds", 9));
            Hand.Add(new Card("Spades", 11));
            Hand.Add(new Card("Clubs", 12));
            Hand.Add(new Card("Hearts", 13));

            Expected.InsertRange(0, Hand.GetRange(0, 2));
            Expected.InsertRange(2, Hand.GetRange(4 ,3));
            Hand = ruleList[7].ExecuteRule(Hand);

            Assert.AreEqual(2, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

        [TestMethod]
        public void DetectHighCard_Test()   
        {
            Hand.Add(new Card("Hearts", 2));
            Hand.Add(new Card("Spades", 7));
            Hand.Add(new Card("Hearts", 8));
            Hand.Add(new Card("Hearts", 9));
            Hand.Add(new Card("Spades", 11));
            Hand.Add(new Card("Diamonds", 12));
            Hand.Add(new Card("Hearts", 14));

            Expected.InsertRange(0, Hand.GetRange(2, 5));
            Hand = ruleList[8].ExecuteRule(Hand);

            Assert.AreEqual(14, Hand.handRankHighCardScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }
    }
}
