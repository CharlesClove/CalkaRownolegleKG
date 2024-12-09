using System;
using System.Collections.Concurrent;
using System.Diagnostics;


public class MenuHandler
{
    private readonly IMenu _menu;
    private readonly IFunkcjaFactory _funkcjafactory;

    public MenuHandler(IMenu menu, IFunkcjaFactory funkcjaFactory) 
    { 
    
    }
    public (string metoda, IFunkcja funkcja) PobierzWybor()
    {
        _menu.ShowMethod();
        string metoda = _menu.metodaChoice();

        _menu.MenuChoice();
        string chocie = _menu.MenuChoice();

        return (metoda, _funkcjafactory.GetFunkcja(chocie));
    }

}
public class ParametryDoCalki
{
    public int podzialy { get; set; }
    public List<(int poczatek, int koniec)> ZakresyCalki { get; set; }
    public ParametryDoCalki()
    {
        ZakresyCalki = new List<(int, int)>();
    }

}



public interface IFunkcja
{
    double ObliczX(double x);

}
public interface IFunkcjaFactory
{
    IFunkcja GetFunkcja(string choice);
}




public interface IKalkulator
{
    List<(int, double)> ObliczCalke(string metoda, ParametryDoCalki parametry, IFunkcja funkcja);


    void Podsumowanie(Stopwatch stopwatch);
}



public interface IKalkulatoryCalek
{
    List<(int, double)> metodaTrapezu(string metoda, ParametryDoCalki parametry, IFunkcja funkcja);
}



public interface IMenu
{
    void ShowMethod();
    string metodaChoice();
    void ShowMenu();
    string MenuChoice();

}


public interface IZbiorParametrow
{
    void ZbierzParametry();
}


public class FunkcjaFactory : IFunkcjaFactory
{
    private static FunkcjaFactory factory_instance;
    private static readonly object lockObj = new object();
    public static FunkcjaFactory FactoryInstance // wywołuje instancje aby nie podawac obiektu, tylko stworzyl sie tutaj
    { get; } = new FunkcjaFactory();

    private static readonly Dictionary<string, IFunkcja> funkcje = new()
    {
        {"1", new Funkcja1() },
        {"2", new Funkcja2() },
        {"3", new Funkcja3() },
        {"4", new Funkcja4() },
    };
    public IFunkcja GetFunkcja(string choice)
    {
        if (funkcje.TryGetValue(choice, out var funkcja))
            return funkcja;
        throw new Exception("Niepoprawny wybor funkcji");
    }


    private IFunkcja funkcja;
    private string funkcjaName;
    public IFunkcja WybranaFunkcja => funkcja;
    public string FunkcjaName => funkcjaName;

    //public void ChoicePass(string choice)
    //{
    //    if (choice == "0") { Environment.Exit(0); };

    //    try
    //    {
    //        funkcja = FunkcjaFactory.GetFunkcja(choice);
    //        funkcjaName = funkcja.GetType().Name;
    //    }
    //    catch (ArgumentException ex)
    //    {
    //        Console.WriteLine(ex.Message);
    //        Console.WriteLine("Wybierz poprawna opcje");
    //    }
    //}
}


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
public class Funkcja4 : IFunkcja
{
    double IFunkcja.ObliczX(double x) => 3 * Math.Pow(x, 5) + 2 * x - 3;
}

public class Kalkulator : IKalkulator
{
    //private readonly KalkulatorFactory kal_factory;
    private List<(int, double)> wyniki; // lista do zwracancyh wynikow
    private readonly IKalkulatoryCalek _kalkulatoryCalek;
    public Kalkulator(IKalkulatoryCalek kalkulatoryCalek)
    {
        _kalkulatoryCalek = kalkulatoryCalek;
    }

