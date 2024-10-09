// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Arc4u.Security;
using Arc4u.Security.Cryptography;
using FluentResults;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool
{
    internal class CertificateHelper
    {
        public CertificateHelper(ILogger<CertificateHelper> logger, IX509CertificateLoader x509CertificateLoader)
        {
            _logger = logger;
            _x509CertificateLoader = x509CertificateLoader;
        }

        readonly ILogger<CertificateHelper> _logger;
        readonly IX509CertificateLoader _x509CertificateLoader;

        public Result<X509Certificate2> GetCertificate([DisallowNull] string cert, string? password, string? storeName, string? storeLocation, bool privKeyIsExportable = false, string prompt = "Password:")
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(cert);

            // Certificate is coming from the store if the certOption.Value() does not contain a file name ending with .pfx
            bool fromCertStore = !cert.EndsWith(".pfx");

            if (fromCertStore)
            {
                return GetCertificateFromStore(cert, storeName, storeLocation);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    password = Prompt.GetPassword(prompt, ConsoleColor.DarkYellow);
                }

                return GetCertificateFromFile(cert, password, privKeyIsExportable);
            }
        }
        private Result<X509Certificate2> GetCertificateFromFile(string cert, string? password, bool privKeyIsExportable)
        {
            if (!File.Exists(cert))
            {
                return Result.Fail($"The certificate file {cert} does not exist!");
            }

            return Result.Try(() => privKeyIsExportable ? new X509Certificate2(cert, password, X509KeyStorageFlags.Exportable) : new X509Certificate2(cert, password));
        }
        private Result<X509Certificate2> GetCertificateFromStore(string cert, string? storeName, string? storeLocation)
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
            _logger.LogInformation("Certificate name is: {Name}", certInfo.Name);
            _logger.LogInformation("Store name is: {StoreName}", certInfo.StoreName);
            _logger.LogInformation("Store location is: {Location}", certInfo.Location);
            _logger.LogInformation("Certificate search is: {FindType}", certInfo.FindType);

            return Result.Try(() => _x509CertificateLoader.FindCertificate(certInfo));
        }

        public Result<string> ConvertPublicKeyToPem(X509Certificate2 x509)
        {
            return Result.Try(() =>
            {
                byte[] certBytes = x509.Export(X509ContentType.Cert);
                return ConvertToPem(certBytes, "CERTIFICATE", true);
            });
        }

        public Result<string> ConvertPrivateKeyToPem(X509Certificate2 x509)
        {
            var privateKey = x509.GetRSAPrivateKey();
            if (privateKey is null)
            {
                return Result.Fail("The certificate doesn't have a RSA private key.");
            }
            return Result.Try(() =>
            {
                return ConvertToPem(privateKey.ExportRSAPrivateKey(), "PRIVATE KEY", true);
            });
        }
        private string ConvertToPem(string base64EncodedData, bool split = false)
        {
            if (!split)
                return base64EncodedData;

            var sb = new StringBuilder();

            var totalLength = base64EncodedData.Length;
            for (int i = 0; i < base64EncodedData.Length; i += 64)
            {
                var len = Math.Min(64, base64EncodedData.Length - i);
                totalLength -= len;
                if (totalLength > 0)
                    sb.AppendLine(base64EncodedData.Substring(i, len));
                else
                    sb.Append(base64EncodedData.Substring(i, len));
            }

            return sb.ToString();
        }
        private string ConvertToPem(byte[] certBytes, string header, bool split = false)
        {
            var base64EncodedData = Convert.ToBase64String(certBytes);
            var sb = new StringBuilder();
            sb.AppendLine($"-----BEGIN {header}-----");

            sb.AppendLine(ConvertToPem(base64EncodedData, split));

            sb.AppendLine($"-----END {header}-----");
            return sb.ToString();
        }
    }
}
