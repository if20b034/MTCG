using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface ICard
    {
        string Name { get; set; }
        int Damage { get; set; }
        ElementType ElementType { get; set; }
    }
}
