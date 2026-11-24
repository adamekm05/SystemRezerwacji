using System;
using System.Collections.Generic;

namespace SystemRezerwacji
{
    public class Film
    {
        public string Tytul { get; set; }
        public int CzasTrwaniaMin { get; set; }
        public decimal CenaBazowa { get; set; }

        public Film(string tytul, int czasTrwaniaMin, decimal cenaBazowa)
        {
            Tytul = tytul;
            CzasTrwaniaMin = czasTrwaniaMin;
            CenaBazowa = cenaBazowa;
        }
    }

    public class Klient
    {
        public string Imie { get; set; }
        public string Email { get; set; }

        public Klient(string imie, string email)
        {
            Imie = imie;
            Email = email;
        }
    }

    public class Seans
    {
        public Film Film { get; set; }
        public string Godzina { get; set; }
        public bool[][] Sala { get; private set; }

        public Seans(Film film, string godzina)
        {
            Film = film;
            Godzina = godzina;
            Sala = new bool[5][];
            Sala[0] = new bool[6];
            Sala[1] = new bool[8];
            Sala[2] = new bool[10];
            Sala[3] = new bool[10];
            Sala[4] = new bool[12];
        }

        public int PoliczWolneMiejsca()
        {
            int wolne = 0;
            for (int i = 0; i < Sala.Length; i++)
            {
                for (int j = 0; j < Sala[i].Length; j++)
                {
                    if (!Sala[i][j]) wolne++;
                }
            }
            return wolne;
        }

        public bool ZarezerwujMiejsce(int rzad, int miejsce)
        {
            int r = rzad - 1;
            int m = miejsce - 1;

            if (r < 0 || r >= Sala.Length) return false;
            if (m < 0 || m >= Sala[r].Length) return false;
            if (Sala[r][m]) return false;

            Sala[r][m] = true;
            return true;
        }

        public void WyswietlSale()
        {
            Console.WriteLine($"\nSeans: {Film.Tytul} ({Godzina})");
            Console.WriteLine("       [ EKRAN ]       \n");

            for (int i = 0; i < Sala.Length; i++)
            {
                Console.Write($"Rzad {i + 1}: ");
                for (int j = 0; j < Sala[i].Length; j++)
                {
                    if (Sala[i][j])
                        Console.Write("[X] ");
                    else
                        Console.Write($"[{j + 1}] ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    public class Rezerwacja : IPodsumowanie
    {
        public Klient Klient { get; set; }
        public Seans Seans { get; set; }
        public List<Bilet> Bilety { get; set; }

        public Rezerwacja(Klient klient, Seans seans)
        {
            Klient = klient;
            Seans = seans;
            Bilety = new List<Bilet>();
        }

        public decimal ObliczLacznaKwote()
        {
            decimal suma = 0m;
            foreach (var bilet in Bilety)
            {
                suma += bilet.ObliczCene();
            }
            return suma;
        }

        public string PobierzOpis()
        {
            return $"Klient: {Klient.Imie} ({Klient.Email}),  Film: {Seans.Film.Tytul} ({Seans.Godzina}),  Biletow: {Bilety.Count},  Suma: {ObliczLacznaKwote()} PLN";
        }
    }
}
