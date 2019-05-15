using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Sales_registration
{
    public class Crypter  //Шифрование и дешифровка паролей с ипользованием алгоритма RSA
    {
        private static string key=  " < RSAKeyValue >< Modulus > qxoUHLZkV4uXPY3crhFQRjt2foQVj6NfOFZ4w1PpmqyEgt8qYIZm8bl74 + kJisS69bmuAj8UWYZlM1DzLPf9S5pAIaZnG1mjZ7UCUS5hIDTufoYHUBMwxas329gwOwSZSmzwLIPXTWxmU64yG8 + 6bhUriTxy8j2IZIpmu1lA5P0=</Modulus><Exponent>AQAB</Exponent><P>w83E/q+yYh8qFwZphCKBP6LbL3i7nvfZPgrietEbSyjryJIlagkcPHZLNr9Vs1/tyULsQe3h3BRKu/dmAM4HTw==</P><Q>37Q1YbkFrmY6v9FZJZNSbfF0f2W9YTvM+TzOP4fmZiteOpr3Fry3IWn679e7/gFrQ1CAgAgnWuL5EK5XycN78w==</Q><DP>JZsgqbW+8f+ASvnNTDaAUmOJ610p08dQbw0SIHqG3nGWj2gaTXpAdRBXM8WdxRy4g74ZuDPi/CzdiapjaeYoGw==</DP><DQ>yyz+auJD8bDx3PQD9qfGbwqlF2xNQ7mvohMC9Bq3PMZYz/udPW5rGZMLEbksCjg5tqqv+xjsvZR2SBtAqoS81w==</DQ><InverseQ>OfsCcuKAzyHFdvwYLK045JrqPIP7Sjl4Pz+LS5K7VontdvT+k7UCRPtPM/wZuuMYlsoAtp1jFW+IWMFk5cyPUg==</InverseQ><D>B8m1FqV6pb110dhuLgIdvESinRmX/aS7Bc5xyro8DM1Dbs2HmUMk1mXR7MrDo2xW8i5UbEfAvl5upspNd0OGZOJ1P0ZhUNWYJxtDpp8FSQN0KGOkuT1lBhR53wyJqlNO9bjapxTsq7iTg3Qjq288MMfmXI16uASTgIECksf21aU=</D></RSAKeyValue>";
 
        internal  static string Decrypt(string password)
        {
            byte[] decrContent = null;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);

            //Обработка исключений хранения паролей в базе в незашифрованом виде
            try
            {
                decrContent = rsa.Decrypt(Convert.FromBase64String(password), false);

                return _toString(decrContent);
            }
            catch
            {
                  return password;
            }
        }

        private static string _toString(byte[] decrContent)
        {
            return Encoding.UTF8.GetString(decrContent);
        }

        private static byte[] _toByte(string pass)
        {
            return Encoding.UTF8.GetBytes(pass);
        }

        internal static string Encrypt(string pass)
        {
            byte[] encContent=null;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            encContent = rsa.Encrypt(_toByte(pass), false);

            return Convert.ToBase64String(encContent);
        }
    }
}
