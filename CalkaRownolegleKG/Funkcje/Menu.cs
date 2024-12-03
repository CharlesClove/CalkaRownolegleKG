using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalkaRownolegleKG.Interfejsy;

namespace CalkaRownolegleKG.Funkcje
{
    public class Menu : IMenu
    {
        private static Menu menu_instance; //tworze instancje menu 
        private IFunkcja funkcja;
        private Menu() {}
        public static Menu MenuInstance // wywołuje instancje aby nie podawac obiektu, tylko stworzyl sie tutaj
        {
            get
            {
                if(menu_instance == null)
                {
                    menu_instance = new Menu();
                }
                return menu_instance;
            }
        }
        public string ShowMenu() //main menu apki
        {

            Console.WriteLine("===== Wybierz funkcję =====\n");
            Console.WriteLine("1. Funkcja y= 2x + 2x^2 ");
            Console.WriteLine("2. Funkcja y= 2x^2 +3 ");
            Console.WriteLine("3. Funkcja y= 3x^2 + 2x - 3");
            Console.WriteLine("4. Funkcja y=3 * Math.Pow(x, 5) + 2 * x - 3 \t[Nowo dodana]");
            Console.WriteLine("0. Wyjście");

            Console.Write("\nWybierz opcję: ");
            return Console.ReadLine();
        }
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
}
