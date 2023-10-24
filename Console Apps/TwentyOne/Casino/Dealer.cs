using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Casino
{
    public class Dealer
    {
        public string Name { get; set; }
        public Deck Deck { get; set; } //does not inherit from deck class; dealer is not a type of deck
        public int Balance { get; set; }

        public void Deal(List<Card> Hand)
        {
            Hand.Add(Deck.Cards.First()); // add first card
            string card = string.Format(Deck.Cards.First().ToString() + "\n");
            Console.WriteLine(card);
            using (StreamWriter file = new StreamWriter(@"C:\Users\keato\Documents\GitHub\The-Tech-Academy-Basic-C-Sharp-Projects\basic_C#_programs\TwentyOne\Logs\log.txt", true))
            {
                file.WriteLine(DateTime.Now);
                file.WriteLine(card);
            }
            Deck.Cards.RemoveAt(0); //Remove first item from deck
        }
    }
}
