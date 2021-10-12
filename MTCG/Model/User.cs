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
        public ICard[] Deck { get; set; }

        public User()
        {
            Collection = new();
            Deck = new ICard[5];
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

        public bool addCardToDeck(ICard card)
        {
            try
            {
                if (Deck.Length == 5)
                    return false;
                List<ICard> cards = new List<ICard>(Deck);
                cards.Add(card);
                Deck = cards.ToArray();
                return true;
            }
            catch (Exception e)
            {
                return false; 
            }
        }

        public bool removeCardFromDeck(ICard card)
        {
            try
            {
                if (Deck.Length == 0)
                    return false;
                List<ICard> cards = new List<ICard>(Deck);
                cards.Remove(card);
                Deck = cards.ToArray();
                return true;
            }
            catch (Exception e)
            {
                return false; 
            }
        }
    }
}
