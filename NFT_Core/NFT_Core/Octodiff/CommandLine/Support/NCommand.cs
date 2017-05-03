using System.IO;

namespace Octodiff.CommandLine.Support
{
    interface NCommand
    {
        void GetHelp(TextWriter writer);
        MemoryStream Execute(string[] commandLineArguments);
    }
}