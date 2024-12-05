using System;
using System.Collections.Generic;
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
        private readonly KalkulatorFactory kal_factory;
        private (List<(int, double)>, bool) wyniki; // lista do zwracancyh wynikow
        
        public Kalkulator()
        {
            var kalkulatorTrapez = new KalkulatoryCalek();
            kal_factory = new KalkulatorFactory(kalkulatorTrapez);
        }
        
        public (List<(int, double)>, bool) ObliczCalke(string metoda,ParametryDoCalki parametry, IFunkcja funkcja)
        {
            Console.Write("Naciśnij Q, żeby anulować");
            Thread.Sleep(1000);

            var metodaWielowatkowosci = kal_factory.GetMetoda(metoda);
            wyniki = metodaWielowatkowosci(parametry, funkcja);
            return wyniki;
        }
       

        public void Podsumowanie() // wyswietla podsumowanie obliczen
        {
            var wynikicalek = wyniki.Item1;
            var przerwano = wyniki.Item2;

            if (!przerwano)
            {

                wynikicalek.Sort();
                Console.WriteLine("\nPodsumowanie:");
                Console.WriteLine($"Funkcja: {FunkcjaFactory.FactoryInstance.FunkcjaName}");
                for (int i = 0; i < ZbiorParametrow.ZbiorInstance.GetParametry().ZakresyCalki.Count; i++)
                {

                    Console.WriteLine($"Przedział{i + 1}: {ZbiorParametrow.ZbiorInstance.GetParametry().ZakresyCalki[i]}");
                }
                Console.WriteLine();
                Console.WriteLine();
                foreach (var wynik in wynikicalek)
                {

                    Console.WriteLine($"Całka#{wynik.Item1}=== {wynik.Item2}");
                }
                Console.WriteLine();
                Console.WriteLine($"Suma wynikow: {wynikicalek.Sum(WynikSumy => WynikSumy.Item2)}");
                Console.WriteLine("Nacisnij przycisk zeby kontynuowac");
                Console.ReadKey();
                Console.Clear();
            }

            Console.Clear();
        }
    }
}
