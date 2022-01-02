using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Key : IEquatable<Key>
    {
        public string Dimension1 { get; set;  }
        public string Dimension2 { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Key);
        }

        public bool Equals(Key other)
        {
            return other != null &&
                   Dimension1 == other.Dimension1 &&
                   Dimension2 == other.Dimension2;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Dimension1, Dimension2);
        }
    }
}
