using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalkaRownolegleKG.Funkcje;

namespace CalkaRownolegleKG.Interfejsy
{
    public interface IKalkulator
    {
        (List<(int, double)>, bool) ObliczCalke(string metoda, ParametryDoCalki parametry, IFunkcja funkcja);
        

        void Podsumowanie();
    }
}
