using System;
using System.Collections.Generic;

namespace SystemRezerwacji
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Seans> seanse = new List<Seans>
            {
                new Seans(new Film("Diuna: Czesc Druga", 166, 28.00m), "18:00"),
                new Seans(new Film("Matrix", 136, 22.00m), "20:30"),
                new Seans(new Film("Incepcja", 148, 25.00m), "21:15")
            };

            List<Rezerwacja> rezerwacje = new List<Rezerwacja>();
            bool dziala = true;

            while (dziala)
            {
                Console.Clear();
                Console.WriteLine("System rezerwacji miejsc w kinie");
                Console.WriteLine("1. Wyswietl repertuar");
                Console.WriteLine("2. Pokaz uklad sali");
                Console.WriteLine("3. Zarezerwuj bilet");
                Console.WriteLine("4. Lista rezerwacji");
                Console.WriteLine("0. Wyjscie");
                Console.Write("\nWybierz opcje: ");

                string wybor = Console.ReadLine() ?? "";

                switch (wybor)
                {
                    case "1":
                        WyswietlRepertuar(seanse);
                        break;

                    case "2":
                        Seans? sPokaz = WybierzSeans(seanse);
                        if (sPokaz != null)
                        {
                            sPokaz.WyswietlSale();
                        }
                        break;

                    case "3":
                        Seans? sRez = WybierzSeans(seanse);
                        if (sRez != null)
                        {
                            sRez.WyswietlSale();

                            Console.Write("Podaj imie klienta: ");
                            string imie = Console.ReadLine() ?? "";
                            Console.Write("Podaj email klienta: ");
                            string email = Console.ReadLine() ?? "";

                            Console.Write("Podaj numer rzedu: ");
                            bool okRzad = int.TryParse(Console.ReadLine(), out int rzad);
                            Console.Write("Podaj numer miejsca: ");
                            bool okMiejsce = int.TryParse(Console.ReadLine(), out int miejsce);

                            if (okRzad && okMiejsce && sRez.ZarezerwujMiejsce(rzad, miejsce))
                            {
                                Console.WriteLine("Wybierz typ biletu: 1 - Normalny / 2 - Ulgowy");
                                string typ = Console.ReadLine() ?? "";

                                Bilet bilet = typ == "2"
                                    ? new BiletUlgowy(rzad, miejsce, sRez.Film.CenaBazowa)
                                    : new BiletNormalny(rzad, miejsce, sRez.Film.CenaBazowa);

                                Klient klient = new Klient(imie, email);
                                Rezerwacja rezerwacja = new Rezerwacja(klient, sRez);
                                rezerwacja.Bilety.Add(bilet);
                                rezerwacje.Add(rezerwacja);

                                Console.WriteLine("\nZarezerwowano pomyslnie:");
                                Console.WriteLine(bilet.PobierzOpis());
                            }
                            else
                            {
                                Console.WriteLine("\nBlad: Miejsce jest zajete lub podano zle dane.");
                            }
                        }
                        break;

                    case "4":
                        Console.WriteLine("\nAktualne rezerwacje:");
                        if (rezerwacje.Count == 0)
                        {
                            Console.WriteLine("Brak rezerwacji.");
                        }
                        else
                        {
                            foreach (var r in rezerwacje)
                            {
                                Console.WriteLine(r.PobierzOpis());
                                foreach (var b in r.Bilety)
                                {
                                    Console.WriteLine("  - " + b.PobierzOpis());
                                }
                            }
                        }
                        break;

                    case "0":
                        dziala = false;
                        break;

                    default:
                        Console.WriteLine("Nieznana opcja.");
                        break;
                }

                if (dziala)
                {
                    Console.WriteLine("\nWcisnij klawisz.");
                    Console.ReadKey();
                }
            }
        }

        static void WyswietlRepertuar(List<Seans> seanse)
        {
            Console.WriteLine("\nAktualny repertuar:");
            for (int i = 0; i < seanse.Count; i++)
            {
                var s = seanse[i];
                Console.WriteLine($"{i + 1}. {s.Film.Tytul},  Godz: {s.Godzina},  Cena: {s.Film.CenaBazowa}zl,  Wolne: {s.PoliczWolneMiejsca()}");
            }
        }

        static Seans? WybierzSeans(List<Seans> seanse)
        {
            WyswietlRepertuar(seanse);
            Console.Write("\nWybierz numer seansu: ");
            if (int.TryParse(Console.ReadLine(), out int nr) && nr >= 1 && nr <= seanse.Count)
            {
                return seanse[nr - 1];
            }
            Console.WriteLine("Nieprawidlowy numer.");
            return null;
        }
    }
}