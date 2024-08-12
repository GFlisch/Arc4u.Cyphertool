﻿// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Arc4u.Cyphertool.Commands;
using Arc4u.Cyphertool.Helpers;
using Arc4u.Cyphertool.Resources;
using Arc4u.Results.Logging;
using Arc4u.Security.Cryptography;
using FluentResults;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Arc4u.Encryptor;

class Program
{
    static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();

        try
        {
            // Set up dependency injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var helper = serviceProvider.GetRequiredService<IHelpTextGenerator>();


            var app = new CommandLineApplication
            {
                AllowArgumentSeparator = true,
                UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
                HelpTextGenerator = helper,
            };


            app.OnValidationError(context =>
            {
                if (app.GetOptions().All(o => !o.HasValue()))
                {
                    app.ShowHelp();
                }
                else
                {
                    Console.Error.WriteLine(context.ErrorMessage);
                }
            });

            app.HelpOption();

            Result.Setup(setup =>
            {
                setup.Logger = new FluentLogger(serviceProvider.GetRequiredService<ILogger<FluentLogger>>());
            });

            app.Command("encrypt", serviceProvider.GetRequiredService<EncryptCommand>().Configure);
            app.Command("decrypt", serviceProvider.GetRequiredService<DecryptCommand>().Configure);
            app.Command("extract", serviceProvider.GetRequiredService<ExtractCommand>().Configure);

            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "No version found!";
            app.VersionOption("-v|--version", version, string.Format(HelpTexts.Version, version));


            app.OnExecute(() =>
            {
                app.ShowVersion();
                return 0;
            });

            return app.Execute(args);

        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return -1;
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Add Serilog to the logging pipeline
        services.AddLogging(configure => configure.AddSerilog());
        services.AddSingleton<CertificateHelper>();
        services.AddSingleton<IX509CertificateLoader, X509CertificateLoader>();

        services.AddSingleton<EncryptCommand>();
        services.AddSingleton<EncryptFromCommand>();
        services.AddSingleton<EncryptFromCertificateStoreCommand>();
        services.AddSingleton<EncryptFromPfxFileCommand>();
        services.AddSingleton<EncryptTextCommand>();
        services.AddSingleton<EncryptFileCommand>();

        services.AddSingleton<DecryptCommand>();
        services.AddSingleton<DecryptFromCommand>();
        services.AddSingleton<DecryptFromCertificateStoreCommand>();
        services.AddSingleton<DecryptFromPfxFileCommand>();
        services.AddSingleton<DecryptTextCommand>();
        services.AddSingleton<DecryptFileCommand>();

        services.AddSingleton<ExtractCommand>();
        services.AddSingleton<ExtractWithCommand>();
        services.AddSingleton<ExtractWithPfxFileCommand>();
        services.AddSingleton<ExtractEncryptCommand>();
        services.AddSingleton<ExtractEncryptWithCommand>();
        services.AddSingleton<ExtractEncryptWithCertificateStoreCommand>();
        services.AddSingleton<ExtractEncryptWithPfxFileCommand>();

        services.AddSingleton<IHelpTextGenerator, HelperTextPage>();
        services.AddSingleton<ExtractCertificateHelper>();
    }
}
