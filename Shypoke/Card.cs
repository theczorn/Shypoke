using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shypoke
{
    enum Hearts
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    };
    enum Clubs
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    };
    enum Diamonds
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    };
    enum Spades
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    };

    class Card
    {
        public int cardPointValue { get; set; }
        private string cardSuiteName;
        private string cardValueName;

        private string[] SuiteNameList = Enum.GetNames(typeof(Hearts));
        public Card(String Suite, int value)
        {
            this.cardSuiteName = Suite;
            this.cardPointValue = value;
            this.cardValueName = SuiteNameList[value - 2];
        }
    }
}