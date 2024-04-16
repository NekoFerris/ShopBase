using MySql.Data.MySqlClient;

namespace ShopBase.Persistenz
{
    internal class DBShopImage
    {
        public static void Anlegen(ShopImage shopImage)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Anlegen(shopImage);
        }

        public static ShopImage Lesen(int id)
        {
            using DBZugriff dBZugriff = new();
            MySqlDataReader mySqlDataReader = dBZugriff.Lesen(DBZugriff.GetPropName(typeof(ShopImage)), id);
            if (mySqlDataReader.Read())
            {
                ShopImage shopImage = new()
                {
                    Id = mySqlDataReader.GetInt32("Id"),
                    ImageData = StringToByte(mySqlDataReader.GetString("ImageData"))
                };
                return shopImage;
            }
            return null;
        }

        private static byte[] StringToByte(string str)
        {
            byte[] bytes = new byte[str.Length / 3];
            for (int i = 0; i < str.Length / 3; i++)
            {
                byte b = Convert.ToByte(str.Substring(i * 3, 3));
                bytes[i] = b;
            }
            return bytes;
        }

        public static void Aktualisieren(ShopImage shopImage)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Aktualisieren(shopImage);
        }

        public static void Loeschen(ShopImage shopImage)
        {
            using DBZugriff dBZugriff = new();
            dBZugriff.Loschen(shopImage);
        }
    }
}