using System.Security.Cryptography;
using System.Text;

namespace ShopBase.Persistenz
{
    public class DBTools
    {
        public static string? HashPassword(string password)
        {
            if (password != null)
                return BitConverter.ToString(SHA512.HashData(Encoding.ASCII.GetBytes(password)));
            return null;
        }
        public static string RandomPassword()
        {
            return RandomPassword(12);
        }
        public static string RandomPassword(int laenge)
        {
            Random random = new();
            char[] password = new char[laenge];
            char[] klein = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] groß = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] zahl = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            char[] sonder = new char[] { '!', '?', '+', '-', '*', '/', '§', '$', '%', '&', '=', '~', '_', '@', '#', '€' };
            while (true)
            {
                for (int i = 0; i < password.Length; i++)
                {
                    switch (random.Next(0, 4))
                    {
                        case 0:
                            password[i] = klein[random.Next(0, klein.Length)];
                            break;
                        case 1:
                            password[i] = groß[random.Next(0, groß.Length)];
                            break;
                        case 2:
                            password[i] = zahl[random.Next(0, zahl.Length)];
                            break;
                        case 3:
                            password[i] = sonder[random.Next(0, sonder.Length)];
                            break;
                    }
                }
                if (!password.Any(c => klein.Any(cc => cc == c)))
                    continue;
                if (!password.Any(c => groß.Any(cc => cc == c)))
                    continue;
                if (!password.Any(c => zahl.Any(cc => cc == c)))
                    continue;
                if (!password.Any(c => sonder.Any(cc => cc == c)))
                    continue;
                if (!password.All(c => klein.Count(cc => cc == c) < (password.Length / 5)))
                    continue;
                if (!password.All(c => groß.Count(cc => cc == c) < (password.Length / 5)))
                    continue;
                if (!password.All(c => zahl.Count(cc => cc == c) < (password.Length / 5)))
                    continue;
                if (!password.All(c => sonder.Count(cc => cc == c) < (password.Length / 5)))
                    continue;
                return new string(password);
            }
        }
    }
}