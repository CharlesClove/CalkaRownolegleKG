class Program
{
    static void Main(string[] args)
    {
        int total = 10; // Liczba iteracji w pętli

        Console.WriteLine("Rozpoczynam wypisywanie liczb:");
        for (int i = 1; i <= total; i++)
        {


            // Wyświetlenie liczby
            Console.Write($"Wypisuję liczbę: {i} ");

            // Rysowanie paska postępu
            DrawProgressBar(i, total);

            Console.WriteLine(); // Nowa linia po każdej iteracji
        }

        Console.WriteLine("Zakończono!");
    }

    static void DrawProgressBar(int current, int total)
    {
        int progressWidth = 20; // Szerokość paska postępu
        int filledWidth = (int)((double)current / total * progressWidth);

        Console.Write("["); // Lewa granica
        Console.Write(new string('#', filledWidth)); // Wypełnione pole
        Console.Write(new string(' ', progressWidth - filledWidth)); // Puste pole
        Console.Write($"] {current}/{total}");
        Thread.Sleep(1000);
        Console.Clear();
    }
}




