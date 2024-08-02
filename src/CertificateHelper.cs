// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Arc4u.Security;
using Arc4u.Security.Cryptography;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Arc4u.Encryptor
{
    internal class CertificateHelper
    {
        public static Result<X509Certificate2> GetCertificate([DisallowNull] string cert, string? password, string? storeName, string? storeLocation, IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(cert);

            var logger = serviceProvider.GetRequiredService<ILogger<CertificateHelper>>();
            var x509Logger = serviceProvider.GetRequiredService<ILogger<X509CertificateLoader>>();

            // Certificate is coming from the store if the certOption.Value() does not contain a file name ending with .pfx
            bool fromCertStore = !cert.EndsWith(".pfx");

            if (fromCertStore)
            {
                return GetCertificateFromStore(cert, storeName, storeLocation, logger, x509Logger);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.Write("Password:");
                    while (true)
                    {
                        var key = Console.ReadKey(true);
                        // Break the loop if Enter key is pressed
                        if (key.Key == ConsoleKey.Enter)
                            break;
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                    Console.WriteLine("");
                }

                return GetCertificateFromFile(cert, password, logger);
            }
        }

        private static Result<X509Certificate2> GetCertificateFromFile(string cert, string? password, ILogger<CertificateHelper> logger)
        {
            if (!File.Exists(cert))
            {
                return Result.Fail($"The certificate file {cert} does not exist!");
            }

            if (File.Exists(cert))
            {
                return Result.Try(() => new X509Certificate2(cert, password));
            }

            return Result.Fail($"The certificate file {cert} does not exist!");
        }
        private static Result<X509Certificate2> GetCertificateFromStore(string cert, string? storeName, string? storeLocation, ILogger<CertificateHelper> logger, ILogger<X509CertificateLoader> x509Logger)
        {
            var certInfo = new CertificateInfo
            {
                Name = cert
            };

            if (!string.IsNullOrWhiteSpace(storeName))
            {
                try
                {
                    certInfo.StoreName = Enum.Parse<StoreName>(storeName, true);
                }
                catch (Exception)
                {

                    return Result.Fail($"{storeName} is not a valid store!");
                }

            }

            if (!string.IsNullOrWhiteSpace(storeLocation))
            {
                try
                {
                    certInfo.Location = Enum.Parse<StoreLocation>(storeLocation, true);
                    
                }
                catch (Exception)
                {

                    return Result.Fail($"{storeLocation} is not a valid location!");
                }
            }
            logger.LogInformation($"Certificate name is: {certInfo.Name}");
            logger.LogInformation($"Store name is: {certInfo.StoreName}");
            logger.LogInformation($"Store location is: {certInfo.Location}");
            logger.LogInformation($"Certificate search is: {certInfo.FindType}");

            return Result.Try(() => new X509CertificateLoader(x509Logger).FindCertificate(certInfo));
        }

    }
}
