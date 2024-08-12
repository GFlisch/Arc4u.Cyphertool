// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Arc4u.Cyphertool.Extensions;
using Arc4u.Cyphertool.Helpers;
using Arc4u.Diagnostics;
using Arc4u.Encryptor;
using Arc4u.Results;
using Arc4u.Security.Cryptography;
using FluentResults;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;

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


            Result result = Result.Ok();
            // Find the parent with full name ExtractFromPfxHelper
            cmd.Find("ExtractFromPfxHelper")
               .LogIfFailed()
               .OnSuccessNotNull(extractCmd =>
               {
                   var passwordOption = extractCmd.Options.FirstOrDefault(o => o.LongName == "password");// as CommandOption<string>;
                   var folderOption = extractCmd.Options.FirstOrDefault(o => o.LongName == "folder");// as CommandOption<string>;
                   var caOption = extractCmd.Options.FirstOrDefault(o => o.LongName == "certificate-authorities");// as CommandOption<bool>;
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
                   _certificateHelper.GetCertificate(certifcateToEncrypt.Value!, null, nameOption.Value(), locationOption.Value())
                                     .LogIfFailed()
                                        .OnSuccessNotNull(x509 =>
                                        {
                                            _extract.ExtractCertificatePems(certificate!, passwordOption!, folderOption!, caOption!, x509);
                                        });
               });
            return 0;
        });
    }
}
