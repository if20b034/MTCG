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
                        Dimension1 = p1Card.ElementType,
                        Dimension2 = p2Card.ElementType
                    }, out var output))
                        multiplication1 *= output;
                    if (BattleLogic.TryGetValue(new Key()
                    {
                        Dimension1 = p2Card.ElementType,
                        Dimension2 = p1Card.ElementType
                    }, out output))
                        multiplication2 *= output;

                    //TODO: 

                    if (p1Card.GetType() == typeof(Spell) && p2Card.GetType() == typeof(Monster))
                    {
                        Monster monsterCard2 = (Monster)p2Card;
                       
                        if (BattleLogic.TryGetValue(new Key()
                        {
                            Dimension1 = monsterCard2.MonsterType,
                            Dimension2 = p1Card.ElementType
                        }, out output))
                            multiplication1 *= output;
                        if (BattleLogic.TryGetValue(new Key()
                        {
                            Dimension1 = p1Card.ElementType,
                            Dimension2 = monsterCard2.MonsterType
                        }, out output))
                            multiplication2 *= output;
                    }
                    else if (p1Card.GetType() == typeof(Monster) && p2Card.GetType() == typeof(Spell)){

                        Monster monsterCard1 = (Monster)p1Card;

                        if (BattleLogic.TryGetValue(new Key()
                        {
                            Dimension1 = p2Card.ElementType,
                            Dimension2 = monsterCard1.MonsterType
                        }, out output))
                            multiplication1 *= output;
                        if (BattleLogic.TryGetValue(new Key()
                        {
                            Dimension1 = monsterCard1.MonsterType,
                            Dimension2 = p2Card.ElementType
                        }, out output))
                            multiplication2 *= output;
                    }
                }
                else
                {
                    Monster monsterCard1 = (Monster) p1Card;
                    Monster monsterCard2 = (Monster)p2Card;

                    if(BattleLogic.TryGetValue(new Key() { Dimension1 = monsterCard1.MonsterType, Dimension2 = monsterCard2.MonsterType }, out var i1))
                        multiplication1 *= i1;
                    if(BattleLogic.TryGetValue(new Key() { Dimension1 = monsterCard2.MonsterType, Dimension2 = monsterCard1.MonsterType }, out var i2))
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
            BattleLogic[new Key() { Dimension1 = "Fire", Dimension2 = "Grass" }] = timestwo;
            BattleLogic[new Key() { Dimension1 = "Grass", Dimension2 = "Fire" }] = half;

            BattleLogic[new Key() { Dimension1 = "Water", Dimension2 = "Fire" }] = timestwo;
            BattleLogic[new Key() { Dimension1 = "Fire", Dimension2 = "Water" }] = half;

            BattleLogic[new Key() { Dimension1 = "Grass", Dimension2 = "Water" }] = timestwo;
            BattleLogic[new Key() { Dimension1 = "Water", Dimension2 = "Grass" }] = half;

            //MonsterType
            BattleLogic[new Key() { Dimension1 = "Goblin", Dimension2 = "Dragon" }] = zero;

            BattleLogic[new Key() { Dimension1 = "Ork", Dimension2 = "Wizard" }] = zero;

            BattleLogic[new Key() { Dimension1 = "Dragon", Dimension2 = "Elf" }] = zero;
            BattleLogic[new Key() { Dimension1 = "Elf", Dimension2 = "Dragon" }] = zero;

            //ElementType and MonsterType
            BattleLogic[new Key() { Dimension1 = "Knight", Dimension2 = "Water" }] = zero;

            BattleLogic[new Key() { Dimension1 = "Fire", Dimension2 = "Kraken" }] = zero;

            BattleLogic[new Key() { Dimension1 = "Water", Dimension2 = "Kraken" }] = zero;

            BattleLogic[new Key() { Dimension1 = "Grass", Dimension2 = "Kraken" }] = zero;
            }

        public bool addType(Key key ,double value)
        {
            try
            {
                BattleLogic[key] = value;
                return true; 
            }catch(Exception e)
            {
                return false;
            }           
        }
    }
}
