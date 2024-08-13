// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using Arc4u.Cyphertool.Helpers;
using Arc4u.Encryptor;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands
{
    internal class ExtractWithPfxFileCommand
    {
        public ExtractWithPfxFileCommand(ILogger<EncryptFromPfxFileCommand> logger,
                                         CertificateHelper certificateHelper,
                                         ExtractCertificateHelper extract,
                                         ExtractEncryptCommand encryptCommand)
        {
            _logger = logger;
            _certificateHelper = certificateHelper;
            _encryptCommand = encryptCommand;
            _extract = extract;
        }

        readonly ExtractCertificateHelper _extract;
        readonly ExtractEncryptCommand _encryptCommand;
        readonly CertificateHelper _certificateHelper;
        readonly ILogger<EncryptFromPfxFileCommand> _logger;


        public void Configure(CommandLineApplication cmd)
        {
            cmd.FullName = "ExtractFromPfxCommand";
            cmd.HelpOption();

            // Commmad to encrypt the private key certificate.
            cmd.Command("encrypt", _encryptCommand.Configure);

            // Argument
            var certifcate = cmd.Argument<string>("certificate", "The pfx certificate file.");

            // Options
            var passwordOption = cmd.Option("-p | --password", "The password to use for the file pfx certificate", CommandOptionType.SingleValue);
            var folderOption = cmd.Option("-f | --folder", "The folder to store the keys.", CommandOptionType.SingleValue);
            var caOption = cmd.Option("-ca | --certificate-authorities", "Extract the CA certificates.", CommandOptionType.NoValue);

            cmd.OnExecute(() =>
            {
                return _extract.ExtractCertificatePems(certifcate, passwordOption, folderOption, caOption);
            });
        }



    }
}
