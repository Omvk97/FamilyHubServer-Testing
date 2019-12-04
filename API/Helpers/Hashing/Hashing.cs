using System;
using System.Linq;
using System.Security.Cryptography;

namespace API.Helpers.Hashing
{
    public sealed class Hashing : IHashing
    {
        private const int SALT_SIZE = 16; // 128 bit
        private const int KEY_SIZE = 32; // 256 bit
        private const int ITERATIONS = 10000;

        public string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                    password,
                    SALT_SIZE,
                    ITERATIONS,
                    HashAlgorithmName.SHA512))
            {
                var hashedPassword = Convert.ToBase64String(algorithm.GetBytes(KEY_SIZE));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{ITERATIONS}.{salt}.{hashedPassword}";
            }
        }

        public bool Check(string hash, string inputPassword)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format. " +
                  "Should be formatted as `{iterations}.{salt}.{hash}`");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);

            using (var algorithm = new Rfc2898DeriveBytes(
              inputPassword,
              salt,
              iterations,
              HashAlgorithmName.SHA512))
            {
                var inputPasswordHashed = algorithm.GetBytes(KEY_SIZE);

                var hashedPassword = Convert.FromBase64String(parts[2]);

                var verified = inputPasswordHashed.SequenceEqual(hashedPassword);

                return verified;
            }
        }
    }
}
