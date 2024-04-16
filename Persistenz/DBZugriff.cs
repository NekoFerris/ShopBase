using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;

namespace ShopBase.Persistenz
{
    internal class DBZugriff : IDisposable
    {
        private readonly MySqlConnection MySqlConnection = null;
        private MySqlTransaction sqlTransaction = null;

        public void Dispose()
        {
            //sqlTransaction.Commit();
            //sqlTransaction.Dispose();
            MySqlConnection.Close();
        }

        internal DBZugriff()
        {
            MySqlConnection = new("");
            MySqlConnection.Open();
            //sqlTransaction = MySqlConnection.BeginTransaction();
        }

        internal int ExecuteNonQuery(string sql)
        {
            MySqlCommand sqlCommand = new(sql, MySqlConnection);
            return sqlCommand.ExecuteNonQuery();
        }

        internal int LastInsertId()
        {
            MySqlCommand sqlCommand = new("SELECT LAST_INSERT_ID();", MySqlConnection);
            return Convert.ToInt32(sqlCommand.ExecuteScalar());
        }

        internal MySqlDataReader ExecuteReader(string sql)
        {
            ;
            MySqlCommand sqlCommand = new(sql, MySqlConnection);
            MySqlDataReader dataReader = sqlCommand.ExecuteReader();
            return dataReader;
        }

        internal int Anlegen(object o)
        {
            string _tabellenname = GetPropName(o);
            Dictionary<string, string> _daten = GetStringsVonVariablenFuerSql(GetVariablen(o));
            string _sql = $"INSERT INTO {_tabellenname} ({string.Join(',', _daten.Keys)}) VALUES ('{string.Join("','", _daten.Values)}')";
            return ExecuteNonQuery(_sql);
        }

        internal int Aktualisieren(object o)
        {
            string _tabellenname = GetPropName(o);
            Dictionary<string, string> _daten = GetStringsVonVariablenFuerSql(GetVariablen(o));
            List<string> _aenderungen = [];
            foreach (KeyValuePair<string, string> keyValuePair in _daten)
            {
                _aenderungen.Add($"{keyValuePair.Key}='{keyValuePair.Value}',");
            }
            string _sql = $"UPDATE {_tabellenname} SET {string.Concat(_aenderungen).TrimEnd(',')} WHERE Id='{_daten.Values.First()}'";
            return ExecuteNonQuery(_sql);
        }

        internal MySqlDataReader Lesen(string tabelle, int Id)
        {
            string _sql = $"SELECT * FROM {tabelle} WHERE Id='{Id}'";
            MySqlDataReader mySqlDataReader = ExecuteReader(_sql);
            return mySqlDataReader;
        }

        internal MySqlDataReader LesenMitFremdschluessel(string tabelle, string fremdschluessel, int Id)
        {
            string _sql = $"SELECT * FROM {tabelle} WHERE {fremdschluessel}Id='{Id}'";
            MySqlDataReader mySqlDataReader = ExecuteReader(_sql);
            return mySqlDataReader;
        }

        internal MySqlDataReader AlleLesen(string tabelle)
        {
            string _sql = $"SELECT * FROM {tabelle}";
            MySqlDataReader mySqlDataReader = ExecuteReader(_sql);
            return mySqlDataReader;
        }

        internal int Loschen(object o)
        {
            string _tabellenname = GetPropName(o);
            Dictionary<string, string> _daten = GetStringsVonVariablenFuerSql(GetVariablen(o));
            string _sql = $"DELETE FROM {_tabellenname} WHERE Id = '{_daten["Id"]}'";
            return ExecuteNonQuery(_sql);
        }

        private static Dictionary<string, KeyValuePair<Type, object>> GetVariablen(object o)
        {
            Dictionary<string, KeyValuePair<Type, object>> variablen = [];
            foreach (PropertyInfo prop in o.GetType().GetProperties())
            {
                if (!Attribute.IsDefined(prop, typeof(NotMappedAttribute)))
                    if (prop.GetValue(o, null) != null)
                    {
                        if (!prop.GetValue(o, null).GetType().GetProperties().Any(m => m.Name == "Id"))
                        {
                            variablen.Add(prop.Name, new KeyValuePair<Type, object>(prop.PropertyType, prop.GetValue(o, null)));
                        }
                        else
                        {
                            foreach (PropertyInfo p in prop.GetValue(o, null).GetType().GetProperties())
                            {
                                if (p.Name == "Id")
                                    variablen.Add($"{prop.Name}Id", new KeyValuePair<Type, object>(p.GetType(), p.GetValue(prop.GetValue(o, null), null)));
                            }
                        }
                    }
            }
            return variablen;
        }

        private static Dictionary<string, string> GetStringsVonVariablenFuerSql(Dictionary<string, KeyValuePair<Type, object>> variablen)
        {
            Dictionary<string, string> ausgabe = [];
            foreach (KeyValuePair<string, KeyValuePair<Type, object>> keyValuePair in variablen)
            {
                if (keyValuePair.Value.Key == typeof(DateTime) || keyValuePair.Value.Key == typeof(DateTime?))
                {
                    DateTime GebDat = DateTime.Parse(Convert.ToString(keyValuePair.Value.Value));
                    ausgabe.Add(keyValuePair.Key, GebDat.ToString("yyyy-MM-dd"));
                }
                else if (keyValuePair.Value.Key == typeof(Double) || keyValuePair.Value.Key == typeof(Double?))
                {
                    ausgabe.Add(keyValuePair.Key, Convert.ToDouble(keyValuePair.Value.Value).ToString("0.000", CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")));
                }
                else if (keyValuePair.Value.Key == typeof(Byte[]))
                {
                    string aus = "";
                    foreach (byte b in (byte[])keyValuePair.Value.Value)
                    {
                        switch (b)
                        {
                            case < 10:
                                aus += $"00{b}";
                                break;

                            case < 100:
                                aus += $"0{b}";
                                break;

                            default:
                                aus += b;
                                break;
                        }
                    }
                    ausgabe.Add(keyValuePair.Key, aus);
                }
                else
                {
                    ausgabe.Add(keyValuePair.Key, Convert.ToString(keyValuePair.Value.Value));
                }
            }
            return ausgabe;
        }

        public static string GetPropName(object o)
        {
            return GetPropName(o.GetType());
        }

        public static string GetPropName(Type p)
        {
            return p.ToString().Split('.')[^1];
        }
    }
}