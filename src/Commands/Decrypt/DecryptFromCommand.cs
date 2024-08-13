// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class DecryptFromCommand
{
    public DecryptFromCommand(ILogger<DecryptFromCommand> logger,
                              DecryptFromCertificateStoreCommand fromStoreCommand,
                              DecryptFromPfxFileCommand fromPfxFileCommand)
    {
        _logger = logger;
        _fromStoreCommand = fromStoreCommand;
        _fromPfxFileCommand = fromPfxFileCommand;
    }

    readonly DecryptFromPfxFileCommand _fromPfxFileCommand;
    readonly DecryptFromCertificateStoreCommand _fromStoreCommand;
    readonly ILogger<DecryptFromCommand> _logger;

    const string certificateStoreCommand = "certificate-store";
    const string pfxFileCommand = "pfx";
    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "DecryptCommand";
        cmd.HelpOption();

        cmd.Command(certificateStoreCommand, _fromStoreCommand.Configure);
        cmd.Command("keychain", _fromStoreCommand.Configure);
        cmd.Command("cert", _fromStoreCommand.Configure);

        cmd.Command(pfxFileCommand, _fromPfxFileCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }

}
