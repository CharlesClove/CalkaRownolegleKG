using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalkaRownolegleKG.Interfejsy;

namespace CalkaRownolegleKG.Funkcje
{
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
                if(zbiorParametrow_instance == null)
                {
                    zbiorParametrow_instance = new ZbiorParametrow();
                }
                return zbiorParametrow_instance;
            }
        }
        public void ZbierzParametry()
        {
            Console.WriteLine("\nPodaj ilość podziałów [ile całek chcesz obliczyć]: ");
            //parametry.podzialy = int.Parse(Console.ReadLine());
            parametry.podzialy = 1;
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
    
}
