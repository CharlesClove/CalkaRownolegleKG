using CalkaRownolegleKG.Funkcje;
using CalkaRownolegleKG.Interfejsy;
using CalkaRownolegleKG.Kalkulatory;

namespace CalkaRownolegleKG
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("===== Wybierz funkcję =====\n");
                Console.WriteLine("1. Funkcja y= 2x + 2x^2 ");
                Console.WriteLine("2. Funkcja y= 2x^2 +3 ");
                Console.WriteLine("3. Funkcja y= 3x^2 + 2x - 3");
                Console.WriteLine("0. Wyjście");

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
                     _  => throw new Exception("Zly wybor")

                };

                ParametryDoCalki parametry = new ParametryDoCalki();
                Console.WriteLine("\nPodaj ilość podziałów [ile całek chcesz obliczyć]: ");
                //parametry.podzialy = int.Parse(Console.ReadLine());
                parametry.podzialy = 1;
                for (int i = 0; i < parametry.podzialy; i++)
                {
                    Console.WriteLine($"\nPodaj zakresy {i + 1} (format: [Początek całkowania,Koniec całkowania]  )");
                    var wpisane_zakresy = Console.ReadLine().Split(new[] { ',', ' ' },
                                                                   StringSplitOptions.RemoveEmptyEntries);
                    int x = int.Parse(wpisane_zakresy[0]);
                    int y = int.Parse(wpisane_zakresy[1]);
                    //int iloscprzedzialow = int.Parse(wpisane_zakresy[2]);
                    parametry.ZakresyCalki.Add(new Tuple<int, int>(x, y));
                }

                KalkulatorTrapez kalkulatorTra = new KalkulatorTrapez();
                var (wynikicalek, przerwano) = kalkulatorTra.metodaTrapezow(parametry, funkcja);
                if (!przerwano)
                {

                    wynikicalek.Sort();
                    Console.WriteLine("\nPodsumowanie:");
                    Console.WriteLine($"Funkcja: {funkcja.GetType().Name}");
                    for (int i = 0; i < parametry.ZakresyCalki.Count; i++)
                    {

                        Console.WriteLine($"Przedział{i + 1}: {parametry.ZakresyCalki[i]}");
                    }
                    foreach (var wynik in wynikicalek)
                    {

                        Console.WriteLine($"Całka#{wynik.Item1}=== {wynik.Item2}");
                    }
                    Console.WriteLine($"Suma wynikow: {wynikicalek.Sum(WynikSumy => WynikSumy.Item2)}");
                    Console.WriteLine("Nacisnij przycisk zeby kontynuowac");
                    Console.ReadKey();
                    Console.Clear();
                }
                Console.Clear();
            }
        }
    }
}
