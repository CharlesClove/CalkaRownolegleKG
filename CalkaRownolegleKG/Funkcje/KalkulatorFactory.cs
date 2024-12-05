using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalkaRownolegleKG.Interfejsy;
using CalkaRownolegleKG.Kalkulatory;

namespace CalkaRownolegleKG.Funkcje
{
    public class KalkulatorFactory
    {
        private readonly Dictionary<string, Func<ParametryDoCalki, IFunkcja, (List<(int, double)>, bool)>> _metodyWielowatkowe;
        private readonly Dictionary<string, string> _mapowanieNumeru; 
        //klucz to string, czyli metoda, przyjmuje do funkcji parametry i funkcje i zwraca tuple int+double i bool

        public KalkulatorFactory(KalkulatoryCalek kalkulatoryCalek) //Fabryka nazw metod
        {
            _mapowanieNumeru = new Dictionary<string, string> //dodalem mape, zeby w menu mozna wpisac np 1 lub parallel, 
            {
                {"1","parallel" },
                {"2","thread"},
                {"3","threadpool"}
            };
            _metodyWielowatkowe = new Dictionary<string, Func<ParametryDoCalki, IFunkcja, (List<(int, double)>, bool)>>()
            {
                {"parallel", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu_Parallel(parametry,funkcja) },
                {"thread", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu_Thread(parametry,funkcja) },
                {"threadpool", (parametry, funkcja) => kalkulatoryCalek.metodaTrapezu_Threadpool(parametry,funkcja) },
            };        
        }
        public Func<ParametryDoCalki, IFunkcja, (List<(int, double)>,bool)> GetMetoda(string metoda)
        {
            if (_mapowanieNumeru.ContainsKey(metoda)) // sprawdza metody po numerach
            {
                metoda = _mapowanieNumeru[metoda];
            }

            if (!_metodyWielowatkowe.ContainsKey(metoda.ToLower())){
                throw new ArgumentException($"nieznana metoda: {metoda}"); //jezeli metody nie ma w fabryce zwraca blad
            }
            return _metodyWielowatkowe[metoda.ToLower()]; //zwracanie metody, aby poprawnie uzyc fabryki, dałem tolower zeby string sie zgadzal
        }
    }
}
