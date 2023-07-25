// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.X509Certificates;
using Arc4u.Encryptor;
using McMaster.Extensions.CommandLineUtils;

namespace MyEncryptionTool
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                AllowArgumentSeparator = true,
                UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.StopParsingAndCollect,
            };

            app.HelpOption();

            var certOption = app.Option("-c", "The name of the certificate", CommandOptionType.SingleValue)
                .IsRequired();

            var textOption = app.Option("-t", "The text to encrypt", CommandOptionType.SingleValue)
                .IsRequired();

            var decrypt = app.Option<bool>("-d", "Decrypt if specified otherwhise encrypt.", CommandOptionType.NoValue);

            app.OnExecute(() =>
            {
                if (string.IsNullOrWhiteSpace(certOption.Value()))
                {
                    Console.WriteLine("A certificate is needed");
                }
                string certificateName = certOption.Value()!;

                if (string.IsNullOrWhiteSpace(textOption.Value()))
                {
                    Console.WriteLine("A text is needed");
                }
                string text = textOption.Value()!;

                try
                {
                    // Load the certificate from the specified file
                    X509Certificate2 certificate = Certificate.FindCertificate(certificateName);

                    if (decrypt.HasValue())
                    {
                        string decryptedText = certificate.Decrypt(text);

                        Console.WriteLine($"Encrypted string: '{decryptedText}'");

                        return 0;
                    }
                    // Convert the encrypted bytes to a base64 string for easy transmission
                    string encryptedString = certificate.Encrypt(text);

                    Console.WriteLine($"Encrypted string: '{encryptedString}'");

                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());   
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return 1;
                }
            });

            return app.Execute(args);
        }
    }
}
