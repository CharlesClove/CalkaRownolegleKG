using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalkaRownolegleKG.Interfejsy;
using CalkaRownolegleKG.Kalkulatory;

namespace CalkaRownolegleKG.Funkcje
{
    public class Kalkulator : IKalkulator
    {
        private static Kalkulator kal_instance; // instancja kal
        private KalkulatorTrapez KalTra = new KalkulatorTrapez();// utworzenie Kalkulatoratrapeza parallelfor

        private (List<(int, double)>, bool) wyniki; // lista do zwracancyh wynikow
        public static Kalkulator KalInstance // tworzenie instancji kalkulator
        {
            get
            {
                if (kal_instance == null)
                {
                    kal_instance = new Kalkulator();
                }
                return kal_instance;
            }
        }

        public (List<(int, double)>, bool) ParallelForKal(ParametryDoCalki parametry, IFunkcja funkcja) // kalkulator parallel for 
        {
            Console.Write("Naciśnij Q, żeby anulować");
            Thread.Sleep(1000);

            var (wynikicalek, przerwano) = KalTra.metodaTrapezow(parametry, funkcja);
            wyniki = (wynikicalek, przerwano);
            return (wynikicalek,przerwano);
        }
        public void ThreadKal() { }
        public void ThreadPoolKal() { }

        public void Podsumowanie() // wyswietla podsumowanie obliczen
        {
            var wynikicalek = wyniki.Item1;
            var przerwano = wyniki.Item2;

            if (!przerwano)
            {

                wynikicalek.Sort();
                Console.WriteLine("\nPodsumowanie:");
                Console.WriteLine($"Funkcja: {Menu.MenuInstance.GetFunkcja().GetType().Name}");
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
