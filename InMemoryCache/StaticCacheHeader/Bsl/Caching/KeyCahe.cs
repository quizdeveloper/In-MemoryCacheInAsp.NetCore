
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StaticCacheHeader.Bsl.Caching
{
    public class KeyCahe
    {

        public static string GenCacheKey(string cacheName, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                string separator =  "_";
                string cacheKey = cacheName;
                return args.Aggregate(cacheKey, (current, param) => current + (separator + (param.GetType() == typeof(string) ? CalculateMD5Hash(param.ToString()) : param)));
            }
            else return cacheName;
        }

        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
