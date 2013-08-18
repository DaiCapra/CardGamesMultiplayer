using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerServer
{
    class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            CreateDeck();

            int n = 100000;
            Console.WriteLine("Shuffling " + n + " times...");
            for (int i = 0; i < n; i++)
                Shuffle();

            PrintDeck();
           
            
            /*
            int size = cards.Count;
            for (int i = size - 5; i < size; i++)
            {
                Console.WriteLine("V: " + cards[i].Value + ", F: " + cards[i].Face);
            }
            */
        }

        private void CreateDeck()
        {
            cards = new List<Card>();
    
            for (int i = 2; i < 15; i++) //value
            {
                for (int j = 1; j < 5; j++) //face
                {
                    cards.Add(new Card(i, j));
                }
            }
        }
        public bool Shuffle()
        {
            Random r = new Random();
            List<Card> tempCards = new List<Card>();
            for (int i = 0; i < 52; i++)
            {
                int a = r.Next(0, cards.Count);
                tempCards.Add(cards[a]);

                cards[a] = cards[cards.Count-1];
                cards.RemoveAt(cards.Count-1);
            }

            cards = tempCards;

            return true;
        }
        private void PrintDeck()
        {
            Console.WriteLine("Showing current deck...");
            foreach (Card c in cards)
            {
                Console.WriteLine("V: " + c.Value + ", F: " + c.Suit);
            }
        }
    }
}
