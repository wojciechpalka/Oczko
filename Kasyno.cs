using System;
using System.Collections.Generic;

namespace Oczko
{
    class Kasyno
    {
        public static int MinimalnyZaklad { get; } = 5;

        
        //Zwraca wartość true jeśli gracz ma oczko
        public static bool CzyOczko(List<Karta> karty_na_rence)
        {
            if (karty_na_rence.Count == 2)
            {
                if (karty_na_rence[0].Awers == Awers.As && karty_na_rence[1].Wartosc == 10) return true;
                else if (karty_na_rence[1].Awers == Awers.As && karty_na_rence[0].Wartosc == 10) return true;
            }
            return false;
        }
        //Zwraca wartość true jeśli gracz ma perskie oczko
        public static bool CzyPerskieOczko(List<Karta> karty_na_rence)
        {
            if (karty_na_rence.Count == 2)
            {  
                if (karty_na_rence[1].Awers == Awers.As && karty_na_rence[0].Awers == Awers.As) return true;
            }
            return false;
        }

        //Zamień kolory na podstawowe 
        public static void ZresetujKolor()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
    //Tworzy gracza razem z jego początkowymi statystykami
    public class Gracz
    {
        public int Zetony { get; set; } = 100;
        public int Zaklad { get; set; }
        public int Wygrane { get; set; }
        public int Rozegrane { get; set; } = 1;
        public int Przegrane22 { get; set; }
        public int PrzegraneK { get; set; }
        public int CałkowityZakład { get; set; }
        public int Oczko { get; set; }
        public int PerskieOczko { get; set; }

        public List<Karta> Reka { get; set; }

        //Odejmuje wartość zakładu od zetonów gracza a dodaje do puli zakładu
        public void DodajZaklad(int zaklad)
        {
            Zaklad += zaklad;
            Zetony -= zaklad;
        }

        //Po grze przywróć wartość zakładu do zera
        public void WyczyscZaklad()
        {
            Zaklad = 0;
        }

        //W przypadku remisu użyj tej funkcji w celu zwróceniu zakładu
        public void ZwrocZaklad()
        {
            Zetony += Zaklad;
            WyczyscZaklad();
        }

        //W zależności od typu wygranej przydiel odpowiednią nagrodę a następnie wyczyść zakład
        public int WygranyZaklad(bool oczko)
        {
            int WygraneZetony;
            if (oczko)
            {
                WygraneZetony = (int)Math.Floor(Zaklad * 3.0);
            }
            else
            {
                WygraneZetony = Zaklad * 2;
            }

            Zetony += WygraneZetony;
            WyczyscZaklad();
            return WygraneZetony;
        }

        //Sprawdź wartość punktową ręki gracza
        public int PunktacjaReki()
        {
            int wartosc = 0;
            foreach (Karta karta in Reka)
            {
                wartosc += karta.Wartosc;
            }
            return wartosc;
        }

        //Wypisz interfejs gracza
        public void Wypisz()
        {
            // Wypisz wartości "stałe": ilość żetonów, wysokość zakładu itp  
            Console.Write("Zakład: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Zaklad + "  ");
            Kasyno.ZresetujKolor();
            Console.Write("Żetony: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Zetony + "  ");
            Kasyno.ZresetujKolor();
            Console.Write("Wygrane: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Wygrane);
            Kasyno.ZresetujKolor();
            Console.WriteLine("Runda " + Rozegrane);
            //Wypisz Karty na rence gracza
            Console.WriteLine();
            Console.WriteLine("Twoja punktacja (" + PunktacjaReki() + "):");

            foreach (Karta karta in Reka)
            {
                karta.WypiszKarty();
            }

            Console.WriteLine();
        }
    }

    public class Krupier
    {
        public static List<Karta> UkryteKarty { get; set; } = new List<Karta>();
        public static List<Karta> OdkryteKarty { get; set; } = new List<Karta>();

        //Wymienie pierwszą karte z listy kart ukrytych i dodaję ją do listy kart odkrytych
        public static void OdkryjKarte()
        {
            OdkryteKarty.Add(UkryteKarty[0]);
            UkryteKarty.RemoveAt(0);
        }

        //Wartość punktowa kart krupiera w odkrytych kartach
        public static int Punkty()
        {
            int punkty = 0;
            foreach (Karta karta in OdkryteKarty)
            {
                punkty += karta.Wartosc;
            }
            return punkty;
        }

        //Wypisz karty krupiera dla kart ukrytych zamiast opisu wypisz "???"
        public static void Wypisz()
        {
            Console.WriteLine("Punktacja Krupiera (" + Punkty() + "):");

            foreach (Karta karta in OdkryteKarty)
            {
                karta.WypiszKarty();
            }

            for (int i = 0; i < UkryteKarty.Count; i++)
            {
                Console.WriteLine("???");
            }
            Console.WriteLine();
        }
    }
}
