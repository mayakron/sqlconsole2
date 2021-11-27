using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace SqlConsole2
{
    public partial class MainWindow
    {
        private void HelpProjectPageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/mayakron/sqlconsole2");
        }

        private void HelpAboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"SqlConsole2 is an application for running queries on a variety of data sources.\n\nCurrent version is {Assembly.GetExecutingAssembly().GetName().Version}.", "About SqlConsole2", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}