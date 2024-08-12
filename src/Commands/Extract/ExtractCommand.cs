// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class ExtractCommand
{
    public ExtractCommand(ExtractWithCommand fromCommand)
    {
        _fromCommand = fromCommand;
    }

    readonly ExtractWithCommand _fromCommand;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "ExtractHelper";
        cmd.HelpOption();

        cmd.Command("with", _fromCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }

 
}
