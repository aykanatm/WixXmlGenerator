using System;
using System.Collections.Generic;
using System.Linq;
using WixXmlGenerator.Commands;

namespace WixXmlGenerator.Services
{
    public static class CommandParser
    {
        public static string Parse(List<string> args)
        {
            try
            {
                string result;

                var command = args.First();

                if (command == Statics.Commands.Help)
                {
                    var helpCommand = new HelpCommand();
                    var numberOfArgs = helpCommand.GetNumberOfArgs();
                    if (numberOfArgs == args.Count - 1)
                    {
                        result = helpCommand.Execute(new List<string>());
                    }
                    else
                    {
                        throw new Exception("Command not recognized. Help command can be used as 'WixXmlGenerator -help'");
                    }
                }
                else if (command == Statics.Commands.Version)
                {
                    var versionCommand = new VersionCommand();
                    var numberOfArgs = versionCommand.GetNumberOfArgs();
                    if (numberOfArgs == args.Count - 1)
                    {
                        result = versionCommand.Execute(new List<string>());
                    }
                    else
                    {
                        throw new Exception("Command not recognized. Version command can be used as 'WixXmlGenerator -version'");
                    }
                }
                else if (command == Statics.Commands.Generate)
                {
                    var generateCommand = new GenerateCommand();
                    var numberOfArgs = generateCommand.GetNumberOfArgs();
                    if (numberOfArgs == args.Count - 1)
                    {
                        result = generateCommand.Execute(args);
                    }
                    else
                    {
                        throw new Exception("Command not recognized. Generate command can be used as 'WixXmlGenerator -generate -sourceDir <SOURCE_DIRECTORY_PATH> -wxsDir <WXS_FILE_FOLDER> -projectName <PROJECT_NAME> -outputFile <OUTPUT_FILE_PATH>'");
                    }
                }
                else
                {
                    throw new Exception("Command not recognized. Please use 'WixXmlGenerator -help' to see the usage.");
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
