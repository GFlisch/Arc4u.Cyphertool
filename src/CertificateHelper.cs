// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Arc4u.Security;
using Arc4u.Security.Cryptography;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Arc4u.Encryptor
{
    internal class CertificateHelper
    {
        public static Result<X509Certificate2> GetCertificate([DisallowNull] string cert, string? password, string? storeName, string? storeLocation, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(cert);

            var logger = loggerFactory.CreateLogger<CertificateHelper>();
            var x509Logger = loggerFactory.CreateLogger<X509CertificateLoader>();

            // Certificate is coming from the store if the certOption.Value() does not contain a file name ending with .pfx
            bool fromCertStore = !cert.EndsWith(".pfx");

            if (fromCertStore)
            {
                return GetCertificateFromStore(cert, storeName, storeLocation, logger, x509Logger);
            }
            else
            {
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

            return Result.Try(() => new X509CertificateLoader(x509Logger).FindCertificate(certInfo));
        }

    }
}
