
using System.Security.Cryptography;

namespace Infrastructure.Helpers
{
    public class Hasher : IHasher
    {
        private const int SaltSize = 32;
        private const int KeySize = 32;
        private const int iterations = 10000;
        private readonly HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
        private static char Delimiter = ';';

        public string Hash(ReadOnlySpan<char> secret)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize); //генерация соли
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(secret, salt, iterations, algorithm, KeySize); //хэширование по выбранному алгоритму
            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash)); //склейка
        }

        public bool Verify(ReadOnlySpan<char> secret, string secretHashed)
        {
            string[] list = secretHashed.Split(Delimiter);

            byte[] salt = Convert.FromBase64String(list[0]);
            byte[] hash = Convert.FromBase64String(list[1]);

            byte[] hashSecret = Rfc2898DeriveBytes.Pbkdf2(secret, salt, iterations, algorithm, KeySize);
            return CryptographicOperations.FixedTimeEquals(hash, hashSecret);
        }
    }
}
