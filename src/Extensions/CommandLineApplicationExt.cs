// Licensed to the Arc4u Foundation under one or more agreements.
// The Arc4u Foundation licenses this file to you under the MIT license.

using FluentResults;
using McMaster.Extensions.CommandLineUtils;

namespace Arc4u.Cyphertool.Extensions
{
    internal static class CommandLineApplicationExt
    {
        public static Result<CommandLineApplication> Find(this CommandLineApplication cmd, string fullName)
        {
            var rootNode = cmd;
            while (rootNode.Parent != null)
            {
                rootNode = rootNode.Parent;
                if (rootNode.FullName is not null && rootNode.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase))
                {
                    return Result.Ok(rootNode);
                }
            }

            return Result.Fail($"No CommandLine with full name {fullName}");
        }
    }
}
