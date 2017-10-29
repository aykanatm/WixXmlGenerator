using System.Collections.Generic;

namespace WixXmlGenerator.Commands
{
    public class GenerateCommand : Command, ICommand
    {
        public GenerateCommand() : base(4)
        {

        }

        public string Execute(List<string> args)
        {
            return "Executed.";
        }
    }
}
