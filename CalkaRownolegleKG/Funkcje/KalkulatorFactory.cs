//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CalkaRownolegleKG.Interfejsy;
//using CalkaRownolegleKG.Kalkulatory;

//namespace CalkaRownolegleKG.Funkcje
//{
//    public class KalkulatorFactory
//    {
//        private readonly Dictionary<string, Func<ParametryDoCalki, IFunkcja, List<(int, double)>>> _metodyWielowatkowe;
        
//        //klucz to string, czyli metoda, przyjmuje do funkcji parametry i funkcje i zwraca tuple int+double i bool

//        public KalkulatorFactory(KalkulatoryCalek kalkulatoryCalek) //Fabryka nazw metod
//        {
            
//            _metodyWielowatkowe = new Dictionary<string, Func<ParametryDoCalki, IFunkcja, List<(int, double)>>>()
//            {
//                {"parallel", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu(parametry,funkcja) },
//                {"1", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu(parametry,funkcja) },

//                {"thread", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu(parametry,funkcja) },
//                {"2", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu(parametry,funkcja) },

//                {"threadpool", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu(parametry,funkcja) },
//                {"3", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu(parametry,funkcja) },

//                {"task", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu(parametry,funkcja) },
//                {"4", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu(parametry,funkcja) },
//            };
//        }
//        public Func<ParametryDoCalki, IFunkcja, List<(int, double)>> WybranaMetoda(string metoda)
//        {
//            if (_metodyWielowatkowe.TryGetValue(metoda.ToLower(), out var metodaWybor))//??
//            {
//                return metodaWybor;
//            }
//            throw new ArgumentException($"Nieznana metoda: {metoda}");
//        }
//    }
//}
