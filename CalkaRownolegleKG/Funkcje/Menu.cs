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
        private Menu() {}
        private static readonly object lockObj = new object();

        public static Menu MenuInstance // wywołuje instancje aby nie podawac obiektu, tylko stworzyl sie tutaj
        {
            get
            {
                if(menu_instance == null)
                {
                    lock (lockObj)
                    {
                        if (menu_instance == null)
                        {
                            menu_instance = new Menu();
                        }
                    }
                }
                return menu_instance;
            }
        }
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
}
