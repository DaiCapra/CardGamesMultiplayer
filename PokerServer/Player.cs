using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerServer
{
    public class Player
    {
        private int cash;
        private int bankCash;
        private List<Card> hand;

        public Player()
        {
            hand = new List<Card>();
        }

        public int Cash()
        {
            return cash;
        }
        public int BankCash()
        {
            return BankCash();
        }
    }
}