    public List<(int, double)> ObliczCalke(string metoda, ParametryDoCalki parametry, IFunkcja funkcja)
    {




        wyniki = _kalkulatoryCalek.metodaTrapezu(metoda, parametry, funkcja);
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
public class Menu : IMenu
{
    private static Menu menu_instance; //tworze instancje menu 
    public Menu() { }
    private static readonly object lockObj = new object();

    public static Menu MenuInstance { get; } = new Menu();


    private readonly List<string> opcjeMenu = new()
        {
            "1. Funkcja y= 2x + 2x^2 ",
            "2. Funkcja y= 2x^2 +3 ",
            "3. Funkcja y= 3x^2 + 2x - 3",
            "4. Funkcja y=3 * Math.Pow(x, 5) + 2 * x - 3 \t[Nowo dodana]",
            "0. Wyjście"
        };

    public void ShowMenu() //main menu apki
    {
        Console.WriteLine("===== Wybierz funkcję =====");
        foreach (var opcjaMenu in opcjeMenu)
        {
            Console.WriteLine(opcjaMenu);
        }
        Console.WriteLine("Funkcja:");

    }
    public string MenuChoice() { return Console.ReadLine(); }

    private readonly List<string> opcjeMetod = new()
        {
            "1. Parallel",
            "2. Thread",
            "3. ThreadPool",
            "4. Task",
            "0. Wyjście"
        };
    public void ShowMethod()
    {
        Console.WriteLine("===== Wybierz metode =====");
        foreach (var opcjaMetody in opcjeMetod)
        {
            Console.WriteLine(opcjaMetody);
        }
        Console.WriteLine("Metoda:");

    }
    public string metodaChoice() => Console.ReadLine();
}

public class ZbiorParametrow : IZbiorParametrow
{
    private static ZbiorParametrow zbiorParametrow_instance;
    private ParametryDoCalki parametry;
    private ZbiorParametrow()
    {
        parametry = new ParametryDoCalki();
    }
    private static readonly object lockObj = new object();
    public static ZbiorParametrow ZbiorInstance { get; } = new ZbiorParametrow();

    public void ZbierzParametry()
    {
        parametry.ZakresyCalki.Clear();
        Console.WriteLine("\nPodaj ilość podziałów [ile całek chcesz obliczyć]: ");
        //parametry.podzialy = int.Parse(Console.ReadLine());
        parametry.podzialy = 3;
        if(parametry.podzialy <= 0) { return; }

        for (int i = 0; i < parametry.podzialy; i++)
        {
            Console.WriteLine($"\nPodaj zakresy {i + 1} (format: [Początek całkowania,Koniec całkowania]  )");
            var wpisane_zakresy = Console.ReadLine().Split(new[] { ',', ' ' },
                                                           StringSplitOptions.RemoveEmptyEntries);

            int x = int.Parse(wpisane_zakresy[0]);
            int y = int.Parse(wpisane_zakresy[1]);
            if (x == null || y == null) { return; }
            //int iloscprzedzialow = int.Parse(wpisane_zakresy[2]);

            parametry.ZakresyCalki.Add((x, y));

        }
        Console.Clear();

    }
    public ParametryDoCalki GetParametry()
    {
        return parametry;
    }
}

public class ProgressBar
{
    private int taskId;
    private int totalTasks;
    private int lastProgress = -1; // do sledzenia procentow
    public ProgressBar(int taskId, int totalTasks)
    {
        this.taskId = taskId;
        this.totalTasks = totalTasks;

    }
    public void DrawProgress(int current, int total)
    {
        int progressWidth = 50;
        int progressProcenty = (int)((double)current / total * 100);
        if (progressProcenty / 10 > lastProgress)
        {
            lastProgress = progressProcenty / 10;
            int filledWidth = (int)((double)current / total * progressWidth);

            lock (Console.Out)
            {
                int consoleRow = Math.Min(taskId, Console.WindowHeight - 1);
                Console.SetCursorPosition(0, consoleRow);
                if (progressProcenty % 10 == 0)
                {
                    Console.Write($"\nTask {taskId + 1}/{totalTasks}: [");
                    Console.Write(new string('#', filledWidth));
                    Console.Write(new string(' ', progressWidth - filledWidth));
                    Console.Write($"] {current}  /{total}");
                    Console.Write($"] {progressProcenty}%");




                    Thread.Sleep(200);
                }

            }

        }

    }

}


public class StopLoop
{
    private CancellationTokenSource _cts;
    public StopLoop()
    {
        _cts = new CancellationTokenSource();
        NasluchiwanieZatrzymania();
    }
    public CancellationToken Token { get { return _cts.Token; } }
    public void Stop() { _cts.Cancel(); }

    public void Reset()
    {
        if (_cts.IsCancellationRequested)
        {
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }

    private void NasluchiwanieZatrzymania()
    {
        _ = Task.Run(() =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    Stop();
                }
            }
        });
    }
}


public class KalkulatoryCalek : IKalkulatoryCalek
{
    private readonly StopLoop _stopLoop;

    public KalkulatoryCalek()
    {
        _stopLoop = new StopLoop();
    }
    //tworze instancje stooploop
    public List<(int, double)> metodaTrapezu(string metoda, ParametryDoCalki parametry, IFunkcja funkcja)
    {


        return SterowanieKalkulatora(metoda, parametry, funkcja);
    }

    private List<(int, double)> SterowanieKalkulatora(string metoda, ParametryDoCalki parametry, IFunkcja funkcja)
    {

        if (FunkcjaFactory.FactoryInstance.WybranaFunkcja == null)
        {
            throw new InvalidOperationException("Wybrana funkcja nie została zainicjalizowana.");
        }


        _stopLoop.Reset();
        var wyniki = new ConcurrentBag<(int, double)>();



        try
        {

            switch (metoda)
            {
                case "1":
                    ExecuteParallelFor(parametry, funkcja, wyniki);
                    break;

                case "2":

                    break;

                case "3":

                    break;

                case "4":

                    ExecuteTasks(parametry, funkcja, wyniki);
                    break;
                default:
                    throw new ArgumentException($"Nieznana metoda: {metoda}");
            }
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


        Console.Clear();
        //return wynikCalkowania;
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

    }

    private void ExecuteTasks(ParametryDoCalki parametry, IFunkcja funkcja, ConcurrentBag<(int, double)> wyniki)
    {

        if (_stopLoop.Token.IsCancellationRequested)
        { 
            return;
        }
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


class Program
{
    static void Main(string[] args)
    {
        
        IMenu menu = new Menu();
        IFunkcjaFactory funkcjaFactory = new FunkcjaFactory();
        MenuHandler menuHandler = new MenuHandler(menu, funkcjaFactory);


        IKalkulator kalkulator = new Kalkulator(new KalkulatoryCalek());
        while (true)
        {
            var (metoda, funkcja) = menuHandler.PobierzWybor();
            if (metoda == "0") { Environment.Exit(0); }
            //menu.ShowMethod();
            //string metoda = menu.metodaChoice();
            //if (int.Parse(metoda) == 0) { Environment.Exit(0); }
            //Console.Clear();

            //menu.ShowMenu();
            //string choice = menu.MenuChoice();
            //Console.Clear();

            //FunkcjaFactory.FactoryInstance.ChoicePass(choice);
            ZbiorParametrow.ZbiorInstance.ZbierzParametry();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            kalkulator.ObliczCalke(metoda, ZbiorParametrow.ZbiorInstance.GetParametry(), FunkcjaFactory.FactoryInstance.WybranaFunkcja);

            kalkulator.Podsumowanie(stopwatch);
        }
    }
}
