using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using CalkaRownolegleKG.Interfejsy;
using CalkaRownolegleKG.Kalkulatory;

namespace CalkaRownolegleKG.Funkcje
{
    public class Kalkulator : IKalkulator
    {
        //private readonly KalkulatorFactory kal_factory;
        private List<(int, double)> wyniki; // lista do zwracancyh wynikow
        private readonly IKalkulatoryCalek _kalkulatoryCalek;
        
        public Kalkulator()
        {
            _kalkulatoryCalek = new KalkulatoryCalek();
        }

        public List<(int, double)> ObliczCalke(string metoda,ParametryDoCalki parametry, IFunkcja funkcja)
        {

            wyniki = _kalkulatoryCalek.metodaTrapezu(metoda, parametry,funkcja);
            return wyniki;
        }
       

        public void Podsumowanie(Stopwatch stopwatch) // wyswietla podsumowanie obliczen
        {
            stopwatch.Stop();
            wyniki.Sort();
            Console.WriteLine("\nPodsumowanie:");
            Console.WriteLine($"Funkcja: {FunkcjaFactory.FactoryInstance.FunkcjaName}");
            for (int i = 0; i < ZbiorParametrow.ZbiorInstance.GetParametry().ZakresyCalki.Count; i++)
            {

                Console.WriteLine($"Przedział{i + 1}: {ZbiorParametrow.ZbiorInstance.GetParametry().ZakresyCalki[i]}");
            }
            Console.WriteLine();
            Console.WriteLine();
            foreach (var wynik in wyniki)
            {

                Console.WriteLine($"Całka#{wynik.Item1}=== {wynik.Item2}");
            }
            Console.WriteLine();
            Console.WriteLine($"Suma wynikow: {wyniki.Sum(WynikSumy => WynikSumy.Item2)}");
            
            
            Console.WriteLine($"Czas trwania: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine("Nacisnij przycisk zeby kontynuowac");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
