using MySql.Data.MySqlClient;
using System.Data;

namespace ShopBase.Persistenz
{
    internal class DBBestellung
    {
        internal static int Anlegen(Bestellung a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Anlegen(a);
            return dBZugriff.LastInsertId();
        }

        internal static void Aktualisieren(Bestellung a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Aktualisieren(a);
        }

        internal static void Loeschen(Bestellung a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Loschen(a);
        }
        internal static Bestellung Laden(int bid)
        {
            using DBZugriff dBZugriff = new();
            MySqlDataReader _lstdaten = dBZugriff.Lesen(DBZugriff.GetPropName(typeof(Bestellung)), bid);
            Bestellung b = new()
            {
                Id = _lstdaten.GetInt32(0),
                Datum = _lstdaten.GetDateTime(1),
                Kunde = Kunde.Lesen(_lstdaten.GetInt32(2)),
                BestellStatus = Enum.Parse<BestellStatus>(_lstdaten.GetString(3)),
                LstBestPoss = BestellPos.AlleLesen(_lstdaten.GetInt32(4))
            };
            _lstdaten.Close();
            return b;
        }
        internal static List<Bestellung> AlleLesen()
        {
            using DBZugriff dBZugriff = new();
            List<Bestellung> blist = new();
            MySqlDataReader _lstdaten = dBZugriff.AlleLesen(DBZugriff.GetPropName(typeof(Bestellung)));
            foreach (IDataRecord record in _lstdaten)
            {
                Bestellung b = new()
                {
                    Id = record.GetInt32(0),
                    Datum = record.GetDateTime(1),
                    Kunde = Kunde.Lesen(record.GetInt32(2)),
                    BestellStatus = Enum.Parse<BestellStatus>(record.GetString(3)),
                    LstBestPoss = BestellPos.AlleLesen(record.GetInt32(0))
                };
                blist.Add(b);
            }
            _lstdaten.Close();
            return blist;
        }
    }
}
