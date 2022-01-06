using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Model;

namespace MTCG
{
    public class Battle
    {
        const double timestwo = 2;
        const double zero = 0;
        const double half = 0.5;
        private Dictionary<Key, double> BattleLogic = new();

        public Battle()
        {
            FillLogic();
        }
        //TODO: Thorw exception if decks not full
        public bool Fight(User p1, User p2)
        {
            List<ICard> deck1 =new List<ICard>(p1.Deck);
            List<ICard> deck2 = new List<ICard>(p2.Deck);
            int count = 0; 

            do
            {
                //Segment 1 get the Top card of each player 
                ICard p1Card = deck1[0];
                ICard p2Card = deck2[0];
                double multiplication1 = 1;
                double multiplication2 = 1;

                //Segment 2 fight the Cards
                if (p1Card.GetType() == typeof(Spell) || p2Card.GetType() == typeof(Spell)) 
                {
                    if (BattleLogic.TryGetValue(new Key()
                    {
                        Dimension1 = p1Card.ElementType.ToLower(),
                        Dimension2 = p2Card.ElementType.ToLower()
                    }, out var output))
                        multiplication1 *= output;
                    if (BattleLogic.TryGetValue(new Key()
                    {
                        Dimension1 = p2Card.ElementType.ToLower(),
                        Dimension2 = p1Card.ElementType.ToLower()
                    }, out output))
                        multiplication2 *= output;

                    //TODO: 

                    if (p1Card.GetType() == typeof(Spell) && p2Card.GetType() == typeof(Monster))
                    {
                        Monster monsterCard2 = (Monster)p2Card;
                       
                        if (BattleLogic.TryGetValue(new Key()
                        {
                            Dimension1 = monsterCard2.MonsterType.ToLower(),
                            Dimension2 = p1Card.ElementType.ToLower()
                        }, out output))
                            multiplication1 *= output;
                        if (BattleLogic.TryGetValue(new Key()
                        {
                            Dimension1 = p1Card.ElementType.ToLower(),
                            Dimension2 = monsterCard2.MonsterType.ToLower()
                        }, out output))
                            multiplication2 *= output;
                    }
                    else if (p1Card.GetType() == typeof(Monster) && p2Card.GetType() == typeof(Spell)){

                        Monster monsterCard1 = (Monster)p1Card;

                        if (BattleLogic.TryGetValue(new Key()
                        {
                            Dimension1 = p2Card.ElementType.ToLower(),
                            Dimension2 = monsterCard1.MonsterType.ToLower()
                        }, out output))
                            multiplication1 *= output;
                        if (BattleLogic.TryGetValue(new Key()
                        {
                            Dimension1 = monsterCard1.MonsterType.ToLower(),
                            Dimension2 = p2Card.ElementType.ToLower()
                        }, out output))
                            multiplication2 *= output;
                    }
                }
                else
                {
                    Monster monsterCard1 = (Monster) p1Card;
                    Monster monsterCard2 = (Monster)p2Card;

                    if(BattleLogic.TryGetValue(new Key() { Dimension1 = monsterCard1.MonsterType.ToLower(), Dimension2 = monsterCard2.MonsterType.ToLower() }, out var i1))
                        multiplication1 *= i1;
                    if(BattleLogic.TryGetValue(new Key() { Dimension1 = monsterCard2.MonsterType.ToLower(), Dimension2 = monsterCard1.MonsterType.ToLower() }, out var i2))
                        multiplication2 *= i2;
                }

                //Segment 3 Add Card to Winner deck
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (p1Card.Damage * multiplication1 == p2Card.Damage * multiplication2) {
                    //TODO Sorter
                }else if(p1Card.Damage * multiplication1 > p2Card.Damage * multiplication2) {
                    deck1.Add(p2Card);
                    deck2.Remove(p2Card);
                }
                else
                {
                    deck2.Add(p2Card);
                    deck1.Remove(p2Card);
                }

                //Segment 4 ...

                //Segment 5 winner
                if (deck1.Count== 0)
                    return false;
                if(deck2.Count== 0)
                    return true;
            } while (count <=100);
            return false; //Todo schicke backlog zurück statt true/false
        }

        private void FillLogic()
        {
            //ElementType
            BattleLogic[new Key() { Dimension1 = "fire", Dimension2 = "grass" }] = timestwo;
            BattleLogic[new Key() { Dimension1 = "grass", Dimension2 = "fire" }] = half;

            BattleLogic[new Key() { Dimension1 = "water", Dimension2 = "fire" }] = timestwo;
            BattleLogic[new Key() { Dimension1 = "fire", Dimension2 = "water" }] = half;

            BattleLogic[new Key() { Dimension1 = "grass", Dimension2 = "water" }] = timestwo;
            BattleLogic[new Key() { Dimension1 = "water", Dimension2 = "grass" }] = half;

            //MonsterType
            BattleLogic[new Key() { Dimension1 = "goblin", Dimension2 = "dragon" }] = zero;

            BattleLogic[new Key() { Dimension1 = "ork", Dimension2 = "wizard" }] = zero;

            BattleLogic[new Key() { Dimension1 = "dragon", Dimension2 = "elf" }] = zero;
            BattleLogic[new Key() { Dimension1 = "elf", Dimension2 = "dragon" }] = zero;

            //ElementType and MonsterType
            BattleLogic[new Key() { Dimension1 = "knight", Dimension2 = "water" }] = zero;

            BattleLogic[new Key() { Dimension1 = "fire", Dimension2 = "kraken" }] = zero;

            BattleLogic[new Key() { Dimension1 = "water", Dimension2 = "kraken" }] = zero;

            BattleLogic[new Key() { Dimension1 = "grass", Dimension2 = "kraken" }] = zero;
            }

        public bool addType(Key key ,double value)
        {
            try
            {
                BattleLogic[key] = value;
                return true; 
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }           
        }
    }
}
