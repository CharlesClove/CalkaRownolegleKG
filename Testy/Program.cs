using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using System.Text;







    public interface IFunkcja
    {
        double ObliczX(double x);
    }


public class Funkcja1 : IFunkcja
{
    public double ObliczX(double x) => 2 * x + 2 * Math.Pow(x, 2);
}

public class Funkcja2 : IFunkcja
{
    public double ObliczX(double x) => 2 * Math.Pow(x, 2) + 3;
}

public class Funkcja3 : IFunkcja
{
    public double ObliczX(double x) => 3 * Math.Pow(x, 2) + 2 * x - 3;
}


    public class ParametryDoCalki
    {
        public int podzialy { get; set; }
        public List<Tuple<int, int, int>> ZakresyCalki { get; set; }
        public ParametryDoCalki()
        {
            ZakresyCalki = new List<Tuple<int, int, int>>();
        }

    }



    public class KalkulatorTrapez
    {
        public List<double> Wyniki { get; private set; } = new List<double>();
        public void metodaTrapezow(ParametryDoCalki parametry, IFunkcja funkcja)
        {
            Parallel.For(0, parametry.podzialy, i =>
            {
                var zakres = parametry.ZakresyCalki[i];
                int poczatek = zakres.Item1;
                int koniec = zakres.Item2;
                int iloscPrzedzialow = zakres.Item3;
                double szerokoscTrapezu = (double)(koniec - poczatek) / iloscPrzedzialow;
                double poleTrapezow = 0;

                for (int j = 0; j < iloscPrzedzialow; j++)
                {
                    double x1 = poczatek + j * szerokoscTrapezu;
                    double x2 = poczatek + (j + 1) * szerokoscTrapezu;
                    double y1 = funkcja.ObliczX(x1);
                    double y2 = funkcja.ObliczX(x2);

                    poleTrapezow += ((y1 + y2) * szerokoscTrapezu) / 2;
                }
                Console.WriteLine($"Wynik całki {i+1}= " + poleTrapezow + "\n");
                
            });
            
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("\n\n===== Wybierz funkcję =====\n");
                Console.WriteLine("\t1. Funkcja y=2x + 2x^2 ");
                Console.WriteLine("\t2. Funkcja y=2x^2 +3 ");
                Console.WriteLine("\t3. Funkcja y=3x^2 + 2x - 3");
                Console.WriteLine("\t0. Wyjście");

                Console.Write("\nWybierz opcję: ");
                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    Console.WriteLine("Do widzenia!");
                    break;
                }

                IFunkcja funkcja = choice switch
                {
                    "1" => new Funkcja1(),
                    "2" => new Funkcja2(),
                    "3" => new Funkcja3(),
                };

                ParametryDoCalki parametry = new ParametryDoCalki();
                Console.WriteLine("Podaj ilość podziałów [ile całek chcesz obliczyć]: ");
            //parametry.podzialy = int.Parse(Console.ReadLine());
            parametry.podzialy = 3;
                for (int i = 0; i < parametry.podzialy; i++)
                {
                    Console.WriteLine($"Podaj zakresy {i + 1} (format: x, y, ilość przedziałów do obliczen)");
                    var wpisane_zakresy = Console.ReadLine().Split(",");
                    int x = int.Parse(wpisane_zakresy[0]);
                    int y = int.Parse(wpisane_zakresy[1]);
                    int iloscprzedzialow = int.Parse(wpisane_zakresy[2]);
                    parametry.ZakresyCalki.Add(new Tuple<int, int, int>(x, y, iloscprzedzialow));
                }
                KalkulatorTrapez kalkulatorTra = new KalkulatorTrapez();
                kalkulatorTra.metodaTrapezow(parametry, funkcja);
                
            }
        }
    }

