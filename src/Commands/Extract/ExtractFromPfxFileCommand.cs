// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.X509Certificates;
using System.Text;
using Arc4u.Diagnostics;
using Arc4u.Encryptor;
using Arc4u.Results;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands
{
    internal class ExtractFromPfxFileCommand
    {
        public ExtractFromPfxFileCommand(ILogger<EncryptFromPfxFileCommand> logger,
                                         CertificateHelper certificateHelper,
                                         ExtractEncryptCommand encryptCommand)
        {
            _logger = logger;
            _certificateHelper = certificateHelper;
            _encryptCommand = encryptCommand;
        }

        readonly ExtractEncryptCommand _encryptCommand;
        readonly CertificateHelper _certificateHelper;
        readonly ILogger<EncryptFromPfxFileCommand> _logger;


        public void Configure(CommandLineApplication cmd)
        {
            cmd.FullName = "ExtractFromPfxHelper";
            cmd.HelpOption();

            // Commmad to encrypt the private key certificate.
            cmd.Command("encrypt", _encryptCommand.Configure);

            // Argument
            var certifcate = cmd.Argument<string>("certificate", "The pfx certificate file.");

            // Options
            var passwordOption = cmd.Option("-p | --password", "The password to use for the file pfx certificate", CommandOptionType.SingleValue);
            var folderOption = cmd.Option("-f | --folder", "The folder to store the keys.", CommandOptionType.SingleValue);
            var caOption = cmd.Option("-c | --ca", "Extract the CA certificates.", CommandOptionType.NoValue);

            // Display id the certificate exist!
            cmd.OnExecute(() =>
            {
                if (null == certifcate?.Value)
                {
                    _logger.LogError("The certificate is missing!");
                    return -1;
                }

                if (!File.Exists(certifcate.Value))
                {
                    _logger.LogError($"The certificate file '{certifcate.Value}' does not exist!");
                    return -1;
                }

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
                                                              if (saveToFolder)
                                                              {
                                                                  var fileName = Path.Combine(folder!, $"{x509.FriendlyName}.key.pem");
                                                                  _logger.Technical().LogInformation("Save private key to folder {folder} with name {name}", folder!, fileName);
                                                                  File.WriteAllText(Path.Combine(folder!, fileName),
                                                                                    pem);
                                                              }
                                                              else
                                                              {
                                                                  _logger.Technical().LogInformation("Extract private key.");
                                                                  Console.WriteLine(pem);
                                                              }
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

                return 0;
            });
        }


    }
}
