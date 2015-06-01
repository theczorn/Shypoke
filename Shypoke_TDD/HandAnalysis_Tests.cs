using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shypoke;
using System.Collections.Generic;
using System.Linq;

namespace Shypoke_TDD
{
    /// <summary>
    /// <CZTODO>
    ///     -Add in checks for expected hands as well as handscore
    /// </CZTODO>
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public List<Card> Hand = new List<Card>();
        public List<Card> Expected = new List<Card>(); 
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);
            

            Assert.AreEqual(800, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(700, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(600, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(600, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(500, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(400, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(400, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(400, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(300, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(200, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(200, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);

            Assert.AreEqual(100, HandScore);
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
            HandAnalysis.AnalyzeHand(ref Hand, ref HandScore);
            
            Assert.AreEqual(1, HandScore);
            CollectionAssert.AreEqual(Expected, Hand);
        }

    }
}
