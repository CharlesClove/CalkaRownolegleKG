using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalkaRownolegleKG.Interfejsy;

namespace CalkaRownolegleKG.Funkcje
{
    public class Funkcja1 : IFunkcja
    {
        double IFunkcja.ObliczX(double x) => 2 * x + 2 * Math.Pow(x, 2);
    }
    public class Funkcja2 : IFunkcja
    {
        double IFunkcja.ObliczX(double x) => 2 * Math.Pow(x, 2) + 3;
    }
    public class Funkcja3 : IFunkcja
    {
        double IFunkcja.ObliczX(double x) => 3 * Math.Pow(x, 2) + 2 * x - 3;
    }
}
