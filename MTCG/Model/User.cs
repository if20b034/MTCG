using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        private const int StartingMoney = 20;

        private const int StartingElo = 100;
        //TODO: Credentials
        private string Email { get; set; }
        private string Pass { get; set;  }

        private Guid Session { get; set; }

        private int Coins { get; set; }

        public int ELO { get; set; }

        public List<ICard> Collection { get; set; }
        public List<ICard> Deck { get; set; }

        public User()
        {
            Collection = new();
            Deck = new();
            Coins = StartingMoney;
            ELO = StartingElo; //TODO: Remove from contrs
        }

        public bool buyPack(Package package)
        {
            if (Coins < 5) return false;
            Collection.AddRange(package.Cards);
            Coins = Coins - 5;
            return true;
        }
        //TODO: add only if in Collection (Remove from colleciton) etc..
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
