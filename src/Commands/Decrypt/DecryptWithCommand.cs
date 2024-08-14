// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class DecryptWithCommand
{
    public DecryptWithCommand(ILogger<DecryptWithCommand> logger,
                              DecryptWithCertificateStoreCommand fromStoreCommand,
                              DecryptWithPfxFileCommand fromPfxFileCommand)
    {
        _logger = logger;
        _fromStoreCommand = fromStoreCommand;
        _fromPfxFileCommand = fromPfxFileCommand;
    }

    readonly DecryptWithPfxFileCommand _fromPfxFileCommand;
    readonly DecryptWithCertificateStoreCommand _fromStoreCommand;
    readonly ILogger<DecryptWithCommand> _logger;

    const string certificateStoreCommand = "certificate-store";
    const string pfxFileCommand = "pfx";
    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = nameof(DecryptWithCommand);
        cmd.Description = "DecryptCommand";
        cmd.HelpOption();

        var certCommand = cmd.Command(certificateStoreCommand, _fromStoreCommand.Configure);
        certCommand.AddName("cert");
        certCommand.AddName("keychain");

        cmd.Command(pfxFileCommand, _fromPfxFileCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }

}
