using System.Collections.Concurrent;
using CalkaRownolegleKG.Funkcje;
using CalkaRownolegleKG.Interfejsy;

namespace CalkaRownolegleKG.Kalkulatory
{
    public class KalkulatoryCalek
    {
        private readonly StopLoop _stopLoop;
        public KalkulatoryCalek() 
        {
            _stopLoop = new StopLoop(); 
        } 
        //tworze instancje stooploop
        public (List<(int, double)>, bool) metodaTrapezu_Parallel( ParametryDoCalki parametry, IFunkcja funkcja)
        {
            return SterowanieKalkulatora("parallel", parametry, funkcja);
        }
        public (List<(int, double)>, bool) metodaTrapezu_Thread(ParametryDoCalki parametry, IFunkcja funkcja)
        {
            return SterowanieKalkulatora("thread", parametry, funkcja);
        }
        public (List<(int, double)>, bool) metodaTrapezu_Threadpool(ParametryDoCalki parametry, IFunkcja funkcja)
        {
            return SterowanieKalkulatora("threadpool", parametry, funkcja);
        }

        private (List<(int, double)>, bool) SterowanieKalkulatora(string metoda, ParametryDoCalki parametry, IFunkcja funkcja )
        {

            if (FunkcjaFactory.FactoryInstance.WybranaFunkcja == null)
            {
                throw new InvalidOperationException("Wybrana funkcja nie została zainicjalizowana.");
            }


            _stopLoop.Reset();
            var wyniki = new ConcurrentBag<(int, double)>();                                      
            
            bool runningParallel = true;
            Task.Run(() => 
                { while (runningParallel)
                    {
                        if (Console.ReadKey(true).Key == ConsoleKey.Q)
                        {
                            _stopLoop.Stop(); 
                            runningParallel = false; 
                        }
                    }
                }
            );
            try
            {
                switch (metoda.ToLower()) 
                {
                    case "parallel":
                        Parallel.For(0, parametry.podzialy, new ParallelOptions { CancellationToken = _stopLoop.Token }, i =>
                        {
                            ObliczCalkiTrapez(parametry, funkcja,wyniki,i);
                        });
                        break;
                    case "thread":
                        
                        break;
                    case "threadpool":
                        
                        break;
                }
            }
            catch (OperationCanceledException) 
            {   
                Console.Clear();
                Console.WriteLine("Przerwano loop, wcisnij przycisk aby kontynuowac");
                Console.ReadKey();
                return (new List<(int, double)>(), true);

            }
            finally
            {
                runningParallel = false;
            }

            Console.Clear();
            //return wynikCalkowania;
            return (wyniki.ToList(),false);
        }
        private void ObliczCalkiTrapez(ParametryDoCalki parametry, IFunkcja funkcja, ConcurrentBag<(int, double)> wyniki, int i)
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
        }

    }
}
