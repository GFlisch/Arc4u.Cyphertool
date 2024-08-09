// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class DecryptCommand
{
    public DecryptCommand(DecryptFromCommand fromCommand)
    {
        _fromCommand = fromCommand;
    }

    readonly DecryptFromCommand _fromCommand;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "DecryptHelper";
        cmd.HelpOption();

        cmd.Command("from", _fromCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}
