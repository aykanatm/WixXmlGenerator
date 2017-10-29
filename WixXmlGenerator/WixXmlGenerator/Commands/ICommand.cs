using System.Collections.Generic;

namespace WixXmlGenerator.Commands
{
    interface ICommand
    {
        string Execute(List<string> args);
    }
}
