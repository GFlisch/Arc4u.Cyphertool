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
            var app = new CommandLineApplication();

            app.HelpOption();

            var certOption = app.Option("-c|--certificate <CERTIFICATE_Name>", "The name of the certificate", CommandOptionType.SingleValue)
                .IsRequired();

            var textOption = app.Option("-t|--text <TEXT>", "The text to encrypt", CommandOptionType.SingleValue)
                .IsRequired();

            app.OnExecute(() =>
            {
                string certificateName = certOption.Value();
                string textToEncrypt = textOption.Value();
                try
                {
                    // Load the certificate from the specified file
                    X509Certificate2 certificate = Certificate.FindCertificate(certificateName);

                    // Convert the encrypted bytes to a base64 string for easy transmission
                    string encryptedString = certificate.Encrypt(textToEncrypt);

                    Console.WriteLine($"Encrypted string: {encryptedString}");

                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return 1;
                }
            });

            return app.Execute(args);
        }
    }
}
