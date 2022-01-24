using NUnit.Framework;
using Model;
using Main;
using System.Collections.Generic;

namespace MTCG.Test
{
    public class Tests
    {
        Battle BL = Battle.GetInstance();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BattleTest1()
        {
           
            User u1 = new();
            User u2 = new();
            
            Shop shop = new();
            Package package = new();
            Monster monster = new Monster() {id= new System.Guid(), Damage=20, ElementType= "Water", MonsterType="Dragon", Name="WaterDragon"};
            Spell spell = new() { Damage = 20, ElementType = "Grass", id = new System.Guid(), Name = "GrassSpell" };

            List<ICard> list = new();
            list.Add(monster);
            list.Add(monster);
            list.Add(monster);
            list.Add(monster);
            list.Add(monster);
            package.addCards(list);
            shop.addPacakage(package);
            shop.buyPackage(u1);

            list.Clear();
            list.Add(spell);
            list.Add(spell);
            list.Add(spell);
            list.Add(spell);
            list.Add(spell);
            package.addCards(list);
            shop.addPacakage(package);
            shop.buyPackage(u2);

            u1.addCardToDeck(monster);
            u1.addCardToDeck(monster);
            u1.addCardToDeck(monster);
            u1.addCardToDeck(monster);
            u1.addCardToDeck(monster);

            u2.addCardToDeck(spell);
            u2.addCardToDeck(spell);
            u2.addCardToDeck(spell);
            u2.addCardToDeck(spell);
            u2.addCardToDeck(spell);

            Assert.That(BL.Fight(u2, u1).winner);
        }
    }
}