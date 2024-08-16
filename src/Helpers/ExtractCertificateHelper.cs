﻿// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.X509Certificates;
using System.Text;
using Arc4u.Cyphertool.Extensions;
using Arc4u.Diagnostics;
using Arc4u.Cyphertool;
using Arc4u.Results;
using Arc4u.Security.Cryptography;
using FluentResults;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Helpers
{
    internal class ExtractCertificateHelper
    {
        public ExtractCertificateHelper(CertificateHelper certificateHelper, ILogger<CertificateHelper> logger)
        {
            _certificateHelper = certificateHelper;
            _logger = logger;
        }

        readonly ILogger<CertificateHelper> _logger;
        readonly CertificateHelper _certificateHelper;
        public int ExtractCertificatePems(CommandArgument<string> certifcate, CommandOption passwordOption, CommandOption folderOption, CommandOption caOption, X509Certificate2? x509Encrypt = null)
        {
            Result result = Result.Ok();
            CheckCertificateArgument(certifcate)
                .LogIfFailed()
                .OnFailed(result)
                .OnSuccessNotNull(cert =>
                {
                    _certificateHelper.GetCertificate(cert, passwordOption?.Value(), null, null, true)
                          .LogIfFailed()
                          .OnFailed(result)
                          .OnSuccessNotNull(x509 =>
                          {
                              _logger.Technical().LogInformation("The certificate '{subject}' has been loaded!", x509.Subject);

                              var folder = folderOption.Value();
                              if (folderOption.HasValue())
                              {
                                  if (!Directory.Exists(folder))
                                  {
                                      _logger.Technical().LogError($"The folder '{folder}' does not exist! The current directory will be used.");
                                      folder = Environment.CurrentDirectory;
                                  }
                                  _logger.Technical().LogInformation("The keys will be stored in the folder '{folder}'.", folder!);
                              }

                              var commonName = x509.GetCommonName();

                              ExtractPublicKey(x509, commonName, folder);

                              ExtractPrivateKey(x509, commonName, folder, x509Encrypt);

                              ExtractCertificateAuthorities(x509, caOption, commonName, folder);
                          });
                });
            return result.IsSuccess ? 1 : -1;
        }

        private Result<string> CheckCertificateArgument(CommandArgument<string> certifcate)
        {
            if (null == certifcate?.Value)
            {
                return Result.Fail("The certificate is missing!");
            }

            if (!File.Exists(certifcate.Value))
            {
                return Result.Fail($"The certificate file '{certifcate.Value}' does not exist!");
            }

            return Result.Ok(certifcate.Value);
        }

        private void ExtractCertificateAuthorities(X509Certificate2 x509, CommandOption caOption, string commonName, string? folder)
        {
            if (caOption.HasValue())
            {
                // chain.
                var chain = new X509Chain();
                chain.Build(x509);
                int idx = 1;
                if (chain.ChainElements.Count > 1)
                {
                    _logger.Technical().LogInformation("Extract the CA certificates");

                    foreach (var element in chain.ChainElements.Skip(1))
                    {
                        _logger.Technical().LogInformation("{idx}: Certificate Subject: {subject}", idx++, element.Certificate.Subject);
                    }

                    StringBuilder sb = new StringBuilder();
                    foreach (var element in chain.ChainElements.Skip(1))
                    {

                        _certificateHelper.ConvertPublicKeyToPem(element.Certificate)
                                          .LogIfFailed()
                                          .OnSuccessNotNull((pem) =>
                                          {
                                              sb.Append(pem);
                                          });
                    }
                    if (folder is not null)
                    {
                        var fileName = SaveFile(sb.ToString(), folder, commonName, "{0}.ca.pem");
                        _logger.Technical().LogInformation("Save certificates authority public keys to {filename}", fileName);
                    }
                    else
                    {
                        Console.Write(sb.ToString());
                    }
                }
            }
        }

        private void ExtractPrivateKey(X509Certificate2 x509, string commonName, string? folder, X509Certificate2? x509Encrypt = null)
        {
            if (!x509.HasPrivateKey)
            {
                _logger.Technical().LogWarning("The certificate doesn't have a private key.");
                return;
            }

            _certificateHelper.ConvertPrivateKeyToPem(x509)
                              .LogIfFailed()
                              .OnSuccessNotNull((pem) =>
                              {
                                  var folderInfo = "Save encrypted private key to {name}";
                                  var consoleInfo = "Extract private key.";
                                  var content = pem;
                                  if (x509Encrypt is not null)
                                  {
                                      Result.Try(() => x509Encrypt.Encrypt(pem))
                                            .LogIfFailed()
                                            .OnSuccessNotNull(encrypted =>
                                            {
                                                content = encrypted;
                                                consoleInfo = "Extract encrypted private key.";
                                            });
                                  }


                                  if (folder is not null)
                                  {
                                      var fileName = SaveFile(content, folder, commonName, "{0}.key.pem");
                                      _logger.Technical().LogInformation(folderInfo, fileName);
                                  }
                                  else
                                  {
                                      _logger.Technical().LogInformation(consoleInfo);
                                      Console.WriteLine(content);
                                  }
                              });
        }

        private void ExtractPublicKey(X509Certificate2 x509, string commonName, string? folder)
        {
            _certificateHelper.ConvertPublicKeyToPem(x509)
                              .LogIfFailed()
                              .OnSuccessNotNull((pem) =>
                              {
                                  if (folder is not null)
                                  {
                                      var fileName = SaveFile(pem, folder, commonName, "{0}.pem");
                                      _logger.Technical().LogInformation("Save public key to {name}", fileName);
                                  }
                                  else
                                  {
                                      _logger.Technical().LogInformation("Extract public key.");
                                      Console.WriteLine(pem);
                                  }
                              });
        }

        private string SaveFile(string content, string folder, string commonName, string pattern)
        {
            var fileName = Path.Combine(folder, string.Format(pattern, commonName));
            var idx = 1;
            while (File.Exists(fileName))
            {
                fileName = Path.Combine(folder, string.Format(pattern, $"{commonName}({idx++})"));
            }
            File.WriteAllText(fileName, content);

            return fileName;
        }
    }
}
