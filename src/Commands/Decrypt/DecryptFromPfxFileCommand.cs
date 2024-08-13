// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands
{
    internal class DecryptFromPfxFileCommand
    {
        public DecryptFromPfxFileCommand(ILogger<DecryptFromPfxFileCommand> logger,
                                         DecryptTextCommand decryptTextCommand,
                                         DecryptFileCommand decryptFileCommand)
        {
            _logger = logger;
            _decryptTextCommand = decryptTextCommand;
            _decryptFileCommand = decryptFileCommand;
        }

        readonly DecryptFileCommand _decryptFileCommand;
        readonly ILogger<DecryptFromPfxFileCommand> _logger;
        readonly DecryptTextCommand _decryptTextCommand;

        public void Configure(CommandLineApplication cmd)
        {
            cmd.FullName = "DecryptFromPfxCommand";
            cmd.HelpOption();

            // Argument
            var certifcate = cmd.Argument<string>("certificate", "The pfx certificate file.");

            // Options
            var passwordOption = cmd.Option("-p | --password", "The password to use for the file pfx certificate", CommandOptionType.SingleValue);

            cmd.Command("text", _decryptTextCommand.Configure);
            cmd.Command("file", _decryptFileCommand.Configure);

            // Display id the certificate exist!
            cmd.OnExecute(() =>
            {
                cmd.ShowHelp();
                return 0;
            });
        }
    }
}
