// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using Arc4u.Cyphertool.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class ExtractEncryptCommand
{
    public ExtractEncryptCommand(ExtractEncryptWithCommand withCommand)
    {
        _withCommand = withCommand;
    }

    readonly ExtractEncryptWithCommand _withCommand;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = nameof(ExtractEncryptCommand);
        cmd.Description = "ExtractEncryptCommand";
        cmd.HelpOption();

        cmd.Command("with", _withCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}

internal class ExtractEncryptWithCommand
{
    public ExtractEncryptWithCommand(ExtractEncryptWithCertificateStoreCommand ExtractEncryptWithCertificateStore,
                                     ExtractEncryptWithPfxFileCommand ExtractEncryptWithPfxFile)
    {
        _ExtractEncryptWithCertificateStore = ExtractEncryptWithCertificateStore;
        _ExtractEncryptWithPfxFile = ExtractEncryptWithPfxFile;
    }

    readonly ExtractEncryptWithCertificateStoreCommand _ExtractEncryptWithCertificateStore;
    readonly ExtractEncryptWithPfxFileCommand _ExtractEncryptWithPfxFile;
    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = nameof(ExtractEncryptWithCommand);
        cmd.Description = "ExtractCommand";
        cmd.HelpOption();

        // Commands
        var certCommand = cmd.Command("certificate-store", _ExtractEncryptWithCertificateStore.Configure);
        certCommand.AddName("cert");
        certCommand.AddName("keychain");

        cmd.Command("pfx", _ExtractEncryptWithPfxFile.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}
