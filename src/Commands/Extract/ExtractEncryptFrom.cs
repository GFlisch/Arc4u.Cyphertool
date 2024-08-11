// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;

internal class ExtractEncryptCommand
{
    public ExtractEncryptCommand(ExtractEncryptFromCommand fromCommand)
    {
        _fromCommand = fromCommand;
    }

    readonly ExtractEncryptFromCommand _fromCommand;

    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "ExtractEncryptHelper";
        cmd.HelpOption();

        cmd.Command("from", _fromCommand.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}

internal class ExtractEncryptFromCommand
{
    public ExtractEncryptFromCommand(ExtractEncryptFromCertificateStoreCommand extractEncryptFromCertificateStore,
                                     ExtractEncryptFromPfxFileCommand extractEncryptFromPfxFile)
    {
        _extractEncryptFromCertificateStore = extractEncryptFromCertificateStore;
        _extractEncryptFromPfxFile = extractEncryptFromPfxFile;
    }

    readonly ExtractEncryptFromCertificateStoreCommand _extractEncryptFromCertificateStore;
    readonly ExtractEncryptFromPfxFileCommand _extractEncryptFromPfxFile;
    public void Configure(CommandLineApplication cmd)
    {
        cmd.FullName = "ExtractEncryptHelper";
        cmd.HelpOption();

        // Commands
        cmd.Command("certificatestore", _extractEncryptFromCertificateStore.Configure);
        cmd.Command("cert", _extractEncryptFromCertificateStore.Configure);
        cmd.Command("keychain", _extractEncryptFromCertificateStore.Configure);

        cmd.Command("pfx", _extractEncryptFromPfxFile.Configure);

        cmd.OnExecute(() =>
        {
            cmd.ShowHelp();
            return 0;
        });
    }
}
