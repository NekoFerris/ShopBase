using MySql.Data.MySqlClient;
using System.Data;

namespace ShopBase.Persistenz
{
    internal class DBKunde
    {
        internal static void Anlegen(Kunde a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Anlegen(a);
        }

        internal static void Aktualisieren(Kunde a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Aktualisieren(a);
        }

        internal static void Loeschen(Kunde a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Loschen(a);
        }

        internal static Kunde Lesen(int kid)
        {
            using DBZugriff dBZugriff = new();
            MySqlDataReader _lstdaten = dBZugriff.Lesen(DBZugriff.GetPropName(typeof(Kunde)), kid);
            _lstdaten.Read();
            Kunde k = new()
            {
                Id = _lstdaten.GetInt32(0),
                Vorname = _lstdaten.GetString(1),
                Name = _lstdaten.GetString(2),
                Adresse = _lstdaten.GetString(3),
                GebDat = _lstdaten.GetDateTime(4),
                Geschlecht = (Geschlecht)Enum.Parse(typeof(Geschlecht), _lstdaten.GetString(5)),
                Pw = _lstdaten.GetString(6),
                Email = _lstdaten.GetString(7)

            };
            _lstdaten.Close();
            return k;
        }

        internal static List<Kunde> AlleLesen()
        {
            using DBZugriff dBZugriff = new();
            MySqlDataReader _lstdaten = dBZugriff.AlleLesen(DBZugriff.GetPropName(typeof(Kunde)));
            List<Kunde> lstKunde = new();
            foreach (IDataRecord record in _lstdaten)
            {
                Kunde k = new()
                {
                    Id = record.GetInt32(0),
                    Vorname = record.GetString(1),
                    Name = record.GetString(2),
                    Adresse = record.GetString(3),
                    GebDat = record.GetDateTime(4),
                    Geschlecht = (Geschlecht)Enum.Parse(typeof(Geschlecht), record.GetString(5)),
                    Pw = record.GetString(6),
                    Email = record.GetString(7)

                };
                lstKunde.Add(k);
            }
            _lstdaten.Close();
            return lstKunde;
        }
    }
}
