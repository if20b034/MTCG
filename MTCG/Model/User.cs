using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        public Guid id { get; set; }
        private const int StartingMoney = 20;

        private const int StartingElo = 100;
        public string Username { get; set; }
        public string Password { get; set;  }

        public Guid Session { get; set; }

        public int Coins { get; set; }

        public int ELO { get; set; }

        public string saltkey { get; set; }

        public List<ICard> Collection { get; set; }
        public List<ICard> Deck { get; set; }

        public User()
        {
            Collection = new();
            Deck = new();
            Coins = StartingMoney;
            ELO = StartingElo;
        }

        public bool buyPack(Package package)
        {
            if (Coins < 5) return false;
            Collection.AddRange(package.Cards);
            Coins = Coins - 5;
            return true;
        }
        public bool addCardToDeck(ICard card)
        {
            try
            {
                if (Deck.Count>=5)
                    return false;
                Deck.Add(card);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false; 
            }
        }

        public bool removeCardFromDeck(ICard card)
        {
            try
            {
                if (Deck.Count == 0)
                    return false;
                Deck.Remove(card);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false; 
            }
        }
    }
}
