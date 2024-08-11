// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using Arc4u.Encryptor;
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
        var certifcate = cmd.Argument<string>("certificate", "The friendly name of the certificate");

        // Options
        var nameOption = cmd.Option("-n | --storename", "The name of the folder where the certificate is stored in a Keychain or Certificate Store.", CommandOptionType.SingleValue);
        var locationOption = cmd.Option("-l | --storelocation", "The location where the certificate is stored in a Keychain or Certificate Store. Like on Windows: CurrentUser or LocalMachine. Default is CurrentUser!", CommandOptionType.SingleValue);


        // Display id the certificate exist!
        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}
