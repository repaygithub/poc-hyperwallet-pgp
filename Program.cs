// USAGE:
// ------
// 
// 1. The program will look for a file called `sample` in the current directory.
//        - It will encrypt it to a new file named `encrypted`, using the supplied public key in the code.
//        - It will then decrypt the newly encrypted file into a new file called `decrypted`, using the private key in the code.
//
// 2. If you want to create your own keys, you need install gnupg on your machine. Then run the following command to generate a key pair:
//
//        $ gpg --gen-key
//
//        I used "PayPal paypal@paypal.com" as the user and email address to simulate encrypting a payout file with hyperwallet public
//        key and for Paypal to decrypt it with their private key.
//
//        When asked for a password leave it blank. There is no information on hyperwallet documentation that the decryption will require
//        password sharing between Repay and Paypal. Note: You'll have to hit the button on the left of the confirmation window a few
//        times.
//
//        You can check that the keys exist with:
//
//        $ gpg--list - keys
//
//        Now we need to export these keys to a file so we can simulate the encryption and decryption process based on SSM parameters.
//
//        $ gpg --output privat.pgp --armor --export-secret-key Paypal
//        $ gpg --output public.pgp--armor--export Paypal
//
//        You should now have 2 ascii files in the current folder which you can open and copy into the code below. If you wish, you can
//        also use verbatim strings with @.

using System.IO;
using System.Threading.Tasks;

namespace PayoutEncryption;

internal class Program
{
    const string _pubKey = "mDMEZAoJrhYJKwYBBAHaRw8BAQdAXJrbUduBAuhxZP1+aOgxIoUgbB+vuLSZDEu4\r\n8xRGpnq0GlBheXBhbCA8cGF5cGFsQHBheXBhbC5jb20+iJkEExYKAEEWIQRxmnkr\r\nxW7U+MN8+pupMRrTtHAyIQUCZAoJrgIbAwUJA8JnAAULCQgHAgIiAgYVCgkICwIE\r\nFgIDAQIeBwIXgAAKCRCpMRrTtHAyIeFXAPwMOVXnf1MxejUIynhalBv8djylXDSh\r\nPXzHHVAh8DzvJQEAlG+i3sEIuU2+rBiw0YkDSoJAUYEsz02EfSvZJ/+G5wa4OARk\r\nCgmuEgorBgEEAZdVAQUBAQdARzxsTgcJ1HosbK5st/NHrpJecvfwAEZSIb4Quh3a\r\nrDADAQgHiH4EGBYKACYWIQRxmnkrxW7U+MN8+pupMRrTtHAyIQUCZAoJrgIbDAUJ\r\nA8JnAAAKCRCpMRrTtHAyIWz4AP4rRYy+qczaWjLG8EMBAsSaOHpfZI0nJa2AIc/9\r\nSAyXCQEA4W9k/lPX5T5WPrY5wKgG81IPQd+rWJKczKOGAMVdSgY=\r\n=feC8";
    const string _prvKey = "lFgEZAoJrhYJKwYBBAHaRw8BAQdAXJrbUduBAuhxZP1+aOgxIoUgbB+vuLSZDEu4\r\n8xRGpnoAAQDCBScQX4WTpVIsmxnpiXqGAaQEyU5RUU07Wv+jt36OIA2ItBpQYXlw\r\nYWwgPHBheXBhbEBwYXlwYWwuY29tPoiZBBMWCgBBFiEEcZp5K8Vu1PjDfPqbqTEa\r\n07RwMiEFAmQKCa4CGwMFCQPCZwAFCwkIBwICIgIGFQoJCAsCBBYCAwECHgcCF4AA\r\nCgkQqTEa07RwMiHhVwD8DDlV539TMXo1CMp4WpQb/HY8pVw0oT18xx1QIfA87yUB\r\nAJRvot7BCLlNvqwYsNGJA0qCQFGBLM9NhH0r2Sf/hucGnF0EZAoJrhIKKwYBBAGX\r\nVQEFAQEHQEc8bE4HCdR6LGyubLfzR66SXnL38ABGUiG+ELod2qwwAwEIBwAA/3rM\r\nDMg3D8Hf4rFQ9xSyp2YroRu0eZsTj49v7luyicMoEWqIfgQYFgoAJhYhBHGaeSvF\r\nbtT4w3z6m6kxGtO0cDIhBQJkCgmuAhsMBQkDwmcAAAoJEKkxGtO0cDIhbPgA/itF\r\njL6pzNpaMsbwQwECxJo4el9kjSclrYAhz/1IDJcJAQDhb2T+U9flPlY+tjnAqAbz\r\nUg9B36tYkpzMo4YAxV1KBg==\r\n=YvOw";

    static async Task Main()
    {
        await EncryptAsync();
        await DecryptAsync();
    }

    private static async Task EncryptAsync()
    {
        using MemoryStream ouStream = new();
        using FileStream sampleFile = new("sample", FileMode.Open, FileAccess.Read, FileShare.Read);
        using FileStream encryptedFile = new("encrypted", FileMode.Create, FileAccess.Write, FileShare.Write);

        await CryptoService.PgpEncryptAsync(sampleFile, ouStream, _pubKey);
        await ouStream.CopyToAsync(encryptedFile);
    }

    private static async Task DecryptAsync()
    {
        using MemoryStream ouStream = new();
        using FileStream encryptedFile = new("encrypted", FileMode.Open, FileAccess.Read, FileShare.Read);
        using FileStream decryptedFile = new("decrypted", FileMode.Create, FileAccess.Write, FileShare.Write);

        await CryptoService.PgpDecryptAsync(encryptedFile, ouStream, _prvKey);
        await ouStream.CopyToAsync(decryptedFile);
    }
}