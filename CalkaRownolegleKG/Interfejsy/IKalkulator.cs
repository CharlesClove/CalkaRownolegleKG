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
        (List<(int, double)>, bool) ParallelForKal(ParametryDoCalki parametry, IFunkcja funkcja);
        void ThreadKal();
        void ThreadPoolKal();

        void Podsumowanie();
    }
}
