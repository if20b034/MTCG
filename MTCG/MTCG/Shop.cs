using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace MTCG
{
    public class Shop
    {
        public List<Package> Packages { get; set; }

        public Shop()
        {
            Packages = new();
        }

        public bool addPacakage(Package package)
        {
            try
            {
                Packages.Add(package);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool buyPackage(User user)
        {
            try
            {
                if (!user.buyPack(Packages[0]))
                    return false;

                Packages.Remove(Packages[0]);
                return true;
                
            }
            catch (Exception e)
            {
                return false; 
            }
        }


    }
}
