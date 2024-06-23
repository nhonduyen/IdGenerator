using System.Security.Cryptography;
using System.Text;

namespace Nano
{
    public class NanoId
    {
        // Define the default alphabet for NanoID
        private const string DefaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

        // Default length for NanoID
        private const int DefaultSize = 21;

        public static string Generate(int size = DefaultSize, string alphabet = DefaultAlphabet)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be a positive integer.");
            }

            if (string.IsNullOrEmpty(alphabet))
            {
                throw new ArgumentException("Alphabet must be a non-empty string.", nameof(alphabet));
            }

            var bytes = new byte[size];
            var result = new StringBuilder(size);

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);

                foreach (var b in bytes)
                {
                    result.Append(alphabet[b % alphabet.Length]);
                }
            }

            return result.ToString();
        }
    }
}
