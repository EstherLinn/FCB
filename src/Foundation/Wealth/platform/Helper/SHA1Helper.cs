using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Wealth.Helper
{
   public class SHA1Helper
    {
        public static string Encrypt(string content)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();

            byte[] shaHash = sha.ComputeHash(Encoding.GetEncoding(950).GetBytes(content));

            char[] hexDigits = {
                                   '0', '1', '2', '3', '4', '5', '6', '7',
                                   '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'
                               };

            char[] chars = new char[shaHash.Length * 2];

            for (int i = 0; i < shaHash.Length; i++)
            {
                int b = shaHash[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);

        }
    }
}
