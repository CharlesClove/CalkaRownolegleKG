using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalkaRownolegleKG.Interfejsy;

namespace CalkaRownolegleKG.Funkcje
{
    public class FunkcjaFactory : IFunkcjaFactory
    {
        private static FunkcjaFactory factory_instance;
        private static readonly object lockObj = new object();
        public static FunkcjaFactory FactoryInstance // wywołuje instancje aby nie podawac obiektu, tylko stworzyl sie tutaj
        {
            get
            {
                if (factory_instance == null)
                {
                    lock (lockObj)
                    {
                        if (factory_instance == null)
                        {
                            factory_instance = new FunkcjaFactory();
                        }
                    }
                }
                return factory_instance;
            }
        }

        private static readonly Dictionary<string, IFunkcja> funkcje = new()
        {
            {"1", new Funkcja1() },
            {"2", new Funkcja2() },
            {"3", new Funkcja3() },
            {"4", new Funkcja4() },
        };
        public static IFunkcja GetFunkcja(string choice)
        {
            if (funkcje.TryGetValue(choice, out var funkcja))
                return funkcja;
            throw new Exception("Niepoprawny wybor funkcji");
        }


        private IFunkcja funkcja;
        private string funkcjaName;
        public IFunkcja WybranaFunkcja => funkcja;
        public string FunkcjaName => funkcjaName;
        public void ChoicePass(string choice)
        {
            if (choice == "0") { Environment.Exit(0); };

            try
            {
                funkcja = FunkcjaFactory.GetFunkcja(choice);
                funkcjaName = funkcja.GetType().Name;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Wybierz poprawna opcje");
            }
        }
    }
}
