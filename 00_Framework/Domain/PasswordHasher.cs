using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace _00_Framework.Domain
{
    
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// <see cref="SaltSize"/>, <see cref="KeySize"/> and <see cref="Options"/> need to encrypt the password
        /// </summary>
        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit
        private HashingOptions Options { get; }//Get the iteration


        public PasswordHasher(IOptions<HashingOptions> options)
        {
            Options = options.Value;
        }

       


        /// <summary>
        /// encrypt the password
        /// </summary>
        /// <param name="password">the password given from the user</param>
        /// <returns>The encrypted <see cref="password"/> with defined algorithm</returns>
        public string Hash(string password)
        {
            //Make a algorithm to encrypting password
            using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Options.Iterations, HashAlgorithmName.SHA256);
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            
            return $"{Options.Iterations}.{salt}.{key}";
        }


        /// <summary>
        /// Check the <paramref name="password"/>  is current with the hashed password<paramref name="hash"/>"/>
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="password"></param>
        /// <returns>Verified equals to <see langword="true"/>  if password equal to encrypted hash password</returns>
        public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                return (false, false);
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needsUpgrade = iterations != Options.Iterations;

            using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var keyToCheck = algorithm.GetBytes(KeySize);

            var verified = keyToCheck.SequenceEqual(key);

            return (verified, needsUpgrade);
        }
    }
}