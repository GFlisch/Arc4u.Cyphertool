// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using Arc4u.Cyphertool.Extensions;
using Arc4u.Cyphertool.Helpers;
using Arc4u.Diagnostics;
using Arc4u.Encryptor;
using Arc4u.Results;
using FluentResults;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;


internal class ExtractEncryptWithCertificateStoreCommand
{
    public ExtractEncryptWithCertificateStoreCommand(ILogger<EncryptTextCommand> logger,
                                                     CertificateHelper certificateHelper,
                                                     ExtractCertificateHelper extract)
    {
        _logger = logger;
        _extract = extract;
        _certificateHelper = certificateHelper;
    }

    readonly ExtractCertificateHelper _extract;
    readonly ILogger<EncryptTextCommand> _logger;
    readonly CertificateHelper _certificateHelper;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = nameof(ExtractEncryptWithCertificateStoreCommand);
        cmd.Description = "ExtractEncryptCertificateStoreCommand";
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


            Result result = Result.Ok();
            // Find the parent with full name ExtractFromPfxHelper
            cmd.Find(nameof(ExtractWithPfxFileCommand))
               .LogIfFailed()
               .OnSuccessNotNull(extractCmd =>
               {
                   var passwordOption = extractCmd.Options.FirstOrDefault(o => o.LongName == "password");
                   var folderOption = extractCmd.Options.FirstOrDefault(o => o.LongName == "folder");
                   var caOption = extractCmd.Options.FirstOrDefault(o => o.LongName == "certificate-authorities");
                   var certificate = extractCmd.Arguments.FirstOrDefault(a => a.Name is not null && a.Name.Equals("certificate", StringComparison.OrdinalIgnoreCase)) as CommandArgument<string>;

                   if (passwordOption is null)
                   {
                       Result.Fail("Your parent command doesn't contain any option --password!");
                   }
                   if (folderOption is null)
                   {
                       Result.Fail("Your parent command doesn't contain any option --folder!");
                   }
                   if (caOption is null)
                   {
                       Result.Fail("Your parent command doesn't contain any option --certificate-authorities!");
                   }
                   if (certificate is null)
                   {
                       Result.Fail("Your parent command doesn't contain an argument certificate.");
                   }

                   result.LogIfFailed();
                   if (result.IsFailed)
                   {
                       return;
                   }

                   _logger.Technical().LogInformation("Get certificate to encrypt the private key.");
                   _certificateHelper.GetCertificate(certifcateToEncrypt.Value!, null, nameOption.Value(), locationOption.Value())
                                     .LogIfFailed()
                                        .OnSuccessNotNull(x509 =>
                                        {
                                            _logger.Technical().LogInformation("Extract the keys.");
                                            _extract.ExtractCertificatePems(certificate!, passwordOption!, folderOption!, caOption!, x509);
                                        });
               });
            return 0;
        });
    }
}
