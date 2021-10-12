using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Model;

namespace MTCG
{
    class Battle
    {
        public bool fight(User p1, User p2)
        {
            List<ICard> deck1 =new List<ICard>(p1.Deck);
            List<ICard> deck2 = new List<ICard>(p2.Deck);

            do
            {
                //Segment 1 get the Top card of each player 
                ICard p1Card = deck1[0];
                ICard p2Card = deck2[0];

                //Segment 2 fight the Cards

                
                //Segment 3 Add Card to Winner deck


                //Segment 4 ...

                //Segment 5 winner
                if (deck1.Count< 0)
                    return false;
                if(deck2.Count < 0)
                    return true;

            } while (true);


        }
    }
}
