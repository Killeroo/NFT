using System.IO;

namespace Octodiff.CommandLine.Support
{
    interface ICommand
    {
        void GetHelp(TextWriter writer);
        MemoryStream Execute(string[] commandLineArguments, MemoryStream ms = null);
    }
}
