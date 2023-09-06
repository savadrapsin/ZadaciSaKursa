using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiskalniRacun
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Morate uneti naziv prodavnice kao argument komandne linije.");
                return;
            }

            string nazivProdavnice = args[0];
            List<StavkaRacuna> stavkeRacuna = new List<StavkaRacuna>();

            while (true)
            {
                Console.WriteLine("Unesite naziv artikla (ili 'kraj' za završetak unosa):");
                string nazivArtikla = Console.ReadLine();

                if (nazivArtikla.ToLower() == "kraj")
                    break;

                Console.WriteLine("Unesite količinu:");
                int kolicina = int.Parse(Console.ReadLine());

                Console.WriteLine("Unesite cenu po komadu:");
                decimal cenaPoKomadu = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Unesite jedinicu mere (kg, l, kom):");
                string jedinicaMere = Console.ReadLine();

                Console.WriteLine("Da li je artikal na akciji? (da/ne):");
                bool naAkciji = Console.ReadLine().ToLower() == "da";

                Console.WriteLine("Unesite stopu PDV-a (0%, 10% ili 20%) ili 'oslobodjen' ako je bez PDV-a:");
                string stopaPDV = Console.ReadLine();

                stavkeRacuna.Add(new StavkaRacuna(nazivArtikla, kolicina, cenaPoKomadu, jedinicaMere, naAkciji, stopaPDV));
            }

            decimal ukupnaCenaBezPDV = 0;
            decimal ukupnaCenaSaPDV = 0;

            Console.WriteLine("\nFiskalni račun za: " + nazivProdavnice);
            Console.WriteLine("Stavke računa:");
            Console.WriteLine("----------------------------------------");

            foreach (var stavka in stavkeRacuna)
            {
                decimal cenaStavkeBezPDV = stavka.IzracunajCenuBezPDV();
                decimal cenaStavkeSaPDV = stavka.IzracunajCenuSaPDV();

                ukupnaCenaBezPDV += cenaStavkeBezPDV;
                ukupnaCenaSaPDV += cenaStavkeSaPDV;

                Console.WriteLine($"{stavka.NazivArtikla} x {stavka.Kolicina} {stavka.JedinicaMere}: {cenaStavkeBezPDV:C} ({cenaStavkeSaPDV:C} sa PDV-om)");
            }

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Ukupna cena bez PDV-a: {ukupnaCenaBezPDV:C}");
            Console.WriteLine($"Ukupna cena sa PDV-om: {ukupnaCenaSaPDV:C}");
        }
    }

    class StavkaRacuna
    {
        public string NazivArtikla { get; }
        public int Kolicina { get; }
        public decimal CenaPoKomadu { get; }
        public string JedinicaMere { get; }
        public bool NaAkciji { get; }
        public string StopaPDV { get; }

        public StavkaRacuna(string nazivArtikla, int kolicina, decimal cenaPoKomadu, string jedinicaMere, bool naAkciji, string stopaPDV)
        {
            NazivArtikla = nazivArtikla;
            Kolicina = kolicina;
            CenaPoKomadu = cenaPoKomadu;
            JedinicaMere = jedinicaMere;
            NaAkciji = naAkciji;
            StopaPDV = stopaPDV;
        }

        public decimal IzracunajCenuBezPDV()
        {
            decimal cenaBezPDV = Kolicina * CenaPoKomadu;

            if (NaAkciji)
            {
                cenaBezPDV *= 0.9m; // 10% popust
            }

            return cenaBezPDV;
        }

        public decimal IzracunajCenuSaPDV()
        {
            decimal cenaBezPDV = IzracunajCenuBezPDV();

            if (StopaPDV == "oslobodjen")
            {
                return cenaBezPDV;
            }
            else if (StopaPDV == "10%")
            {
                return cenaBezPDV * 1.10m;
            }
            else if (StopaPDV == "20%")
            {
                return cenaBezPDV * 1.20m;
            }
            else
            {
                throw new ArgumentException("Nepoznata stopa PDV-a");
            }
        }
    }
}
