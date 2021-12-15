using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface ICard
    {
        int id { get; set; }
        string Name { get; set; }
        int Damage { get; set; }
        string ElementType { get; set; }
    }
}
