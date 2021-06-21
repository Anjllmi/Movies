using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Utilities
{
    public static class StringExtensions
    {
        public static string EncryptHash256(this string value)
        {
            using SHA256 mySHA256 = SHA256.Create();

            // Compute the hash of the fileStream.
            byte[] hashValue = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(value));
            return PrintByteArray(hashValue);




        }

        // Display the byte array in a readable format.
        public static string PrintByteArray(byte[] array)
        {
            StringBuilder stringBuilder = new(0);
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append($"{array[i]:X2}");
                Console.Write($"{array[i]:X2}");
                if ((i % 4) == 3) Console.Write(" ");
            }

            return stringBuilder.ToString();
        }
    }
}
