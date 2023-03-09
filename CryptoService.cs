using System;

namespace PayoutEncryption;

public class CryptoService
{
    private readonly string? _encryptionKey;
    private readonly string? _decryptionKey;

    public CryptoService(string encryptionKey, string decryptionKey)
    {
        _encryptionKey = encryptionKey;
        _decryptionKey = decryptionKey;
    }

    public CryptoService(string key, OperationType operationType)
    {
        switch (operationType)
        {
            case OperationType.Decrypt:
                _decryptionKey = key;
                break;
            default:
                _encryptionKey = key;
                break;
        }
    }

    public void PgpEncrypt(string fileInput)
    {
        if (string.IsNullOrWhiteSpace(_encryptionKey))
        {
            throw new InvalidOperationException("A valid public key is required for encryption.");
        }
    }

    public void PgpDecrypt(string fileInput)
    {
        if (string.IsNullOrWhiteSpace(_decryptionKey))
        {
            throw new InvalidOperationException("A valid private key is required for decryption.");
        }
    }
}
