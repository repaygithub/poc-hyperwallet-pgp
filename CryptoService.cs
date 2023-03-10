using System;
using System.IO;

using PgpCore;

namespace PayoutEncryption;

public partial class CryptoService
{
    [Flags]
    public enum Operation
    {
        Encrypt = 1,
        Decrypt = 2
    }

    private readonly PGP _crypto;
    private readonly Operation _operation;

    public CryptoService(string publicKey, string privateKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey) || string.IsNullOrWhiteSpace(privateKey))
        {
            throw new ArgumentException("PGP keys must be a valid public/private key pair");
        }

        _crypto = new PGP(new EncryptionKeys(publicKey, privateKey, ""));
        _operation = Operation.Encrypt | Operation.Decrypt;
    }

    public CryptoService(string key, Operation operation)
    {
        EncryptionKeys keys;

        switch (operation)
        {
            case Operation.Decrypt:
                keys = new EncryptionKeys(key, "");
                _operation = Operation.Decrypt;
                break;
            default:
                keys = new EncryptionKeys(key);
                _operation = Operation.Encrypt;
                break;
        }

        _crypto = new PGP(keys);
    }

    public void PgpEncrypt(string fileInput)
    {
        if (!_operation.HasFlag(Operation.Encrypt))
        {
            throw new InvalidOperationException("A valid public key is required for encryption.");
        }

        _crypto.EncryptFile(new FileInfo(fileInput), new FileInfo($"{fileInput}.pgp"));
    }

    public void PgpDecrypt(string fileInput)
    {
        if (!_operation.HasFlag(Operation.Decrypt))
        {
            throw new InvalidOperationException("A valid private key is required for decryption.");
        }

        _crypto.DecryptFile(new FileInfo(fileInput), new FileInfo($"dec.{fileInput}"));
    }
}
