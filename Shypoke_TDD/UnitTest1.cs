using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shypoke;
using System.Collections.Generic;

namespace Shypoke_TDD
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void HighestPair_Test()
        {
            List<Card> multiPair = new List<Card>();
            multiPair.Add(new Card("Hearts", 2));
            multiPair.Add(new Card("Hearts", 2));
            multiPair.Add(new Card("Hearts", 4));
            multiPair.Add(new Card("Diamonds", 9));
            multiPair.Add(new Card("Spades", 11));
            multiPair.Add(new Card("Clubs", 12));
            multiPair.Add(new Card("Hearts", 13));

            Assert.AreEqual(42, HandAnalysis.AnalyzeHand(multiPair));
        }
    }
}
