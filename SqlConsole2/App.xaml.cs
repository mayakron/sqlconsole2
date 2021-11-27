using System;
using System.Linq;
using System.Windows;

namespace SqlConsole2
{
    public partial class App : Application
    {
        public static readonly string[] Args = Environment.GetCommandLineArgs().Skip(1).ToArray();
    }
}