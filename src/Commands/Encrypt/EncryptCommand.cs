// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class EncryptCommand
{
    public EncryptCommand(EncryptWithCommand fromCommand)
    {
        _fromCommand = fromCommand;
    }

    readonly EncryptWithCommand _fromCommand;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "EncryptCommand";
        cmd.HelpOption();

        cmd.Command("with", _fromCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}
