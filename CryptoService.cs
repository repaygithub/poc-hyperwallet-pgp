using System.IO;
using System.Threading.Tasks;

using Org.BouncyCastle.Bcpg;
using PgpCore;

namespace PayoutEncryption;

public class CryptoService
{
    public static async Task PgpEncryptAsync(Stream inStream, Stream ouStream, string publicKey)
    {
        PGP crypto = new(new EncryptionKeys(publicKey))
        {
            SymmetricKeyAlgorithm = SymmetricKeyAlgorithmTag.Aes128,
            CompressionAlgorithm = CompressionAlgorithmTag.Zip
        };

        await crypto.EncryptStreamAsync(inStream, ouStream, armor: false, withIntegrityCheck: true);
        ouStream.Position = 0;
    }

    public static async Task PgpDecryptAsync(Stream inStream, Stream ouStream, string privateKey)
    {
        PGP crypto = new(new EncryptionKeys(privateKey, ""));

        await crypto.DecryptStreamAsync(inStream, ouStream);
        ouStream.Position = 0;
    }
}
