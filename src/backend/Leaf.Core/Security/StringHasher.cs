using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Leaf.Security
{
    public static class StringHasher
    {
        private const int SaltSize = 128 / 8;
        private const int IterationCount = 10000;
        private const int NumBytesRequested = 256 / 8;

        public static string GenerateHashedString(string plain, bool base64 = false)
        {
            var saltBytes = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            var hashedBytes = KeyDerivation.Pbkdf2(plain, saltBytes,
                KeyDerivationPrf.HMACSHA256, IterationCount,
                NumBytesRequested);

            var hashed = base64 ? BytesToBase64(hashedBytes) : BytesToHexString(hashedBytes);

            var salt = base64 ? BytesToBase64(saltBytes) : BytesToHexString(saltBytes);

            return $"{hashed}:{salt}";
        }

        public static bool ValidateHashedString(string hashed, string plain, bool base64 = false)
        {
            if (!hashed.Contains(":")) throw new ArgumentException("해시 문자열이 유효하지 않습니다.");

            var splitedHashes = hashed.Split(":");
            var saltBytes = base64
                ? Base64ToBytes(splitedHashes.ElementAt(1))
                : HexStringToBytes(splitedHashes.ElementAt(1));

            var comparisonBytes = KeyDerivation.Pbkdf2(plain, saltBytes,
                KeyDerivationPrf.HMACSHA256, IterationCount,
                NumBytesRequested);

            var comparison = base64 ? BytesToBase64(comparisonBytes) : BytesToHexString(comparisonBytes);

            return comparison.Equals(splitedHashes.ElementAt(0));
        }

        private static string BytesToHexString(byte[] bytes)
        {
            return string.Concat(bytes.Select(b => b.ToString("X2")).ToArray());
        }

        private static byte[] HexStringToBytes(string hexString)
        {
            return Enumerable
                .Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();
        }

        private static string BytesToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        private static byte[] Base64ToBytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}