using System;

namespace SystemRezerwacji
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] repertuar = { "Diuna: Czesc Druga", "Matrix", "Incepcja" };
            decimal cenaBiletu = 25.00m;

            bool[][][] saleKinowe = new bool[repertuar.Length][][];
            int[] wolneMiejsca = new int[repertuar.Length];

            for (int i = 0; i < repertuar.Length; i++)
            {
                saleKinowe[i] = UtworzSale(out wolneMiejsca[i]);
            }

            decimal utargCalkowity = 0m;
            bool dziala = true;

            while (dziala)
            {
                Console.Clear();
                Console.WriteLine("=== SYSTEM REZERWACJI KINA ===");
                Console.WriteLine($"Utarg calkowity: {utargCalkowity} PLN\n");
                Console.WriteLine("1. Wyswietl repertuar i dostepne miejsca");
                Console.WriteLine("2. Pokaz uklad sali");
                Console.WriteLine("3. Zarezerwuj pojedyncze miejsce");
                Console.WriteLine("4. Zarezerwuj kilka miejsc w rzedzie");
                Console.WriteLine("0. Wyjscie");
                Console.Write("\nWybierz opcje: ");

                string wybor = Console.ReadLine();

                switch (wybor)
                {
                    case "1":
                        WyswietlRepertuar(repertuar, wolneMiejsca);
                        break;

                    case "2":
                        int idPokaz = WybierzFilm(repertuar);
                        if (idPokaz != -1)
                        {
                            WyswietlSale(saleKinowe[idPokaz], repertuar[idPokaz]);
                        }
                        break;

                    case "3":
                        int idRez = WybierzFilm(repertuar);
                        if (idRez != -1)
                        {
                            WyswietlSale(saleKinowe[idRez], repertuar[idRez]);
                            Console.Write("Podaj numer rzedu: ");
                            bool poprawnyRzad = int.TryParse(Console.ReadLine(), out int rzad);
                            Console.Write("Podaj numer miejsca: ");
                            bool poprawneMiejsce = int.TryParse(Console.ReadLine(), out int miejsce);

                            if (poprawnyRzad && poprawneMiejsce)
                            {
                                bool sukces = Zarezerwuj(saleKinowe[idRez], rzad - 1, miejsce - 1, ref wolneMiejsca[idRez], ref utargCalkowity, cenaBiletu);
                                if (sukces)
                                    Console.WriteLine("Pomyslnie zarezerwowano miejsce.");
                                else
                                    Console.WriteLine("Blad: Miejsce zajete lub nieprawidlowe dane.");
                            }
                            else
                            {
                                Console.WriteLine("Blad wprowadzania danych.");
                            }
                        }
                        break;

                    case "4":
                        int idGrupa = WybierzFilm(repertuar);
                        if (idGrupa != -1)
                        {
                            WyswietlSale(saleKinowe[idGrupa], repertuar[idGrupa]);
                            Console.Write("Podaj numer rzedu: ");
                            if (int.TryParse(Console.ReadLine(), out int rzadGrupa))
                            {
                                Console.Write("Podaj numery miejsc po przecinku (np. 1,2,3): ");
                                string wejscie = Console.ReadLine();
                                int[] wybraneMiejsca = ParsujMiejsca(wejscie);

                                if (wybraneMiejsca.Length > 0)
                                {
                                    bool sukces = Zarezerwuj(saleKinowe[idGrupa], rzadGrupa - 1, wybraneMiejsca, ref wolneMiejsca[idGrupa], ref utargCalkowity, cenaBiletu);
                                    if (sukces)
                                        Console.WriteLine("Zarezerwowano wybrane miejsca.");
                                    else
                                        Console.WriteLine("Blad: Przynajmniej jedno z miejsc jest niedostepne.");
                                }
                                else
                                {
                                    Console.WriteLine("Nie podano poprawnych miejsc.");
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
                    Console.WriteLine("\nNacisnij dowolny klawisz, aby kontynuowac...");
                    Console.ReadKey();
                }
            }
        }

        static bool[][] UtworzSale(out int liczbaMiejsc)
        {
            bool[][] sala = new bool[5][];
            sala[0] = new bool[6];
            sala[1] = new bool[8];
            sala[2] = new bool[10];
            sala[3] = new bool[10];
            sala[4] = new bool[12];

            liczbaMiejsc = 0;
            for (int i = 0; i < sala.Length; i++)
            {
                liczbaMiejsc += sala[i].Length;
            }

            return sala;
        }

        static void WyswietlRepertuar(string[] filmy, int[] wolne)
        {
            Console.WriteLine("\nAktualny repertuar:");
            for (int i = 0; i < filmy.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {filmy[i]} (Wolne miejsca: {wolne[i]})");
            }
        }

        static int WybierzFilm(string[] filmy)
        {
            WyswietlRepertuar(filmy, new int[filmy.Length]);
            Console.Write("\nWybierz numer filmu: ");
            if (int.TryParse(Console.ReadLine(), out int nr) && nr >= 1 && nr <= filmy.Length)
            {
                return nr - 1;
            }
            Console.WriteLine("Nieprawidlowy numer filmu.");
            return -1;
        }

        static void WyswietlSale(bool[][] sala, string tytul)
        {
            Console.WriteLine($"\nSala dla filmu: {tytul}");
            Console.WriteLine("       [ EKRAN ]       \n");

            for (int i = 0; i < sala.Length; i++)
            {
                Console.Write($"Rzad {i + 1}: ");
                for (int j = 0; j < sala[i].Length; j++)
                {
                    if (sala[i][j])
                        Console.Write("[X] ");
                    else
                        Console.Write($"[{j + 1}] ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static bool Zarezerwuj(bool[][] sala, int rzad, int miejsce, ref int wolneMiejsca, ref decimal utarg, decimal cena)
        {
            if (rzad < 0 || rzad >= sala.Length)
                return false;

            if (miejsce < 0 || miejsce >= sala[rzad].Length)
                return false;

            if (sala[rzad][miejsce])
                return false;

            sala[rzad][miejsce] = true;
            wolneMiejsca--;
            utarg += cena;
            return true;
        }

        static bool Zarezerwuj(bool[][] sala, int rzad, int[] miejsca, ref int wolneMiejsca, ref decimal utarg, decimal cena)
        {
            if (rzad < 0 || rzad >= sala.Length)
                return false;

            for (int i = 0; i < miejsca.Length; i++)
            {
                int m = miejsca[i] - 1;
                if (m < 0 || m >= sala[rzad].Length || sala[rzad][m])
                {
                    return false;
                }
            }

            for (int i = 0; i < miejsca.Length; i++)
            {
                int m = miejsca[i] - 1;
                sala[rzad][m] = true;
                wolneMiejsca--;
                utarg += cena;
            }

            return true;
        }

        static int[] ParsujMiejsca(string tekst)
        {
            if (string.IsNullOrWhiteSpace(tekst))
                return Array.Empty<int>();

            string[] czesci = tekst.Split(',');
            int[] wynik = new int[czesci.Length];

            for (int i = 0; i < czesci.Length; i++)
            {
                if (!int.TryParse(czesci[i].Trim(), out wynik[i]))
                {
                    return Array.Empty<int>();
                }
            }
            return wynik;
        }
    }
}