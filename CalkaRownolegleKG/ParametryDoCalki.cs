using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalkaRownolegleKG
{
    public class ParametryDoCalki
    {
        public int podzialy { get; set; }
        public List<Tuple<int , int>> ZakresyCalki { get; set; }
        public ParametryDoCalki() { 
            ZakresyCalki = new List<Tuple<int , int>>();
        }
        
    }
}
