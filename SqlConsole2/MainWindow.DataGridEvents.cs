using System.Windows.Controls;

namespace SqlConsole2
{
    public partial class MainWindow
    {
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString(); if (!string.IsNullOrEmpty(header))
            {
                e.Column.Header = header.Replace("_", "__");
            }
        }
    }
}