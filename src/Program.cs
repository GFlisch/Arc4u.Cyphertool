// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Arc4u.Results;
using Arc4u.Results.Logging;
using Arc4u.Security.Cryptography;
using FluentResults;
using McMaster.Extensions.CommandLineUtils;
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

            var app = new CommandLineApplication
            {
                AllowArgumentSeparator = true,
                UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
                HelpTextGenerator = new HelperPage(),
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

            var certOption = app.Option("-c | --certificate", "The name of the certificate", CommandOptionType.SingleValue)
                                .IsRequired();

            var textOption = app.Option("-t | --text", "The text to encrypt", CommandOptionType.SingleValue);

            var decrypt = app.Option<bool>("-d | --decrypt", "Decrypt if specified otherwhise encrypt.", CommandOptionType.NoValue);

            var passwordOption = app.Option("-p | --password", "The password to use for the file pfx certificate", CommandOptionType.SingleValue);

            var fileOption = app.Option("-f | --file", "The file name of the text to encrypt. if the --file option is used the --text one is not used!", CommandOptionType.SingleValue);

            var nameOption = app.Option("-n | --storename", "The name of the folder where the certificate is stored in a Keychain or Certificate Store.", CommandOptionType.SingleValue);

            var locationOption = app.Option("-l | --storelocation", "The location where the certificate is stored in a Keychain or Certificate Store. Like on Windows: CurrentUser or LocalMachine. Default is CurrentUser!", CommandOptionType.SingleValue);

            var outputOption = app.Option("-o | --output", "The file to store the content.", CommandOptionType.SingleValue);


            app.OnExecute(() =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                Assembly assembly = Assembly.GetExecutingAssembly();

                logger.LogInformation($"Arc4u.Encryption, version {assembly.GetName().Version}");

                string certificateName = certOption.Value()!;
                bool stopOnErrors = false;

                if (string.IsNullOrWhiteSpace(textOption.Value()) && string.IsNullOrWhiteSpace(fileOption.Value()))
                {
                    logger.LogError("A text -t | --text or a file -f | --file is needed");
                    stopOnErrors = true;
                }

                if (!string.IsNullOrWhiteSpace(textOption.Value()) && !string.IsNullOrWhiteSpace(fileOption.Value()))
                {
                    logger.LogError("Only a text or a file must be given.");
                    stopOnErrors = true;
                }

                if (stopOnErrors)
                {
                    return 1;
                }

                string text = fileOption.HasValue() ? File.ReadAllText(fileOption.Value()!) : textOption.Value()!;

                var result = Result.Ok(string.Empty);

                CertificateHelper.GetCertificate(certOption.Value()!, passwordOption.Value(), nameOption.Value(), locationOption.Value(), serviceProvider)
                   .LogIfFailed()
                   .OnSuccessNotNull(certificate =>
                   {
                       // Decryption
                       if (decrypt.HasValue())
                       {
                           result = Result.Try(() => certificate.Decrypt(text));
                       }
                       else // encryption
                       {
                           result = Result.Try(() => certificate.Encrypt(text));
                       }
                   });

                var returnValue = 1;

                result
                    .LogIfFailed()
                    .OnSuccess(text =>
                    {
                        if (outputOption.HasValue())
                        {
                            try
                            {
                                File.WriteAllText(outputOption.Value()!, text);
                                logger.LogInformation($"The content has been saved in the file '{outputOption.Value()}'");
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex.Message);
                            }
                        }
                        else
                        {
                            if (decrypt.HasValue())
                            {
                                Console.Write("Decrypted text: '");
                            }
                            else
                            {
                                Console.Write("Encrypted text: '");
                            }
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(text);
                            Console.ResetColor();
                            Console.WriteLine("'");
                        }

                        returnValue = 0;
                    });

                return returnValue;
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

        // Register other services
        // services.AddTransient<MyService>();
    }
}
