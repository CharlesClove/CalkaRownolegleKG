using System.Collections.Concurrent;
using CalkaRownolegleKG.Interfejsy;

namespace CalkaRownolegleKG.Kalkulatory
{
    public class KalkulatorTrapez
    {
        private readonly StopLoop _stopLoop;
        public KalkulatorTrapez() { _stopLoop = new StopLoop(); }                   //tworze instancje stooploop
        public (List<(int, double)>, bool) metodaTrapezow(ParametryDoCalki parametry, IFunkcja funkcja)
        {
            var wyniki = new ConcurrentBag<(int, double)>();                                    //uzywam bag zeby zebrac wyniki jakie da mi  calka z parallel.for

            Console.WriteLine("\n\tNacisnij klawisz q aby przerwac");

            Task.Run(() => 
                { while (true)
                    {
                        if (Console.ReadKey(true).Key == ConsoleKey.Q)
                        { _stopLoop.Stop(); break; }
                    }
                }
            );
            try
            {
                Parallel.For(0, parametry.podzialy,new ParallelOptions { CancellationToken = _stopLoop.Token}  , i =>
                {
                    ProgressBar progressBar = new ProgressBar(i, parametry.podzialy);

                    var zakres = parametry.ZakresyCalki[i];
                    int poczatek = zakres.Item1;
                    int koniec = zakres.Item2;
                    int iloscPrzedzialow = 10000;
                    double szerokoscTrapezu = (double)(koniec - poczatek) / iloscPrzedzialow;
                    double poleTrapezow = 0;

                    for (int j = 0; j < iloscPrzedzialow; j++)
                    {
                        if (_stopLoop.Token.IsCancellationRequested) { return; }
                        double x1 = poczatek + j * szerokoscTrapezu;
                        double x2 = poczatek + (j + 1) * szerokoscTrapezu;
                        double y1 = funkcja.ObliczX(x1);
                        double y2 = funkcja.ObliczX(x2);

                        poleTrapezow += ((y1 + y2) * szerokoscTrapezu) / 2;

                        progressBar.DrawProgress(j, iloscPrzedzialow);

                    }
                    //Console.WriteLine($"Numer całki#{i + 1}= " + poleTrapezow + "\n");
                    //wynikCalkowania = (poleTrapezow);

                    wyniki.Add((i + 1, poleTrapezow));
                });

            }
            catch (OperationCanceledException) 
            {   
                Console.Clear();
                Console.WriteLine("Przerwano loop, wcisnij przycisk aby kontynuowac");
                Console.ReadKey();
                return (new List<(int, double)>(), true);

            }

            Console.Clear();
            //return wynikCalkowania;
            return (wyniki.ToList(),false);
        }

    }
}
