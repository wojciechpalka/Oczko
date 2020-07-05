using System;
using System.Collections.Generic;
using System.Threading;

namespace Oczko
{
    public class Program
    {   //Inicjuje Talie i gracza
        private static Talia talia = new Talia();
        private static Gracz gracz = new Gracz();

        //Możliwe rezultaty rundy 
        private enum Rezultat
        {
            Remis,
            WygranaGracza,
            GraczWiecej22,
            GraczOczko,
            GraczPerskieOczko,
            WygranaKrupiera,
            NiewlasciwyZaklad
        }

        //Rozpoczyna grę poprzez Stworzenie Tali rozdaje karty graczowi i krupierowi oraz pokazuje je
        static void RozdajKarty()
        {
            talia.Rozpocznij();

            gracz.Reka = talia.RozdajPoczatek();
            Krupier.UkryteKarty = talia.RozdajPoczatek();
            Krupier.OdkryteKarty = new List<Karta>();            
            Krupier.OdkryjKarte();
            gracz.Wypisz();
            Krupier.Wypisz();
        }

        //Zawiera kod kontrulujący przebieg rundy 
        static void RozpocznijRunde()
        {
            Console.Clear();

            if (!Zaklad())
            {
                Koniec(Rezultat.NiewlasciwyZaklad);
                return;
            }
            Console.Clear();

            RozdajKarty();
            Akcja();

            Krupier.OdkryjKarte();

            Console.Clear();
            gracz.Wypisz();
            Krupier.Wypisz();

            gracz.Rozegrane++;
            if (Kasyno.CzyPerskieOczko(gracz.Reka))
            {
                gracz.Wygrane++;
                gracz.PerskieOczko++;
                gracz.CałkowityZakład += gracz.Zaklad;
                Koniec(Rezultat.GraczOczko);
            }

            if (gracz.PunktacjaReki() > 21)
            {
                gracz.CałkowityZakład += gracz.Zaklad;
                gracz.Przegrane22++;
                Koniec(Rezultat.GraczWiecej22);
                return;
            }

            while (Krupier.Punkty() <= 16)
            {
                Thread.Sleep(2000);
                Krupier.OdkryteKarty.Add(talia.Rozdaj());

                Console.Clear();
                gracz.Wypisz();
                Krupier.Wypisz();
            }


            if (gracz.PunktacjaReki() > Krupier.Punkty())
            {
                gracz.Wygrane++;
                if (Kasyno.CzyOczko(gracz.Reka))
                {
                    gracz.Oczko++;
                    gracz.CałkowityZakład += gracz.Zaklad;
                    Koniec(Rezultat.GraczOczko);
                }
                else
                {
                    gracz.CałkowityZakład += gracz.Zaklad;
                    Koniec(Rezultat.WygranaGracza);
                }
            }
            else if (Krupier.Punkty() > 21)
            {
                gracz.Wygrane++;
                gracz.CałkowityZakład += gracz.Zaklad;
                Koniec(Rezultat.WygranaGracza);
            }
            else if (Krupier.Punkty() > gracz.PunktacjaReki())
            {
                gracz.PrzegraneK++;
                gracz.CałkowityZakład += gracz.Zaklad;
                Koniec(Rezultat.WygranaKrupiera);                
            }
            else
            {
                gracz.CałkowityZakład += gracz.Zaklad;
                Koniec(Rezultat.Remis);
            }

        }

        //Pętla pytająca gracza o ruch aż gracz zdecyduje o koncu swojej rundy lub przekroczy limit punktów
        static void Akcja()
        {
            string akcja;
            do
            {
                Console.Clear();
                gracz.Wypisz();
                Krupier.Wypisz();

                Console.Write("Wybierz z akcji: Dobierz, Koniec, Podwój   ");
                Console.ForegroundColor = ConsoleColor.White;
                akcja = Console.ReadLine();
                Kasyno.ZresetujKolor();

                switch (akcja.ToUpper())
                {
                    case "DOBIERZ":
                        gracz.Reka.Add(talia.Rozdaj());
                        break;
                    case "KONIEC":
                        break;
                    case "PODWÓJ":
                        if (gracz.Zetony <= gracz.Zaklad)
                        {
                            gracz.DodajZaklad(gracz.Zetony);
                        }
                        else
                        {
                            gracz.DodajZaklad(gracz.Zaklad);
                        }
                        gracz.Reka.Add(talia.Rozdaj());
                        break;
                    default:
                        Console.WriteLine("Prawidłowe ruchy:");
                        Console.Write("Dobierz, Koniec, Podwój");
                        Thread.Sleep(3000);
                        break;
                }

            } while (!akcja.ToUpper().Equals("KONIEC") && !akcja.ToUpper().Equals("PODWÓJ")
                 && gracz.PunktacjaReki() <= 21);
        }

