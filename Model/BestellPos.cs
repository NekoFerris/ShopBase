using ShopBase.Persistenz;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBase
{
    public class BestellPos
    {
        public int Id { get; set; }

        public int BestellungId { get; set; }
        public Artikel Artikel { get; set; }
        public int Menge { get; set; }
        public double Preis { get; set; }
        [NotMapped]
        public double PosPreis { get => Menge * Preis; }

        public override bool Equals(object? obj)
        {
            return obj is BestellPos pos && pos.Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"{Artikel.Bezeichnung,-15} {Menge} * {Preis.ToString("C2")} = {PosPreis.ToString("C2")}";
        }

        public static ObservableCollection<BestellPos> AlleLesen(int bid)
        {
            return DBBestellPos.AlleLesen(bid);
        }
        public void Anlegen()
        {
            DBBestellPos.Anlegen(this);
        }
        public void Loeschen()
        {
            DBBestellPos.Loeschen(this);
        }
        public void MengeAendern(int menge)
        {
            this.Menge = menge;
            DBBestellPos.Aktualisieren(this);
        }

        public void Aktuallesieren()
        {
            DBBestellPos.Aktualisieren(this);
        }
    }
}