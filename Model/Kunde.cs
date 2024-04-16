using ShopBase.Persistenz;

namespace ShopBase
{
    public class Kunde : Person
    {
        public int Id { get; set; }

        public string? Pw { get; set; }
        public string Email { get; set; }

        public void Anlegen()
        {
            DBKunde.Anlegen(this);
        }

        public static Kunde Lesen(int Id)
        {
            return DBKunde.Lesen(Id);
        }

        public static List<Kunde> AlleLesen()
        {
            return DBKunde.AlleLesen();
        }

        public void Aktuallesieren()
        {
            DBKunde.Aktualisieren(this);
        }

        public static bool Auth(string email, string passwort, out Kunde? k)
        {
            List<Kunde> lstkunde = AlleLesen();
            Kunde? kunde = lstkunde.FirstOrDefault(l => l.Email == email);
            if (kunde != null && kunde.Pw == DBTools.HashPassword(passwort))
            {
                k = kunde;
                return true;
            }
            k = null;
            return false;
        }

        public static void Loeschen(Kunde k)
        {
            IEnumerable<Bestellung> list = Bestellung.AlleLesen().ToList();
            list = list.Where(kun => kun.Kunde.Id == k.Id);
            foreach (Bestellung b in list)
            {
                b.Loeschen();
            }
            DBKunde.Loeschen(k);
        }

        public override string ToString()
        {
            return $"{Name} {Vorname} {Email}";
        }
    }
}