// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;

namespace Arc4u.Cyphertool.Helpers
{
    internal class EncryptHelp : IHelpTextGenerator
    {
        public void Generate(CommandLineApplication application, TextWriter output)
        {
            output.WriteLine(@"
The command will use the following default values.

-c or --certificate: friendly name of the certificate.
-n or --name: the certificate store name, default value is ""My""
-l or --location: the certificate store location, defaut value is ""Current User"".
-t or --text: the text to encrypt or decrypt.
-f or --file: path file to encrypt.
-o or --output: output file path to store the result. 
");
        }
    }
}
