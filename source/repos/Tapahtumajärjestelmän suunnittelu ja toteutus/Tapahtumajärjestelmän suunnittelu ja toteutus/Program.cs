using System;
using System.Collections.Generic;

public class Tapahtuma
{
    private List<Osallistuja> osallistujat;
    private Queue<Osallistuja> puheenvuorojono; // Puheenvuorojono FIFO-periaatteella
    private int maxPaikkoja;

    public Tapahtuma(int maxPaikkoja)
    {
        this.maxPaikkoja = maxPaikkoja;
        osallistujat = new List<Osallistuja>();
        puheenvuorojono = new Queue<Osallistuja>(); // Alustetaan jono puheenvuoroille
    }

    public class Osallistuja
    {
        public string Nimi { get; set; }
        public string Yhteystiedot { get; set; }

        public Osallistuja(string nimi, string yhteystiedot)
        {
            Nimi = nimi;
            Yhteystiedot = yhteystiedot;
        }

        public override string ToString()
        {
            return $"{Nimi}, {Yhteystiedot}";
        }
    }

    public bool LisaaOsallistuja(string nimi, string yhteystiedot, bool haluaaPuheenvuoron)
    {
        if (osallistujat.Exists(o => o.Nimi == nimi))
        {
            Console.WriteLine($"Osallistuja nimellä {nimi} on jo ilmoittautunut.");
            return false;
        }

        if (osallistujat.Count >= maxPaikkoja)
        {
            Console.WriteLine("Tapahtuma on täynnä.");
            return false;
        }

        var osallistuja = new Osallistuja(nimi, yhteystiedot);
        osallistujat.Add(osallistuja);
        Console.WriteLine($"Osallistuja {nimi} lisätty.");

        if (haluaaPuheenvuoron)
        {
            puheenvuorojono.Enqueue(osallistuja);
            Console.WriteLine($"{nimi} on lisätty puheenvuorojonoon.");
        }

        return true;
    }

    public bool PoistaIlmoittautuminen(string nimi)
    {
        var osallistuja = osallistujat.Find(o => o.Nimi == nimi);
        if (osallistuja != null)
        {
            osallistujat.Remove(osallistuja);
            Console.WriteLine($"Osallistuja {nimi} poistettu.");

            // Poistetaan osallistuja myös puheenvuorojonosta, jos hän on siellä
            var puheenvuorotemp = new Queue<Osallistuja>();
            while (puheenvuorojono.Count > 0)
            {
                var jonossa = puheenvuorojono.Dequeue();
                if (jonossa.Nimi != nimi) puheenvuorotemp.Enqueue(jonossa);
            }
            puheenvuorojono = puheenvuorotemp;

            return true;
        }
        Console.WriteLine("Osallistujaa ei löytynyt.");
        return false;
    }

    public void NaytaKaikkiIlmoittautuneet()
    {
        if (osallistujat.Count == 0)
        {
            Console.WriteLine("Ei ilmoittautuneita.");
            return;
        }
        Console.WriteLine("Ilmoittautuneet:");
        foreach (var osallistuja in osallistujat)
        {
            Console.WriteLine(osallistuja);
        }
    }

    public void NaytaSeuraavaPuheenvuoro()
    {
        if (puheenvuorojono.Count > 0)
        {
            var seuraava = puheenvuorojono.Peek();
            Console.WriteLine($"Seuraava puhuja on: {seuraava.Nimi}");
        }
        else
        {
            Console.WriteLine("Ei seuraavia puhujia.");
        }
    }

    public void PoistaPuheenvuorosta()
    {
        if (puheenvuorojono.Count > 0)
        {
            var poistettu = puheenvuorojono.Dequeue();
            Console.WriteLine($"Puheenvuoro käytetty. {poistettu.Nimi} poistettu jonosta.");
        }
        else
        {
            Console.WriteLine("Ei puhujia jonossa.");
        }
    }
}

public class Program
{
    public static void Main()
    {
        Tapahtuma tapahtuma = new Tapahtuma(50);

        while (true)
        {
            Console.WriteLine("\nValitse toiminto:");
            Console.WriteLine("1: Lisää osallistuja");
            Console.WriteLine("2: Poista osallistuja");
            Console.WriteLine("3: Näytä kaikki osallistujat");
            Console.WriteLine("4: Näytä seuraava puhuja");
            Console.WriteLine("5: Seuraava puhuja");
            Console.WriteLine("6: Lopeta ohjelma");

            string valinta = Console.ReadLine();

            switch (valinta)
            {
                case "1":
                    string nimi = KysyKelvollinenNimi("Syötä osallistujan nimi: ");
                    string yhteystiedot = KysyKelvollinenSyote("Syötä osallistujan yhteystiedot: ");
                    Console.Write("Haluatko puheenvuoron? (k/e): ");
                    bool haluaaPuheenvuoron = Console.ReadLine()?.Trim().ToLower() == "k";
                    tapahtuma.LisaaOsallistuja(nimi, yhteystiedot, haluaaPuheenvuoron);
                    break;

                case "2":
                    string poistettavaNimi = KysyKelvollinenNimi("Syötä poistettavan osallistujan nimi: ");
                    tapahtuma.PoistaIlmoittautuminen(poistettavaNimi);
                    break;

                case "3":
                    tapahtuma.NaytaKaikkiIlmoittautuneet();
                    break;

                case "4":
                    tapahtuma.NaytaSeuraavaPuheenvuoro();
                    break;

                case "5":
                    tapahtuma.PoistaPuheenvuorosta();
                    break;

                case "6":
                    Console.WriteLine("Ohjelma lopetettu.");
                    return;

                default:
                    Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
                    break;
            }
        }
    }

    private static string KysyKelvollinenNimi(string kehote)
    {
        string syote;
        do
        {
            Console.Write(kehote);
            syote = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(syote))
            {
                Console.WriteLine("Nimi ei voi olla tyhjä. Yritä uudelleen.");
            }
        } while (string.IsNullOrEmpty(syote));
        return syote;
    }

    private static string KysyKelvollinenSyote(string kehote)
    {
        string syote;
        do
        {
            Console.Write(kehote);
            syote = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(syote))
            {
                Console.WriteLine("Syöte ei voi olla tyhjä. Yritä uudelleen.");
            }
        } while (string.IsNullOrEmpty(syote));
        return syote;
    }
}
