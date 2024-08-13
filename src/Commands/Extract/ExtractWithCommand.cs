// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class ExtractWithCommand
{
    public ExtractWithCommand(ILogger<EncryptFromCommand> logger,
                              ExtractWithPfxFileCommand pfxFileCommand)
    {
        _logger = logger;
        _fromPfxFileCommand = pfxFileCommand;
    }

    readonly ExtractWithPfxFileCommand _fromPfxFileCommand;
    readonly ILogger<EncryptFromCommand> _logger;

    const string pfxFileCommand = "pfx";
    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "ExtractCommand";
        cmd.HelpOption();

        cmd.Command(pfxFileCommand, _fromPfxFileCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }

}
