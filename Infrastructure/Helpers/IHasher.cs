

namespace Infrastructure.Helpers
{
    public interface IHasher
    {
        /// <summary>
        ///  получить хешированную строку
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        string Hash(ReadOnlySpan<char> secret);

        /// <summary>
        /// сравнить хеш с предполагаемым исходником
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="secretHashed"></param>
        /// <returns></returns>
        bool Verify(ReadOnlySpan<char> secret, string secretHashed);
    }
}
