// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class DecryptCommand
{
    public DecryptCommand(DecryptWithCommand withCommand)
    {
        _withCommand = withCommand;
    }

    readonly DecryptWithCommand _withCommand;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "DecryptCommand";
        cmd.HelpOption();

        cmd.Command("with", _withCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}
