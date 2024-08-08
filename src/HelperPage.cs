// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using Microsoft.Extensions.DependencyInjection;

namespace Arc4u.Encryptor
{
    internal class HelperPage : IHelpTextGenerator
    {
        public HelperPage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        readonly IServiceProvider _serviceProvider;
        public void Generate(CommandLineApplication application, TextWriter output)
        { 
            if (application.Name is null)
            {
                output.WriteLine(@"
 .----------------.  .----------------.  .----------------.  .----------------.  .----------------.
| .--------------. || .--------------. || .--------------. || .--------------. || .--------------. |
| |      __      | || |  _______     | || |     ______   | || |   _    _     | || | _____  _____ | |
| |     /  \     | || | |_   __ \    | || |   .' ___  |  | || |  | |  | |    | || ||_   _||_   _|| |
| |    / /\ \    | || |   | |__) |   | || |  / .'   \_|  | || |  | |__| |_   | || |  | |    | |  | |
| |   / ____ \   | || |   |  __ /    | || |  | |         | || |  |____   _|  | || |  | '    ' |  | |
| | _/ /    \ \_ | || |  _| |  \ \_  | || |  \ `.___.'\  | || |      _| |_   | || |   \ `--' /   | |
| ||____|  |____|| || | |____| |___| | || |   `._____.'  | || |     |_____|  | || |    `.__.'    | |
| |              | || |              | || |              | || |              | || |              | |
| '--------------' || '--------------' || '--------------' || '--------------' || '--------------' |
 '----------------'  '----------------'  '----------------'  '----------------'  '----------------'

######## ##    ##  ######  ########  ##    ## ########  ########  #######  ######## 
##       ###   ## ##    ## ##     ##  ##  ##  ##     ##    ##    ##     ## ##     ##
##       ####  ## ##       ##     ##   ####   ##     ##    ##    ##     ## ##     ##
######   ## ## ## ##       ########     ##    ########     ##    ##     ## ######## 
##       ##  #### ##       ##   ##      ##    ##           ##    ##     ## ##   ##  
##       ##   ### ##    ## ##    ##     ##    ##           ##    ##     ## ##    ## 
######## ##    ##  ######  ##     ##    ##    ##           ##     #######  ##     ##

Encryptor is a tool to encrypt and decrypt text or file using a certificate or a pfx file.

The following commands exist:
- encrypt: Encrypt a text.
- encryptfile: Encrypt a file.

- decrypt: Decrypt a text.
- decryptfile: Decrypt a file.

You can use the --help option to get more information about each command.

like encrypt --help.

");
                output.WriteLine("");
                

            }

            var helpCommandGenerator = _serviceProvider.GetKeyedService<IHelpTextGenerator>("encrypt");

            if (helpCommandGenerator is not null)
                helpCommandGenerator.Generate(application, output);

            return;
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.WriteLine("1. Certificate store (windows) or keychain (linux).");
            Console.ResetColor();
            Console.WriteLine(@"
The command will use the following default values.
-c or --certificate: friendly name of the certificate.
-n or --name: the certificate store name, default value is ""My""
-l or --location: the certificate store location, defaut value is ""Current User"".
-t or --text: the text to encrypt or decrypt.
-f or --file: path file to encrypt.
-o or --output: output file path to store the result. 

Text and file cannot be used together!  

arc4u.encryptor -c devCertName -t ""clear text""  
Will encrypt the text ""clear text"" by using the certificate having a friendly name devCertName and the result will be displayed on the terminal window.  

arc4u.encryptor -c devCertName -f ""C:\temp\file.txt""
Will encrypt the content of the text in the file C:\temp\file.txt by using the certificate having a friendly name devCertName and the result will be displayed on the terminal window.  

");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("2. Using a pfx certificate file name.");
            Console.ResetColor();
            Console.WriteLine(@"
The command will use the following default values.
 -c or --certificate: The full path name of the certificate ending by the extension pfx.
 -p or --password: The certificate password.
 -t or --text: the text to encrypt or decrypt.
 -f or --file: path file to encrypt.
 -o or --output: output file path to store the result.  
Text and file cannot be used together! 
");
        }
    }
}
