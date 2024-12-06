using System;
using System.Collections.Generic;

public class Tapahtuma
{
    private List<Osallistuja> osallistujat;
    private Queue<Osallistuja> puheenvuorojono;
    private int maxPaikkoja;
    private Dictionary<string, List<string>> kontaktit;

    public Tapahtuma(int maxPaikkoja)
    {
        this.maxPaikkoja = maxPaikkoja;
        osallistujat = new List<Osallistuja>();
        puheenvuorojono = new Queue<Osallistuja>();
        kontaktit = new Dictionary<string, List<string>>();
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
        kontaktit[nimi] = new List<string>();
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
            kontaktit.Remove(nimi);
            Console.WriteLine($"Osallistuja {nimi} poistettu.");

            var puheenvuorotemp = new Queue<Osallistuja>();
            while (puheenvuorojono.Count > 0)
            {
                var jonossa = puheenvuorojono.Dequeue();
                if (jonossa.Nimi != nimi) puheenvuorotemp.Enqueue(jonossa);
            }
            puheenvuorojono = puheenvuorotemp;

            foreach (var key in kontaktit.Keys)
            {
                kontaktit[key].Remove(nimi);
            }

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

    public void LisaaKontakti(string nimi1, string nimi2)
    {
        if (!kontaktit.ContainsKey(nimi1) || !kontaktit.ContainsKey(nimi2))
        {
            Console.WriteLine("Jompikumpi osallistujista ei ole rekisteröity.");
            return;
        }

        if (kontaktit[nimi1].Contains(nimi2))
        {
            Console.WriteLine($"Kontakti {nimi1} ja {nimi2} välillä on jo olemassa.");
            return;
        }

        kontaktit[nimi1].Add(nimi2);
        kontaktit[nimi2].Add(nimi1);
        Console.WriteLine($"Kontakti lisätty: {nimi1} - {nimi2}");
    }

    public void PoistaKontakti(string nimi1, string nimi2)
    {
        if (!kontaktit.ContainsKey(nimi1) || !kontaktit.ContainsKey(nimi2) ||
            !kontaktit[nimi1].Contains(nimi2))
        {
            Console.WriteLine("Kontaktia ei löydy.");
            return;
        }

        kontaktit[nimi1].Remove(nimi2);
        kontaktit[nimi2].Remove(nimi1);
        Console.WriteLine($"Kontakti poistettu: {nimi1} - {nimi2}");
    }

    public void NaytaKontaktit(string nimi)
    {
        if (!kontaktit.ContainsKey(nimi) || kontaktit[nimi].Count == 0)
        {
            Console.WriteLine($"{nimi} ei tunne ketään.");
            return;
        }

        Console.WriteLine($"Henkilöt, jotka {nimi} tuntee:");
        foreach (var kontakti in kontaktit[nimi])
        {
            Console.WriteLine(kontakti);
        }
    }

    public void NaytaVerkostoitumisreitit(string nimi)
    {
        if (!kontaktit.ContainsKey(nimi))
        {
            Console.WriteLine($"{nimi} ei ole rekisteröity.");
            return;
        }

        var verkostossa = new HashSet<string>();
        foreach (var suoraKontakti in kontaktit[nimi])
        {
            verkostossa.Add(suoraKontakti);
            foreach (var ystavanystava in kontaktit[suoraKontakti])
            {
                if (ystavanystava != nimi)
                {
                    verkostossa.Add(ystavanystava);
                }
            }
        }

        Console.WriteLine($"Osallistujat, jotka ovat {nimi}:n verkostossa:");
        foreach (var henkilo in verkostossa)
        {
            Console.WriteLine(henkilo);
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
            Console.WriteLine("6: Lisää kontakti");
            Console.WriteLine("7: Poista kontakti");
            Console.WriteLine("8: Näytä osallistujan kontaktit");
            Console.WriteLine("9: Näytä osallistujan verkostoitumisreitit");
            Console.WriteLine("10: Lopeta ohjelma");

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
                    string nimi1 = KysyKelvollinenNimi("Syötä ensimmäisen osallistujan nimi: ");
                    string nimi2 = KysyKelvollinenNimi("Syötä toisen osallistujan nimi: ");
                    tapahtuma.LisaaKontakti(nimi1, nimi2);
                    break;

                case "7":
                    string poistoNimi1 = KysyKelvollinenNimi("Syötä ensimmäisen osallistujan nimi: ");
                    string poistoNimi2 = KysyKelvollinenNimi("Syötä toisen osallistujan nimi: ");
                    tapahtuma.PoistaKontakti(poistoNimi1, poistoNimi2);
                    break;

                case "8":
                    string kontaktiNimi = KysyKelvollinenNimi("Syötä osallistujan nimi: ");
                    tapahtuma.NaytaKontaktit(kontaktiNimi);
                    break;

                case "9":
                    string verkostoNimi = KysyKelvollinenNimi("Syötä osallistujan nimi: ");
                    tapahtuma.NaytaVerkostoitumisreitit(verkostoNimi);
                    break;

                case "10":
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
