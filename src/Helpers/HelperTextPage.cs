// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using Arc4u.Cyphertool.Resources;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;

namespace Arc4u.Cyphertool.Helpers
{
    internal class HelperTextPage : IHelpTextGenerator
    {
        public void Generate(CommandLineApplication application, TextWriter output)
        {
            if (application.Name is null)
            {
                output.WriteLine(HelpTexts.RootCommand);
            }

            if (application.FullName is not null)
            {
                output.WriteLine(HelpTexts.ResourceManager.GetString(application.FullName));
            }
        }
    }
}
