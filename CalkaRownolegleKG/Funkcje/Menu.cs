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
        private static Menu menu_instance;
        private IFunkcja funkcja;
        private Menu() {}
        public static Menu MenuInstance
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
        public string ShowMenu()
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
        public void ChoicePass(string choice) 
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
        public IFunkcja GetFunkcja()
        {
            return funkcja;
        }
    }
}
