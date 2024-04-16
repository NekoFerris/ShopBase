using ShopBase.Persistenz;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ShopBase
{
    public class Artikel
    {
        public int Id { get; set; }
        public double Preis { get; set; } = -1;
        public string Bezeichnung { get; set; } = "daten fehlen";
        public string Beschreibung { get; set; } = "daten fehlen";
        public ShopImage? ShopImage { get; set; } = null;

        [NotMapped]
        public DateTime TimeStamp { get; set; }

        public Artikel()
        {
        }

        public void Anlegen()
        {
            if (ShopImage != null)
                this.ShopImage.Anlegen();
            DBArtikel.Anlegen(this);
        }

        public void Loeschen()
        {
            if (ShopImage != null)
                this.ShopImage.Loeschen();
            DBArtikel.Loeschen(this);
        }

        public void Aktualisieren()
        {
            if (ShopImage != null)
                if (ShopImage.Lesen(Id) != null)
                    this.ShopImage.Aktualisieren();
                else
                    this.ShopImage.Anlegen();
            if (DBArtikel.Lesen(Id).TimeStamp < TimeStamp)
                DBArtikel.Aktualisieren(this);
            else
                throw new MultiUserAccessException("Artikel wurde geändert");
        }

        public static Artikel Lesen(int Id)
        {
            return DBArtikel.Lesen(Id);
        }

        public static List<Artikel> AlleLesen()
        {
            List<Artikel> artikel = DBArtikel.AlleLesen();
            return artikel;
        }

        public static List<Artikel> AlleLesen(string suchbeg)
        {
            suchbeg = suchbeg.ToLower();
            List<Artikel> artikell = DBArtikel.AlleLesen();
            List<Artikel> artikels =
            [
                .. artikell.FindAll(a => a.Beschreibung.Contains(suchbeg, StringComparison.CurrentCultureIgnoreCase)),
                .. artikell.FindAll(a => a.Bezeichnung.Contains(suchbeg, StringComparison.CurrentCultureIgnoreCase)),
            ];
            List<Artikel> artikelr = artikels.DistinctBy(a => a.Id).ToList();
            return artikelr;
        }

        public override string ToString()
        {
            return $"{Bezeichnung,-15} {Preis.ToString("C2", CultureInfo.GetCultureInfoByIetfLanguageTag("de-DE")),10} {Beschreibung,-20}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Artikel artikel && Id == artikel.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}