using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalkaRownolegleKG.Interfejsy
{
    public interface IKalkulatoryCalek
    {
        List<(int, double)> metodaTrapezu(string metoda,ParametryDoCalki parametry, IFunkcja funkcja);
    }
}
