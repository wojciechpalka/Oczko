using System;
using System.Collections.Generic;

namespace Oczko
{
    public class Talia
    {
        private List<Karta> karty;
        //Zwraca potasowaną talie
        public List<Karta> GetZimnaTalia()
        {
            List<Karta> zimnaTalia = new List<Karta>();

            for (int n = 0; n < 4; n++)
            {
                for (int m = 0; m < 13; m++)
                {
                    zimnaTalia.Add(new Karta((Kolor)n, (Awers)m));
                }
            }

            return zimnaTalia;
        }
        //Usuwa 2 karty z talii kart i zwraca je w formie listy
        public List<Karta> RozdajPoczatek()
        {
            List<Karta> karty_na_rence = new List<Karta>();
            karty_na_rence.Add(karty[0]);
            karty_na_rence.Add(karty[1]);
            //usuwa karty z tali
            karty.RemoveRange(0, 2);
            return karty_na_rence;
        }
        //Usuwa kartę z talii kart i ją zwraca
        public Karta Rozdaj()
        {
            Karta karta = karty[0];
            karty.Remove(karta);

            return karta;
        }
        //Tasuje karty
        public void Tasuj()
        {
            Random rng = new Random();

            int n = karty.Count;
            while (n > 1)
            {
                n--;
                int i = rng.Next(n + 1);
                Karta karta = karty[i];
                karty[i] = karty[n];
                karty[n] = karta;
            }
        }
        //Zamienia karty na nową talie i ją tasuje
        public void Rozpocznij()
        {
            karty = GetZimnaTalia();
            Tasuj();
        }
    }
}
