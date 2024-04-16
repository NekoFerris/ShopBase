using MySql.Data.MySqlClient;
using System.Data;

namespace ShopBase.Persistenz
{
    internal static class DBArtikel
    {
        internal static void Anlegen(Artikel a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Anlegen(a);
        }

        internal static void Aktualisieren(Artikel a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Aktualisieren(a);
        }

        internal static void Loeschen(Artikel a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Loschen(a);
        }

        internal static Artikel Lesen(int artid)
        {
            using DBZugriff dBZugriff = new();
            MySqlDataReader _lstdaten = dBZugriff.Lesen(DBZugriff.GetPropName(typeof(Artikel)), artid);
            _lstdaten.Read();
            Artikel a = new()
            {
                Id = _lstdaten.GetInt32(0),
                Preis = _lstdaten.GetDouble(1),
                Bezeichnung = _lstdaten.GetString(2),
                Beschreibung = _lstdaten.GetString(3),
                TimeStamp = _lstdaten.GetDateTime(5)
            };

            if (_lstdaten[4].GetType() != typeof(System.DBNull))
                a.ShopImage = ShopImage.Lesen(_lstdaten.GetInt32(4));
            _lstdaten.Close();
            return a;
        }

        internal static List<Artikel> AlleLesen()
        {
            using DBZugriff dBZugriff = new();
            MySqlDataReader _lstdaten = dBZugriff.AlleLesen(DBZugriff.GetPropName(typeof(Artikel)));
            List<Artikel> lstartikel = [];
            foreach (IDataRecord record in _lstdaten)
            {
                Artikel a = new()
                {
                    Id = record.GetInt32(0),
                    Preis = record.GetDouble(1),
                    Bezeichnung = record.GetString(2),
                    Beschreibung = record.GetString(3),
                    TimeStamp = record.GetDateTime(5)
                };
                if (record[4].GetType() != typeof(System.DBNull))
                    a.ShopImage = ShopImage.Lesen(record.GetInt32(4));
                lstartikel.Add(a);
            }
            _lstdaten.Close();
            return lstartikel;
        }
    }
}