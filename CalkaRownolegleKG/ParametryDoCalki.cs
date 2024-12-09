namespace CalkaRownolegleKG
{
    public class ParametryDoCalki
    {
        public int podzialy { get; set; }
        public List<(int poczatek,int koniec)> ZakresyCalki { get; set; }
        public ParametryDoCalki()
        {
            ZakresyCalki = new List<(int,int)>();
        }

    }
}
