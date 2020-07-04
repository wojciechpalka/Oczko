using System;
using static Oczko.Awers;


namespace Oczko
{
    /// Możliwe Kolory
    public enum Kolor
    {
        Kier,
        Pik,
        Karo,
        Trefl
    }
    /// Możliwe wartości
    public enum Awers
    {
        As,
        Dwa,
        Trzy,
        Cztery,
        Pieć,
        Sześć,
        Siedem,
        Osiem,
        Dziewięć,
        Dziesięć,
        Dupek,
        Królowa,
        Król
    }

    public class Karta
    {
        public Awers Awers { get; }
        public Kolor Kolor { get; }
        public int Wartosc { get; set; }

        /// Incjacja Koloru i Wartości

        public Karta(Kolor kolor, Awers awers)
        {
            Kolor = kolor;
            Awers = awers;

            switch (Awers)
            {
                case Dupek:
                case Dwa:
                    Wartosc = 2;
                    break;
                case Królowa:
                case Trzy:
                    Wartosc = 3;
                    break;
                case Król:
                case Cztery:
                    Wartosc = 4;
                    break;
                case Pieć:
                    Wartosc = 5;
                    break;
                case Sześć:
                    Wartosc = 6;
                    break;
                case Siedem:
                    Wartosc = 7;
                    break;
                case Osiem:
                    Wartosc = 8;
                    break;
                case Dziewięć:
                    Wartosc = 9;
                    break;
                case Dziesięć:
                    Wartosc = 10;
                    break;
                case As:
                    Wartosc = 11;
                    break;

            }

        }
        /// Wydruk opisu karty
        public void WypiszKarty()
        {
            Console.WriteLine(Awers + " " + Kolor);
        }

    }
}
