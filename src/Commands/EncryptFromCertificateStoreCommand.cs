﻿// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using Arc4u.Diagnostics;
using Arc4u.Encryptor;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;


internal class EncryptFromCertificateStoreCommand
{
    public EncryptFromCertificateStoreCommand(ILogger<EncryptTextCommand> logger, EncryptTextCommand textEncryptor, CertificateHelper certificateHelper)
    {
        _logger = logger;
        _textEncryptor = textEncryptor;
        _certificateHelper = certificateHelper;
    }

    readonly EncryptTextCommand _textEncryptor;
    readonly ILogger<EncryptTextCommand> _logger;
    readonly CertificateHelper _certificateHelper;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.HelpOption();

        // Argument
        var certifcate = cmd.Argument<string>("certificate", "The friendly name of the certificate");

        // Options
        var nameOption = cmd.Option("-n | --storename", "The name of the folder where the certificate is stored in a Keychain or Certificate Store.", CommandOptionType.SingleValue);
        var locationOption = cmd.Option("-l | --storelocation", "The location where the certificate is stored in a Keychain or Certificate Store. Like on Windows: CurrentUser or LocalMachine. Default is CurrentUser!", CommandOptionType.SingleValue);

        cmd.Command("text", _textEncryptor.Configure);

        // Display id the certificate exist!
        cmd.OnExecute(() =>
        {
            _logger.Technical().LogInformation($"Encrypt with the certificate. {certifcate.Value}");
        });
    }
}
