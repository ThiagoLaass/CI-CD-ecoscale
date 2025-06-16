using System.Security.Cryptography;
namespace EcoScale.src.Auth
{
    public class Cryptography {

        /// <summary>
        /// Gera um hash utilizando o algoritmo Rfc2898DeriveBytes com SHA256.
        /// </summary>
        /// <param name="strToHash">A string que será utilizada como base para a geração do hash.</param>
        /// <returns>
        /// Uma string codificada em Base64 que contém o sal concatenado com o hash resultante.
        /// </returns>
        /// <remarks>
        /// O método utiliza um salt de 16 bytes e executa 10000 iterações para derivar um hash de 20 bytes, 
        /// aumentando a segurança contra ataques de força bruta.
        /// </remarks>
        public string CreateHash(string strToHash)
        {
            using var deriveBytes = new Rfc2898DeriveBytes (
                strToHash, 16, 10000, HashAlgorithmName.SHA256
            );
            byte[] salt = deriveBytes.Salt;
            byte[] hash = deriveBytes.GetBytes(20);
            return Convert.ToBase64String(salt.Concat(hash).ToArray());
        }

        /// <summary>
        /// Verifica se uma string corresponde ao hash fornecido utilizando PBKDF2 com o algoritmo SHA256.
        /// </summary>
        /// <param name="hashedStr">O hash codificado em Base64 que contém o salt (primeiros 16 bytes) e o hash (bytes subsequentes).</param>
        /// <param name="strToVerify">A string que será verificada contra o hash.</param>
        /// <returns>
        /// Retorna true se o hash derivado da string para verificação for igual ao hash contido em 'hashedStr',
        /// caso contrário, retorna false.
        /// </returns>
        public bool VerifyHash(string hashedStr, string strToVerify)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedStr);
            byte[] salt = [.. hashBytes.Take(16)];
            using var deriveBytes = new Rfc2898DeriveBytes (
                strToVerify, salt, 10000, HashAlgorithmName.SHA256
            );
            byte[] newKey = deriveBytes.GetBytes(20);
            return newKey.SequenceEqual(hashBytes.Skip(16));
        }

        public string GenerateSixDigitString()
        {
            Random random = new();
            return random.Next(100000, 999999).ToString();
        }
    }
}