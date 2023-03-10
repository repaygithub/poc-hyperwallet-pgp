﻿using System;

namespace PayoutEncryption;

internal class Program
{
    static void Main(string[] args)
    {
        string enc = @"-----BEGIN PGP PUBLIC KEY BLOCK-----

mDMEZAoJrhYJKwYBBAHaRw8BAQdAXJrbUduBAuhxZP1+aOgxIoUgbB+vuLSZDEu4
8xRGpnq0GlBheXBhbCA8cGF5cGFsQHBheXBhbC5jb20+iJkEExYKAEEWIQRxmnkr
xW7U+MN8+pupMRrTtHAyIQUCZAoJrgIbAwUJA8JnAAULCQgHAgIiAgYVCgkICwIE
FgIDAQIeBwIXgAAKCRCpMRrTtHAyIeFXAPwMOVXnf1MxejUIynhalBv8djylXDSh
PXzHHVAh8DzvJQEAlG+i3sEIuU2+rBiw0YkDSoJAUYEsz02EfSvZJ/+G5wa4OARk
CgmuEgorBgEEAZdVAQUBAQdARzxsTgcJ1HosbK5st/NHrpJecvfwAEZSIb4Quh3a
rDADAQgHiH4EGBYKACYWIQRxmnkrxW7U+MN8+pupMRrTtHAyIQUCZAoJrgIbDAUJ
A8JnAAAKCRCpMRrTtHAyIWz4AP4rRYy+qczaWjLG8EMBAsSaOHpfZI0nJa2AIc/9
SAyXCQEA4W9k/lPX5T5WPrY5wKgG81IPQd+rWJKczKOGAMVdSgY=
=feC8
-----END PGP PUBLIC KEY BLOCK-----";

        string dec = @"-----BEGIN PGP PRIVATE KEY BLOCK-----

lFgEZAoJrhYJKwYBBAHaRw8BAQdAXJrbUduBAuhxZP1+aOgxIoUgbB+vuLSZDEu4
8xRGpnoAAQDCBScQX4WTpVIsmxnpiXqGAaQEyU5RUU07Wv+jt36OIA2ItBpQYXlw
YWwgPHBheXBhbEBwYXlwYWwuY29tPoiZBBMWCgBBFiEEcZp5K8Vu1PjDfPqbqTEa
07RwMiEFAmQKCa4CGwMFCQPCZwAFCwkIBwICIgIGFQoJCAsCBBYCAwECHgcCF4AA
CgkQqTEa07RwMiHhVwD8DDlV539TMXo1CMp4WpQb/HY8pVw0oT18xx1QIfA87yUB
AJRvot7BCLlNvqwYsNGJA0qCQFGBLM9NhH0r2Sf/hucGnF0EZAoJrhIKKwYBBAGX
VQEFAQEHQEc8bE4HCdR6LGyubLfzR66SXnL38ABGUiG+ELod2qwwAwEIBwAA/3rM
DMg3D8Hf4rFQ9xSyp2YroRu0eZsTj49v7luyicMoEWqIfgQYFgoAJhYhBHGaeSvF
btT4w3z6m6kxGtO0cDIhBQJkCgmuAhsMBQkDwmcAAAoJEKkxGtO0cDIhbPgA/itF
jL6pzNpaMsbwQwECxJo4el9kjSclrYAhz/1IDJcJAQDhb2T+U9flPlY+tjnAqAbz
Ug9B36tYkpzMo4YAxV1KBg==
=YvOw
-----END PGP PRIVATE KEY BLOCK-----";

        CryptoService crypto = new(enc, dec);

        crypto.PgpEncrypt("testfile.txt");
        crypto.PgpDecrypt("testfile.txt.pgp");
    }
}