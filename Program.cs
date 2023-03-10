// USAGE:
// ------
// 
// 1. If you don't supply any arguments, the executable will look for a file names `testfile.txt` in the current path and encrypt it.
//
// 2. The -key parameter can have the values public or private, and will determine both what key is going to be used and what operation is going to be performed.
//       - public : will encrypt the input file using the public key
//       - private : will decrypt the input file using the private key
//       - defaults to public
//
// 3. The -in parameter can contain the relative or full path to a file to be encrypted or decrypted.
//       - defaults to `testfile.txt`
//
// 4. If you want to create and use your own key pair, start by generating the sample key pairs to be used through the exchange.
//
//    You need to install gnupg on your machine, if you don't have it already. Then run the following command to generate a key pair.
//
//    $ gpg --gen-key
//
//    I've used "PayPal paypal@paypal.com" as the user and email address to simulate encrypting a payout file with hyperwallet public key and Paypal decrypting it
//    with their private key.
//    When asked for a password leave it blank. There is no information on hyperwallet documentation that the decryption will require password sharing between Repay
//    and Paypal. Note: You'll have to hit the button on the left of the confirmation window a few times.
//
//    You can check that the keys exist with:
//
//    $ gpg--list - keys
//
//    Now we need to export these keys to a file so we can simulate the encryption and decryption process based on SSM and SM parameters.
//
//    $ gpg --output privat.pgp --armor --export-secret-key Paypal
//    $ gpg --output public.pgp--armor--export Paypal
//
//    You should now have 2 ascii files in the current folder which you can open and copy into the code below. Note you can also use verbatim strings with @.

using System;

namespace PayoutEncryption;

internal class Program
{
    static void Main(string[] args)
    {
        const string pub = "mDMEZAoJrhYJKwYBBAHaRw8BAQdAXJrbUduBAuhxZP1+aOgxIoUgbB+vuLSZDEu4\r\n8xRGpnq0GlBheXBhbCA8cGF5cGFsQHBheXBhbC5jb20+iJkEExYKAEEWIQRxmnkr\r\nxW7U+MN8+pupMRrTtHAyIQUCZAoJrgIbAwUJA8JnAAULCQgHAgIiAgYVCgkICwIE\r\nFgIDAQIeBwIXgAAKCRCpMRrTtHAyIeFXAPwMOVXnf1MxejUIynhalBv8djylXDSh\r\nPXzHHVAh8DzvJQEAlG+i3sEIuU2+rBiw0YkDSoJAUYEsz02EfSvZJ/+G5wa4OARk\r\nCgmuEgorBgEEAZdVAQUBAQdARzxsTgcJ1HosbK5st/NHrpJecvfwAEZSIb4Quh3a\r\nrDADAQgHiH4EGBYKACYWIQRxmnkrxW7U+MN8+pupMRrTtHAyIQUCZAoJrgIbDAUJ\r\nA8JnAAAKCRCpMRrTtHAyIWz4AP4rRYy+qczaWjLG8EMBAsSaOHpfZI0nJa2AIc/9\r\nSAyXCQEA4W9k/lPX5T5WPrY5wKgG81IPQd+rWJKczKOGAMVdSgY=\r\n=feC8";
        const string priv = "lFgEZAoJrhYJKwYBBAHaRw8BAQdAXJrbUduBAuhxZP1+aOgxIoUgbB+vuLSZDEu4\r\n8xRGpnoAAQDCBScQX4WTpVIsmxnpiXqGAaQEyU5RUU07Wv+jt36OIA2ItBpQYXlw\r\nYWwgPHBheXBhbEBwYXlwYWwuY29tPoiZBBMWCgBBFiEEcZp5K8Vu1PjDfPqbqTEa\r\n07RwMiEFAmQKCa4CGwMFCQPCZwAFCwkIBwICIgIGFQoJCAsCBBYCAwECHgcCF4AA\r\nCgkQqTEa07RwMiHhVwD8DDlV539TMXo1CMp4WpQb/HY8pVw0oT18xx1QIfA87yUB\r\nAJRvot7BCLlNvqwYsNGJA0qCQFGBLM9NhH0r2Sf/hucGnF0EZAoJrhIKKwYBBAGX\r\nVQEFAQEHQEc8bE4HCdR6LGyubLfzR66SXnL38ABGUiG+ELod2qwwAwEIBwAA/3rM\r\nDMg3D8Hf4rFQ9xSyp2YroRu0eZsTj49v7luyicMoEWqIfgQYFgoAJhYhBHGaeSvF\r\nbtT4w3z6m6kxGtO0cDIhBQJkCgmuAhsMBQkDwmcAAAoJEKkxGtO0cDIhbPgA/itF\r\njL6pzNpaMsbwQwECxJo4el9kjSclrYAhz/1IDJcJAQDhb2T+U9flPlY+tjnAqAbz\r\nUg9B36tYkpzMo4YAxV1KBg==\r\n=YvOw";

        string key = pub;
        string inFile = "testfile.txt";
        CryptoService.KeyType keyType = CryptoService.KeyType.Public;

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-key":
                    var _ = Enum.TryParse(args[++i], ignoreCase: true, out keyType);
                    key = (keyType == CryptoService.KeyType.Public) ? pub : priv;
                    break;
                case "-in":
                    inFile = args[++i];
                    if (string.IsNullOrWhiteSpace(inFile))
                    {
                        throw new ArgumentNullException("-in", "You must provide the path to an input file.");
                    }
                    break;
                default:
                    break;
            }
        }

        CryptoService crypto = new(key, keyType); // defaults to public key and ancryption

        switch (keyType)
        {
            case CryptoService.KeyType.Private:
                crypto.PgpDecrypt(inFile);
                break;
            default:
                crypto.PgpEncrypt(inFile);
                break;
        }
    }
}