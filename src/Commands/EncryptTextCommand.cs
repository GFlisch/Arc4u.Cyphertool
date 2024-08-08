// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using Arc4u.Diagnostics;
using Arc4u.Encryptor;
using Arc4u.Results;
using Arc4u.Security.Cryptography;
using FluentResults;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Arc4u.Cyphertool.Commands;



internal class EncryptTextCommand
{
    public EncryptTextCommand(ILogger<EncryptTextCommand> logger, CertificateHelper certificateHelper)
    {
        _logger = logger;
        _certificateHelper = certificateHelper;
    }

    readonly CertificateHelper _certificateHelper;
    readonly ILogger<EncryptTextCommand> _logger;

    public void Configure(CommandLineApplication app)
    {
        app.HelpOption();

        app.Argument<string>("text", "The text to encrypt.");
        var outputOption = app.Option("-o | --output", "The file to store the content.", CommandOptionType.SingleValue);

        app.OnExecute(() =>
        {
            var parent = app.Parent as CommandLineApplication;
            var certifcate = parent?.Arguments.FirstOrDefault(a => a.Name is not null && a.Name.Equals("certificate", StringComparison.OrdinalIgnoreCase));
            var storeName = parent?.Options.FirstOrDefault(a => a.LongName is not null && a.LongName.Equals("storename", StringComparison.OrdinalIgnoreCase));
            var storeLocation = parent?.Options.FirstOrDefault(a => a.LongName is not null && a.LongName.Equals("storelocation", StringComparison.OrdinalIgnoreCase));
            var password = parent?.Options.FirstOrDefault(a => a.LongName is not null && a.LongName.Equals("password", StringComparison.OrdinalIgnoreCase));

            if (null == certifcate?.Value)
            {
                _logger.LogError("The certificate is missing!");
                return;
            }

            _certificateHelper.GetCertificate(certifcate.Value, password?.Value() , storeName?.Value(), storeLocation?.Value())
             .LogIfFailed()
             .OnSuccessNotNull(x509 =>
             {
                 Result.Try(() => x509.Encrypt("text"))
                 .LogIfFailed()
                 .OnSuccessNotNull(encrypted =>
                 {
                     if (outputOption.HasValue())
                     {
                         try
                         {
                             File.WriteAllText(outputOption.Value()!, encrypted);
                             _logger.LogInformation($"The content has been saved in the file '{outputOption.Value()}'");
                         }
                         catch (Exception ex)
                         {
                             _logger.LogError(ex.Message);
                         }
                     }
                     else
                     {
                         Console.WriteLine(encrypted);
                     }
                 });
             });


        });
    }
}
