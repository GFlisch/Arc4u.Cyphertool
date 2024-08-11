// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Arc4u.Cyphertool.Extensions;
using Arc4u.Diagnostics;
using Arc4u.Encryptor;
using Arc4u.Results;
using Arc4u.Security.Cryptography;
using FluentResults;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;


internal class ExtractEncryptFromCertificateStoreCommand
{
    public ExtractEncryptFromCertificateStoreCommand(ILogger<EncryptTextCommand> logger,
                                                     CertificateHelper certificateHelper)
    {
        _logger = logger;
        _certificateHelper = certificateHelper;
    }

    readonly ILogger<EncryptTextCommand> _logger;
    readonly CertificateHelper _certificateHelper;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "ExtractEncryptCertificateStoreHelper";
        cmd.HelpOption();

        // Argument
        var certifcateToEncrypt = cmd.Argument<string>("certificate", "The friendly name of the certificate");

        // Options
        var nameOption = cmd.Option("-n | --storename", "The name of the folder where the certificate is stored in a Keychain or Certificate Store.", CommandOptionType.SingleValue);
        var locationOption = cmd.Option("-l | --storelocation", "The location where the certificate is stored in a Keychain or Certificate Store. Like on Windows: CurrentUser or LocalMachine. Default is CurrentUser!", CommandOptionType.SingleValue);


        // Display id the certificate exist!
        cmd.OnExecute(() =>
        {
            if (!certifcateToEncrypt.HasValue)
            {
                _logger.Technical().LogError("Missing certificate to encrypt the private key.");
                return 0;
            }

            // Find the parent with full name ExtractFromPfxHelper
            cmd.Find("ExtractFromPfxHelper")
               .LogIfFailed()
               .OnSuccessNotNull(extractCmd =>
               {
                   var passwordOption = extractCmd.Options.FirstOrDefault(o => o.LongName == "password");
                   var folderOption = extractCmd.Options.FirstOrDefault(o => o.LongName == "folder");
                   var caOption = extractCmd.Options.FirstOrDefault(o => o.LongName == "ca");
                   var certifcate = extractCmd.Arguments.FirstOrDefault(a => a.Name is not null && a.Name.Equals("certificate", StringComparison.OrdinalIgnoreCase));

                   _logger.Technical().LogInformation("Password {password}", passwordOption?.Value());
                   _logger.Technical().LogInformation("Folder {folder}", folderOption?.Value());
                   _logger.Technical().LogInformation("CA {ca}", caOption?.Value());
                   _logger.Technical().LogInformation("Certificate {c}", certifcate?.Value);

                   _certificateHelper.GetCertificate(certifcate.Value, passwordOption?.Value(), null, null, true)
                  .LogIfFailed()
                  .OnSuccessNotNull(x509 =>
                  {
                      _logger.Technical().LogInformation("The certificate '{subject}' has been loaded!", x509.Subject);

                      var folder = folderOption.Value();
                      bool saveToFolder = false;
                      if (folderOption.HasValue())
                      {
                          if (!Directory.Exists(folder))
                          {
                              _logger.Technical().LogError($"The folder '{folder}' does not exist!");
                              _logger.Technical().LogInformation("The pem files will be display to the console.");
                          }
                          else
                          {
                              saveToFolder = true;
                          }

                          _logger.Technical().LogInformation("The keys will be stored in the folder '{folder}'.", folder!);
                      }

                      _certificateHelper.ConvertPublicKeyToPem(x509)
                                                        .LogIfFailed()
                                                        .OnSuccessNotNull((pem) =>
                                                        {
                                                            if (saveToFolder)
                                                            {
                                                                var fileName = Path.Combine(folder!, $"{x509.FriendlyName}.pem");
                                                                _logger.Technical().LogInformation("Save public key to folder {folder} with name {name}", folder!, fileName);
                                                                File.WriteAllText(Path.Combine(folder!, fileName), pem);
                                                            }
                                                            else
                                                            {
                                                                _logger.Technical().LogInformation("Extract public key.");
                                                                Console.WriteLine(pem);
                                                            }
                                                        });


                      if (!x509.HasPrivateKey)
                      {
                          _logger.Technical().LogWarning("The certificate doesn't have a private key.");
                          return;
                      }

                      _certificateHelper.ConvertPrivateKeyToPem(x509)
                                                        .LogIfFailed()
                                                        .OnSuccessNotNull((pem) =>
                                                        {
                                                            _certificateHelper.GetCertificate(certifcateToEncrypt.Value!, null, nameOption.Value(), locationOption.Value())
                                                                              .LogIfFailed()
                                                                              .OnSuccessNotNull(x509ToEncrypt =>
                                                                              {
                                                                                  Result.Try(() => x509ToEncrypt.Encrypt(pem))
                                                                                  .LogIfFailed()
                                                                                  .OnSuccessNotNull(encrypted =>
                                                                                  {
                                                                                      if (saveToFolder)
                                                                                      {
                                                                                          var fileName = Path.Combine(folder!, $"{x509.FriendlyName}.key.pem");
                                                                                          _logger.Technical().LogInformation("Save private key to folder {folder} with name {name}", folder!, fileName);
                                                                                          File.WriteAllText(Path.Combine(folder!, fileName),
                                                                                                            encrypted);
                                                                                      }
                                                                                      else
                                                                                      {
                                                                                          _logger.Technical().LogInformation("Extract private key.");
                                                                                          Console.WriteLine(encrypted);
                                                                                      }
                                                                                  });
                                                                              });

                                                        });

                      if (caOption.HasValue())
                      {
                          // chain.
                          var chain = new X509Chain();
                          chain.Build(x509);
                          int idx = 1;
                          if (chain.ChainElements.Count > 1)
                          {
                              Console.WriteLine();
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
                              if (saveToFolder)
                              {
                                  var fileName = Path.Combine(folder!, $"{x509.FriendlyName}.ca.pem");
                                  _logger.Technical().LogInformation("Save certificates authority public keys to folder {folder} with name {name}", folder!, fileName);
                                  File.WriteAllText(Path.Combine(folder!, fileName),
                                                                    sb.ToString());
                              }
                              else
                              {
                                  Console.Write(sb.ToString());
                              }
                          }
                      }
                  });
               });
            return 0;
        });
    }
}
