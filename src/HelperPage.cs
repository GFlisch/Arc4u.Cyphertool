// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;

namespace Arc4u.Encryptor
{
    internal class HelperPage : IHelpTextGenerator
    {
        public void Generate(CommandLineApplication application, TextWriter output)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
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
");

            Console.WriteLine("1. Certificate store (windows) or keychain (linux).");
            Console.ResetColor();
            Console.WriteLine(@"
The command will use the following default values.
 -c or --certificate: friendly name of the certificate.
 -n or --name: the default value is ""My""
 -s or --store: the defaut value is ""Current User"".
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
