using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Package
    {
        public ICard[] Cards { get;  private set; }
        public Guid id { get; set; }

        public Package()
        {
            Cards = new ICard[5];
        }

        public bool addCards(List<ICard> cards)
        {
            try
            {
                if (cards.Count != 5)
                    return false;
                Cards = cards.ToArray();
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
