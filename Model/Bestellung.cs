using ShopBase.Persistenz;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBase
{
    public enum BestellStatus
    {
        Warenkorb,
        Eingegangen,
        InBearbeitung,
        Versandbereit,
        Versand
    }

    public class Bestellung
    {
        public int Id { get; set; }

        public DateTime Datum { get; set; }
        public Kunde Kunde { get; set; }

        [NotMapped]
        public ObservableCollection<BestellPos> LstBestPoss { get; set; } = new();
        [NotMapped]
        public double GesPreis { get => LstBestPoss.Sum(b => b.PosPreis); }

        public BestellStatus BestellStatus { get; set; }

        public static void ArtikelHinzufuegen(int kid, int artid, int menge)
        {
            if (menge < 1)
                return;
            Bestellung? bestellung = Bestellung.OffeneBestellung(kid);
            if (bestellung != null)
            {
                if (!bestellung.LstBestPoss.Any(b => b.Artikel.Id == artid))
                {
                    BestellPos bestellPos = new();
                    Artikel artikel = Artikel.Lesen(artid);
                    if (artikel == null)
                        return;
                    bestellPos.Artikel = Artikel.Lesen(artid);
                    bestellPos.BestellungId = bestellung.Id;
                    bestellPos.Preis = artikel.Preis;
                    bestellPos.Menge = menge;
                    bestellung.LstBestPoss.Add(bestellPos);
                    bestellPos.Anlegen();
                }
                {
                    BestellPos bestellPos = bestellung.LstBestPoss.Where(b => b.Artikel.Id == artid).Single();
                    bestellPos.MengeAendern(bestellPos.Menge + menge);
                }
            }
            else
            {
                bestellung = new()
                {
                    Kunde = Kunde.Lesen(kid),
                    Datum = DateTime.Now.Date,
                    BestellStatus = BestellStatus.Warenkorb
                };
                bestellung.Id = Bestellung.Anlegen(bestellung);
                Artikel artikel = DBArtikel.Lesen(artid);
                if (artikel == null)
                    return;
                BestellPos bestellPos = new()
                {
                    Artikel = Artikel.Lesen(artid),
                    BestellungId = bestellung.Id,
                    Preis = artikel.Preis,
                    Menge = menge
                };
                bestellung.LstBestPoss.Add(bestellPos);
                bestellPos.Anlegen();
            }
        }

        public void ArtikelEntfernen(int pos)
        {
            LstBestPoss[pos].Loeschen();
        }

        public void Bestellen()
        {
            Datum = DateTime.Now.Date;
            BestellStatus = BestellStatus.Eingegangen;
            PreiseAktuallesieren();
            DBBestellung.Aktualisieren(this);
        }

        public void PreiseAktuallesieren()
        {
            foreach (BestellPos bestellPos in LstBestPoss)
            {
                bestellPos.Preis = bestellPos.Artikel.Preis;
            }
        }

        public static Bestellung OffeneBestellung(int kid)
        {
            Bestellung bestellung = DBBestellung.AlleLesen().Where(b => b.Kunde.Id == kid).Where(b => b.BestellStatus == BestellStatus.Warenkorb).FirstOrDefault();
            return bestellung;
        }

        public static int AnzArt(int kid)
        {
            Bestellung _bestellung = OffeneBestellung(kid);
            if (_bestellung != null)
                return OffeneBestellung(kid).LstBestPoss.Count;
            else
                return 0;
        }
        public void Aktuallesieren()
        {
            DBBestellung.Aktualisieren(this);
        }

        internal static int Anlegen(Bestellung bestellung)
        {
            return DBBestellung.Anlegen(bestellung);
        }

        public void Loeschen()
        {
            foreach (BestellPos bp in LstBestPoss)
            {
                bp.Loeschen();
            }
            DBBestellung.Loeschen(this);
        }

        public static IEnumerable<Bestellung> AlleLesen()
        {
            return DBBestellung.AlleLesen();
        }
        public override string ToString()
        {
            return $"{Kunde.Email,-15} {Datum.ToShortDateString()} {BestellStatus} {LstBestPoss.Count}";
        }
    }
}