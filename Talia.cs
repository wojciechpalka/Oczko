using System;
using System.Collections.Generic;

namespace Oczko
{
    public class Talia
    {
        private List<Karta> karty;
        //Zwraca potasowaną Talie
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
        public List<Karta> RozdajPoczatek()
        {
            List<Karta> karty_na_rence = new List<Karta>();
            karty_na_rence.Add(karty[0]);
            karty_na_rence.Add(karty[1]);

            karty.RemoveRange(0, 2);
            return karty_na_rence;
        }


        public Karta Rozdaj()
        {
            Karta karta = karty[0];
            karty.Remove(karta);

            return karta;
        }
