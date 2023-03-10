using System;
using System.IO;

using Org.BouncyCastle.Bcpg;
using PgpCore;

namespace PayoutEncryption;

public partial class CryptoService
{
    [Flags]
    public enum KeyType
    {
        Public = 1,
        Private = 2
    }

    private readonly PGP _crypto;
    private readonly KeyType _keyType;

    private bool _armor;
    private bool _validate;

    public CryptoService(string publicKey, string privateKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey) || string.IsNullOrWhiteSpace(privateKey))
        {
            throw new ArgumentException("PGP keys must be a valid public/private key pair");
        }

        _crypto = new PGP(new EncryptionKeys(publicKey, privateKey, ""));
        _keyType = KeyType.Public | KeyType.Private;
        Configure();
    }

    public CryptoService(string key, KeyType operation)
    {
        EncryptionKeys keys;

        switch (operation)
        {
            case KeyType.Private:
                keys = new EncryptionKeys(key, "");
                _keyType = KeyType.Private;
                break;
            default:
                keys = new EncryptionKeys(key);
                _keyType = KeyType.Public;
                break;
        }

        _crypto = new PGP(keys);
        Configure();
    }

    public void PgpEncrypt(string fileInput)
    {
        if (!_keyType.HasFlag(KeyType.Public))
        {
            throw new InvalidOperationException("A valid public key is required for encryption.");
        }

        _crypto.EncryptFile(new FileInfo(fileInput), new FileInfo($"{fileInput}.pgp"), _armor, _validate);
    }

    public void PgpDecrypt(string fileInput)
    {
        if (!_keyType.HasFlag(KeyType.Private))
        {
            throw new InvalidOperationException("A valid private key is required for decryption.");
        }

        _crypto.DecryptFile(new FileInfo(fileInput), new FileInfo($"dec.{fileInput}"));
    }

    private void Configure()
    {
        _crypto.SymmetricKeyAlgorithm = SymmetricKeyAlgorithmTag.Aes128;
        _crypto.CompressionAlgorithm = CompressionAlgorithmTag.Zip;

        _armor = false;
        _validate = true;
    }
}
