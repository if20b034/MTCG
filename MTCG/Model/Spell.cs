using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Spell : ICard
    {
        public string Name { get ; set; }
        public int Damage { get ; set; }
        public string ElementType { get; set; }
        public Guid id { get; set; }
    }
}
