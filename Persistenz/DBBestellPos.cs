using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;

namespace ShopBase.Persistenz
{
    internal class DBBestellPos
    {
        internal static void Anlegen(BestellPos a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Anlegen(a);
        }

        internal static void Aktualisieren(BestellPos a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Aktualisieren(a);
        }

        internal static void Loeschen(BestellPos a)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Loschen(a);
        }
        internal static BestellPos Lesen(int bid)
        {
            using DBZugriff dBZugriff = new();
            MySqlDataReader _lstdaten = dBZugriff.Lesen(DBZugriff.GetPropName(typeof(BestellPos)), bid);
            BestellPos k = new()
            {
                Id = _lstdaten.GetInt32(0),
                Artikel = Artikel.Lesen(_lstdaten.GetInt32(1)),
                Menge = _lstdaten.GetInt32(2),
                Preis = _lstdaten.GetDouble(3),
                BestellungId = _lstdaten.GetInt32(4)
            };
            _lstdaten.Close();
            return k;
        }
        internal static ObservableCollection<BestellPos> AlleLesen(int bid)
        {
            using DBZugriff dBZugriff = new();
            MySqlDataReader _lstdaten = dBZugriff.LesenMitFremdschluessel(DBZugriff.GetPropName(typeof(BestellPos)), DBZugriff.GetPropName(typeof(Bestellung)), bid);
            ObservableCollection<BestellPos> lstBestPos = new();
            foreach (IDataRecord record in _lstdaten)
            {
                BestellPos k = new()
                {
                    Id = record.GetInt32(0),
                    Artikel = Artikel.Lesen(record.GetInt32(1)),
                    Menge = record.GetInt32(2),
                    Preis = record.GetDouble(3),
                    BestellungId = record.GetInt32(4)
                };
                lstBestPos.Add(k);
            }
            _lstdaten.Close();
            return lstBestPos;
        }
    }
}
