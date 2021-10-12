using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Monster: ICard
    { 
        public string Name { get; set; }
        public int Damage { get; set; }
        public ElementType ElementType { get; set; }

        public MonsterType MonsterType { get; set; }
    }
}
