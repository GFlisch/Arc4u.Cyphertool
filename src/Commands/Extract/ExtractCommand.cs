// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class ExtractCommand
{
    public ExtractCommand(ExtractFromCommand fromCommand)
    {
        _fromCommand = fromCommand;
    }

    readonly ExtractFromCommand _fromCommand;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "ExtractHelper";
        cmd.HelpOption();

        cmd.Command("from", _fromCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}
