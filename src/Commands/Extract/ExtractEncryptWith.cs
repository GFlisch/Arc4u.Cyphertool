// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

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
        cmd.FullName = "ExtractEncryptCommand";
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
        cmd.FullName = "ExtractEncryptCommand";
        cmd.HelpOption();

        // Commands
        cmd.Command("certificate-store", _ExtractEncryptWithCertificateStore.Configure);
        cmd.Command("cert", _ExtractEncryptWithCertificateStore.Configure);
        cmd.Command("keychain", _ExtractEncryptWithCertificateStore.Configure);

        cmd.Command("pfx", _ExtractEncryptWithPfxFile.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}
