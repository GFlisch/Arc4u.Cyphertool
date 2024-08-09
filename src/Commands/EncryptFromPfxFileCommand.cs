// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands
{
    internal class EncryptFromPfxFileCommand
    {
        public EncryptFromPfxFileCommand(ILogger<EncryptFromPfxFileCommand> logger,
                                         EncryptTextCommand encryptTextCommand,
                                         EncryptFileCommand encryptFileCommand)
        {
            _logger = logger;
            _textEncryptor = encryptTextCommand;
            _encryptFileCommand = encryptFileCommand;
        }

        readonly EncryptFileCommand _encryptFileCommand;
        readonly ILogger<EncryptFromPfxFileCommand> _logger;
        readonly EncryptTextCommand _textEncryptor;

        public void Configure(CommandLineApplication cmd)
        {
            cmd.FullName = "EncryptFromPfxHelper";
            cmd.HelpOption();

            // Argument
            var certifcate = cmd.Argument<string>("certificate", "The pfx certificate file.");

            // Options
            var passwordOption = cmd.Option("-p | --password", "The password to use for the file pfx certificate", CommandOptionType.SingleValue);

            cmd.Command("text", _textEncryptor.Configure);
            cmd.Command("file", _encryptFileCommand.Configure);

            // Display id the certificate exist!
            cmd.OnExecute(() =>
            {
                cmd.ShowHelp();
                return 0;
            });
        }
    }
}
