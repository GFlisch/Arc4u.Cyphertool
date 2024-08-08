// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class EncryptCommand
{
    public EncryptCommand(EncryptFromCommand fromCommand)
    {
        _fromCommand = fromCommand;
    }

    readonly EncryptFromCommand _fromCommand;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.Description = "This is a subcommand";
        cmd.HelpOption();

        cmd.Command("from", _fromCommand.Configure);

        cmd.OnExecute(() =>
        {
            Console.WriteLine("Encryptor");
            //cmd.ShowHelp();
            return 0;
        });
    }
}
