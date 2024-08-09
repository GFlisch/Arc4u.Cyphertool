// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using Arc4u.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class EncryptFromCommand
{
    public EncryptFromCommand(ILogger<EncryptFromCommand> logger,
                              EncryptFromCertificateStoreCommand fromStoreCommand,
                              EncryptFromPfxFileCommand fromPfxFileCommand)
    {
        _logger = logger;
        _fromStoreCommand = fromStoreCommand;
        _fromPfxFileCommand = fromPfxFileCommand;
    }

    readonly EncryptFromPfxFileCommand _fromPfxFileCommand;
    readonly EncryptFromCertificateStoreCommand _fromStoreCommand;
    readonly ILogger<EncryptFromCommand> _logger;

    const string certificateStoreCommand = "certificatestore";
    const string pfxFileCommand = "pfxfile";
    public void Configure(CommandLineApplication cmd)
    {
        cmd.HelpOption();

        cmd.Command(certificateStoreCommand, _fromStoreCommand.Configure);
        cmd.Command(pfxFileCommand, _fromPfxFileCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }

}
