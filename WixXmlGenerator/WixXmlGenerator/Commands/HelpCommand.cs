using System.Collections.Generic;

namespace WixXmlGenerator.Commands
{
    public class HelpCommand : Command, ICommand
    {
        private const string HelpString = "Insert help here...";

        public HelpCommand():base(0)
        {
            
        }

        public string Execute(List<string> args)
        {
            return HelpString;
        }
    }
}
