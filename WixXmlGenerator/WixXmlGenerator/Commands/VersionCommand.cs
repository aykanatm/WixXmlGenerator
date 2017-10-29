using System.Collections.Generic;

namespace WixXmlGenerator.Commands
{
    public class VersionCommand : Command, ICommand
    {
        private const string Version = "Wix XML Generator v1.0.0 copyright Murat Aykanat";

        public VersionCommand() : base(0)
        {
            
        }

        public string Execute(List<string> args)
        {
            return Version;
        }
    }
}
