using System.Collections.Concurrent;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Menu.MenuInstance.ShowMenu();
            string choice = Menu.MenuInstance.MenuChoice();
            Menu.MenuInstance.ChoicePass(choice);
            ZbiorParametrow.ZbiorInstance.ZbierzParametry();

            Kalkulator.KalInstance.ParallelForKal(
                ZbiorParametrow.ZbiorInstance.GetParametry(),
                Menu.MenuInstance.GetFunkcja());

            Kalkulator.KalInstance.Podsumowanie();
        }
    }
}
/// <summary>
/// /////////////////////////////////////////////////////////////////////////
/// </summary>
public interface IMenu
{
    void ShowMenu();
    string MenuChoice();
    void ChoicePass(string choice);
}
public class Menu : IMenu
{
    private static Menu menu_instance; //tworze instancje menu 
    private IFunkcja funkcja;
    private Menu() { }
    public static Menu MenuInstance // wywołuje instancje aby nie podawac obiektu, tylko stworzyl sie tutaj
    {
        get
        {
            if (menu_instance == null)
            {
                menu_instance = new Menu();
            }
            return menu_instance;
        }
    }
    public void ShowMenu() //main menu apki
    {

        Console.WriteLine("===== Wybierz funkcję =====\n");
        Console.WriteLine("1. Funkcja y= 2x + 2x^2 ");
        Console.WriteLine("2. Funkcja y= 2x^2 +3 ");
        Console.WriteLine("3. Funkcja y= 3x^2 + 2x - 3");
        Console.WriteLine("4. Funkcja y=3 * Math.Pow(x, 5) + 2 * x - 3 \t[Nowo dodana]");
        Console.WriteLine("0. Wyjście");

        Console.Write("\nWybierz opcję: ");

    }
    public string MenuChoice() { return Console.ReadLine(); }
    public void ChoicePass(string choice) // funkcja podająca wybor z showMenu i wybierająca odpowiednia funkcje
    {
        if (choice == "0")
        {
            Environment.Exit(0);
        }

        funkcja = choice switch
        {
            "1" => new Funkcja1(),
            "2" => new Funkcja2(),
            "3" => new Funkcja3(),
            "4" => new Funkcja4(),
            _ => throw new Exception("Zly wybor")


        };
    }
    public IFunkcja GetFunkcja() // zwraca funkcje wybrana w choicePass
    {
        return funkcja;
    }
}
/// <summary>
/// //////////////////////////////////////////////////////////////////////
/// </summary>
public interface IFunkcja
{
    double ObliczX(double x);
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

/// <summary>
/// /////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
public class ParametryDoCalki
{
    public int podzialy { get; set; }
    public List<Tuple<int, int>> ZakresyCalki { get; set; }
    public ParametryDoCalki()
    {
        ZakresyCalki = new List<Tuple<int, int>>();
    }

}

/// <summary>
/// ////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
public interface IZbiorParametrow
{
    void ZbierzParametry();
}

public class ZbiorParametrow : IZbiorParametrow
{
    private static ZbiorParametrow zbiorParametrow_instance;
    private ParametryDoCalki parametry;
    private ZbiorParametrow()
    {
        parametry = new ParametryDoCalki();
    }

    public static ZbiorParametrow ZbiorInstance
    {
        get
        {
            if (zbiorParametrow_instance == null)
            {
                zbiorParametrow_instance = new ZbiorParametrow();
            }
            return zbiorParametrow_instance;
        }
    }
    public void ZbierzParametry()
    {
        parametry.ZakresyCalki.Clear();
        Console.WriteLine("\nPodaj ilość podziałów [ile całek chcesz obliczyć]: ");
        //parametry.podzialy = int.Parse(Console.ReadLine());
        parametry.podzialy = 3
            ;
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
        Console.Clear();

    }
    public ParametryDoCalki GetParametry()
    {
        return parametry;
    }
}

public interface IKalkulator
{
    (List<(int, double)>, bool) ParallelForKal(ParametryDoCalki parametry, IFunkcja funkcja);
    void ThreadKal();
    void ThreadPoolKal();

    void Podsumowanie();
}

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
        return (wynikicalek, przerwano);
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


////////////////////////////////////////////////////////////////////////
///
public class StopLoop
{
    private CancellationTokenSource _cts;
    public StopLoop()
    {
        _cts = new CancellationTokenSource();
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
}
///////////////////////////////////////////////////////////////////////
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

public class KalkulatorTrapez
{
    private readonly StopLoop _stopLoop;
    public KalkulatorTrapez() { _stopLoop = new StopLoop(); }
    //tworze instancje stooploop
    private static bool _isCancelled = false;
    public (List<(int, double)>, bool) metodaTrapezow(ParametryDoCalki parametry, IFunkcja funkcja)
    {
        _stopLoop.Reset();
        var wyniki = new ConcurrentBag<(int, double)>();                                    //uzywam bag zeby zebrac wyniki jakie da mi  calka z parallel.for

        bool runningParallel = true;
        Task.Run(() =>
        {
            while (runningParallel)
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

            Parallel.For(0, parametry.podzialy, new ParallelOptions { CancellationToken = _stopLoop.Token }, i =>
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
        finally
        {
            runningParallel = false;
        }

        Console.Clear();
        //return wynikCalkowania;
        return (wyniki.ToList(), false);
    }

}