        //Pobiera zakład od gracza
        static bool Zaklad()
        {
            Console.Write("Żetony: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(gracz.Zetony);
            Kasyno.ZresetujKolor();

            Console.Write("Minimalny zakład: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Kasyno.MinimalnyZaklad);
            Kasyno.ZresetujKolor();

            Console.Write("Twój zakład: ");
            Console.ForegroundColor = ConsoleColor.White;
            string z = Console.ReadLine();
            Kasyno.ZresetujKolor();

            if (Int32.TryParse(z, out int zakład) && zakład >= Kasyno.MinimalnyZaklad && gracz.Zetony >= zakład)
            {
                gracz.DodajZaklad(zakład);
                return true;
            }
            return false;
        }

        //switch między możliwymi zakończeniami rundy oraz funkcja która może zakońcyć grę zresetować ją lub rozpocząc kolejną rundę
        static void Koniec(Rezultat rezultat)
        {
            switch (rezultat)
            {
                case Rezultat.Remis:
                    gracz.ZwrocZaklad();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Remis");
                    break;
                case Rezultat.WygranaGracza:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Gracz wygrywa " + gracz.WygranyZaklad(false) + " żetony");
                    break;
                case Rezultat.GraczWiecej22:
                    gracz.WyczyscZaklad();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Gracz przekracza limit");
                    break;
                case Rezultat.GraczOczko:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Oczko!!! gracz wygrywa " + gracz.WygranyZaklad(true));
                    break;
                case Rezultat.GraczPerskieOczko:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Perskie oczko!!! gracz wygrywa " + gracz.WygranyZaklad(true));
                    break;
                case Rezultat.WygranaKrupiera:
                    gracz.WyczyscZaklad();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Krupier wygrywa");
                    break;
                case Rezultat.NiewlasciwyZaklad:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Nieodpowiedni zakład");
                    break;
            }
            //Częśc kodu odpowiadajacy za zresetowanie statystyk w przypadku przegranej gracza
            if (gracz.Zetony <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine();
                if (gracz.Rozegrane - 1 == 1)
                {
                    Console.WriteLine("Skończyły Ci się żetony po " + (gracz.Rozegrane - 1) + " rundzie");                    
                }
                else
                {
                    Console.WriteLine("Skończyły Ci się żetony po " + (gracz.Rozegrane - 1) + " rundach");
                }
                if (gracz.Wygrane == 0)
                {
                    Console.WriteLine("Wygrałeś 0 razy");
                }
                else if (gracz.Wygrane == 1)
                {
                    Console.WriteLine("Wygrałeś 1 raz");
                }
                else
                {
                    Console.WriteLine("Wygrałeś " + (gracz.Wygrane) + " razy");

                    if(gracz.Oczko == 1)
                    {
                        Console.WriteLine("Miałeś oczko 1 raz");
                    }
                    else if (gracz.Oczko > 1)
                    {
                        Console.WriteLine("Miałeś oczko " + (gracz.Oczko) + " razy");
                    }

                    if (gracz.PerskieOczko == 1)
                    {
                        Console.WriteLine("Miałeś perskie oczko 1 raz");
                    }
                    else if (gracz.Oczko > 1)
                    {
                        Console.WriteLine("Miałeś perskie oczko " + (gracz.Oczko) + " razy");
                    }
                }
                if(gracz.Przegrane22 > gracz.PrzegraneK)
                {
                    Console.WriteLine("Przegrywałeś częsciej przez zbyt wysoką liczbę punktów. Postaraj się grać bardziej ostrożnie");
                }
                else if(gracz.Przegrane22 < gracz.PrzegraneK)
                {
                    Console.WriteLine("Przegrywałeś częsciej przez przewagę punktową krupiera. Postaraj się grać bardziej odważnie");
                }
                else
                {
                    Console.WriteLine("Przegrywałeś porówno przez przewagę punktową krupiera jak i zbyt wysoką liczbę punktów.");
                    Console.WriteLine("Możliwe że po prostu miałeś pecha.");

                }
                Console.WriteLine("Twój stanardowy zakład wynosił " + (gracz.CałkowityZakład)/(gracz.Rozegrane - 1) + " żetonów");

                Console.WriteLine("100 żetonów zostało dodanych na Twoje konto a statystyki zostały wyzerowane");

                gracz = new Gracz();
            }
            //Częśc kodu odpowiadajaca za zakończenie gry w przypadku wygranej gracza
            if (gracz.Zetony >= 200)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine();
                if (gracz.Rozegrane - 1 == 1)
                {
                    Console.WriteLine("Gratulacje !!! Wygrałeś po " + (gracz.Rozegrane - 1) + " rundzie");
                }

                else
                { 
                    Console.WriteLine("Gratulacje !!! Wygrałeś po " + (gracz.Rozegrane - 1) + " rundach"); 
                }               
                
                if (gracz.Wygrane != 1)
                {
                    Console.WriteLine("Wygrałeś " + (gracz.Wygrane) + " razy");

                    if (gracz.Oczko == 1)
                    {
                        Console.WriteLine("Miałeś oczko 1 raz");
                    }
                    else if (gracz.Oczko > 1)
                    {
                        Console.WriteLine("Miałeś oczko " + (gracz.Oczko) + " razy");
                    }

                    if (gracz.PerskieOczko == 1)
                    {
                        Console.WriteLine("Miałeś perskie oczko 1 raz");
                    }
                    else if (gracz.Oczko > 1)
                    {
                        Console.WriteLine("Miałeś perskie oczko " + (gracz.Oczko) + " razy");
                    }
                }
                if (gracz.Przegrane22 > gracz.PrzegraneK)
                {
                    Console.WriteLine("Przegrywałeś częsciej przez zbyt wysoką liczbę punktów. Postaraj się grać bardziej ostrożnie");
                }
                else if (gracz.Przegrane22 < gracz.PrzegraneK)
                {
                    Console.WriteLine("Przegrywałeś częsciej przez przewagę punktową krupiera. Postaraj się grać bardziej odważnie");
                }
                else if (gracz.Przegrane22 != 0 && gracz.PrzegraneK != 0)
                {
                    Console.WriteLine("Przegrywałeś porówno przez przewagę punktową krupiera jak i zbyt wysoką liczbę punktów.");
                    Console.WriteLine("Możliwe że po prostu miałeś pecha.");

                }
                Console.WriteLine("Twój stanardowy zakład wynosił " + (gracz.CałkowityZakład) / (gracz.Rozegrane - 1) + " żetonów");
                Console.WriteLine("Nacisnij jakikolwiek przycisk aby zamknąć aplikacje");
                Console.ReadKey();
                Environment.Exit(1);
            }
            //Część kodu odpowiadająca za rozpoczęcie kolejnej rundy jeśli oba poprzednie warunki nie są spełnione
            Kasyno.ZresetujKolor();
            Console.WriteLine("Nacisnij jakikolwiek przycisk aby kontynuować");
            Console.ReadKey();
            RozpocznijRunde();
        }
        //Odpowiada za rozpoczęcie pierwszej rundy oraz wydrukowanie zasad gry
        static void Main(string[] args)
        {
            Kasyno.ZresetujKolor();
            Console.WriteLine("Witaj w grze Oczko. Twoim celem jest podwoić swoją stawkę żetonów. W tym celu muisz znaleźć się jak najbliżej 21 punktów ale pamiętaj osiągnięcię 22 lub więcej punktów bedzię skótkowało natychmiastową przegraną rundy.");
            Console.WriteLine("");
            Console.WriteLine("W grze mamy 1 specjalną kombinacje jest to perskie oczko są nim 2 asy na ręcę tuż po rozdaniu. W tym wypadku Twoja suma punktów wyniesie 22 ale mimo to wygrasz rundę jedyne co musisz zrobić to nie dobierać więcej kart");
            Console.WriteLine("");
            Console.WriteLine("Punktacja kart: As= 11, Król = 4, Królowa = 3, Dupek = 2 reszta kart ma puktację zgodną z wartością na karcie np dziewięć = 9");
            Console.WriteLine("");
            Console.WriteLine("W grze masz możliwość wykonania 3 ruchów:");
            Console.WriteLine("1 - Dobierz - po prostu dobierasz kolejną kartę, 2 - Koniec zakańczasz dobieranie następnie dobiera krupier osoba bliżej granicy 21 punktów wygrywa, 3 Podwój - Podwajasz swój początkowy zakład i dobierasz jedną ostatnią kartę");
            Console.WriteLine("");
            Console.WriteLine("Krupier nie bedzię dobierał jeśli ma więcej niż 16 punktów");
            Console.WriteLine("");
            Console.WriteLine("Gra zakończy się jeśli na Twoim koncie będzie minimum 200 żetonów. W przypadku kiedy przegrasz wszystkie żetony gra zostanie automatycznie zresetowana.");
            Console.WriteLine("");
            Console.WriteLine("Powodzenia");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Nacisnij jakikolwiek przycisk aby kontynuować");
            Console.ReadKey();
            RozpocznijRunde();
        }
    }
}
