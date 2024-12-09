using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Markup;
using CalkaRownolegleKG.Funkcje;
using CalkaRownolegleKG.Interfejsy;
using System.Reflection;

namespace CalkaRownolegleKG.Kalkulatory
{
    public class KalkulatoryCalek : IKalkulatoryCalek
    {
        private readonly StopLoop _stopLoop;
        
        public KalkulatoryCalek() 
        {
            _stopLoop = new StopLoop(); //tworze instancje stooploop
        }
        
        public List<(int, double)> metodaTrapezu(string metoda, ParametryDoCalki parametry, IFunkcja funkcja)
        {
            return SterowanieKalkulatora(metoda, parametry, funkcja);
        }

        private List<(int, double)> SterowanieKalkulatora(string metoda, ParametryDoCalki parametry, IFunkcja funkcja )
        {
            if (FunkcjaFactory.FactoryInstance.WybranaFunkcja == null)
            {
                throw new InvalidOperationException("Wybrana funkcja nie została zainicjalizowana.");
            }

            _stopLoop.Reset();
            var wyniki = new ConcurrentBag<(int, double)>();
            using var cts_nasluchujQ = new CancellationTokenSource(); // lokalny CancelToken do zatrzymania taska uruchomionego przez nasluchujQ, zeby nie dzialal w tle.

            try
            {
                NasluchujQ(cts_nasluchujQ.Token);
                switch (metoda) 
                {
                    case "1"://parallel.for
                        
                        ExecuteParallelFor(parametry, funkcja, wyniki);
                        
                        break;

                    case "2": //Thread
                        
                        ExecuteThread(parametry, funkcja, wyniki);
                        
                        break;

                    case "3": //Threadpool

                        ExecuteThreadpool(parametry, funkcja, wyniki);
                        
                        break;

                    case "4"://Tasks
                        
                        ExecuteTasks(parametry, funkcja, wyniki);
                        
                        break ;

                    default: //zla metoda
                        throw new ArgumentException($"Nieznana metoda: {metoda}");
                }
                cts_nasluchujQ.Cancel();
            }
            catch (OperationCanceledException) 
            {   
                Console.Clear();
                Console.WriteLine("Przerwano loop, wcisnij przycisk aby kontynuowac");
                return (new List<(int, double)>());

            }
            finally 
            {
                while (Console.KeyAvailable) Console.ReadKey(true);
            }
            Thread.Sleep(1500);
            Console.Clear();
            return (wyniki.ToList());
        }

        private void ExecuteParallelFor(ParametryDoCalki parametry, IFunkcja funkcja, ConcurrentBag<(int, double)> wyniki)
        {
            Parallel.For(0, parametry.podzialy, new ParallelOptions { CancellationToken = _stopLoop.Token }, i =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                ObliczCalkiTrapez(parametry, funkcja, wyniki, i);
                sw.Stop();
                Console.WriteLine($"\n\n\nCzas dla calki #{i}: {sw.ElapsedMilliseconds}ms");
            });
            Console.WriteLine("\nKoniec wykonywania parallel.for");
        }

        private void ExecuteThread(ParametryDoCalki parametry, IFunkcja funkcja, ConcurrentBag<(int, double)> wyniki) 
        {   
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < parametry.ZakresyCalki.Count; i++)
            {
                int index = i;
                var threadStart = new ThreadStart(() => ObliczCalkiTrapez(parametry, funkcja, wyniki, index));
                Thread thread = new Thread(threadStart);
                thread.Start();
                threads.Add(thread);
            }
            foreach (var thread in threads)
            {
                thread.Join(); //metoda blokuje wątek do czesu zakonczenia pracy wątku, aby program nie szedł dalej ( podobne do Task.WhenAll(); )
            }

            Console.WriteLine("\nKoniec wykonywania parallel Thread");
            
        }
        private void ExecuteThreadpool(ParametryDoCalki parametry, IFunkcja funkcja, ConcurrentBag<(int, double)> wyniki)
        {

            ManualResetEvent[] doneEvents = new ManualResetEvent[parametry.ZakresyCalki.Count]; // Tablica zdarzen sygnalizujących zakończenie pracy watkow

            for (int i = 0; i < parametry.ZakresyCalki.Count; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                int index = i;

                
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        ObliczCalkiTrapez(parametry, funkcja, wyniki, index);
                    }
                    finally
                    {
                        // sygnal zakonczenia pracy dla bieżącego wątku threadpool
                        doneEvents[index].Set();
                    }
                });
            }

            
            WaitHandle.WaitAll(doneEvents); //oczekiwanie na zakonczenie wszystkich zadan z tablicy zdarzen.

            Console.WriteLine("\nKoniec wykonywania parallel ThreadPool");
        }


        
        

        private void ExecuteTasks(ParametryDoCalki parametry, IFunkcja funkcja, ConcurrentBag<(int, double)> wyniki)
        {
            List<Task> listaTaskow = new List<Task>();
            for (int i = 0; i < parametry.ZakresyCalki.Count; i++)
            {

                int index = i;
                var tasks = Task.Run(() =>
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    ObliczCalkiTrapez(parametry, funkcja, wyniki, index);
                    sw.Stop();
                    Console.WriteLine($"\n\n\nCzas dla calki #{i}: {sw.ElapsedMilliseconds}ms");

                }, _stopLoop.Token);
                listaTaskow.Add(tasks);
            }
            Task.WhenAll(listaTaskow).GetAwaiter().GetResult();
            Console.WriteLine("\nKoniec wykonywania parallel Task");
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

                progressBar.DrawProgress(j+1, iloscPrzedzialow);


            }
            //Console.WriteLine($"Numer całki#{i + 1}= " + poleTrapezow + "\n");
            //wynikCalkowania = (poleTrapezow);

            wyniki.Add((i + 1, poleTrapezow));
        }
        private void NasluchujQ(CancellationToken token)
        {
            Console.WriteLine("press Q to end");
            var stopper = Task.Run(() =>
            {
                while (!token.IsCancellationRequested && !_stopLoop.Token.IsCancellationRequested)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                    {
                        _stopLoop.Stop();
                        Console.WriteLine("\n\n\n\nZatrzymano liczenie");
                        Thread.Sleep(1500);

                    }
                }

            },token);
            
        }

    }

   
}

//private void ExecuteThreadpool(ParametryDoCalki parametry, IFunkcja funkcja, ConcurrentBag<(int, double)> wyniki)
//{
//    using (var countdown = new CountdownEvent(parametry.ZakresyCalki.Count)) // uzywam klasy CountdownEvent zeby zsynchronizowac zakonczenie zadan threadpool. 
//    {
//        for (int i = 0; i < parametry.ZakresyCalki.Count; i++)
//        {
//            int index = i;
//            ThreadPool.QueueUserWorkItem(_ =>
//            {
//                try
//                {
//                    ObliczCalkiTrapez(parametry, funkcja, wyniki, index);
//                }
//                finally
//                {
//                    countdown.Signal(); //Daje znać jeden wątek skonczyl prace
//                }
//            });
//            countdown.Wait(); //czeka tutaj az wszystkie watki skoncza prace.

//        }
//    }
//    Console.WriteLine("Koniec wykonywania parallel ThreadPool");
//}
