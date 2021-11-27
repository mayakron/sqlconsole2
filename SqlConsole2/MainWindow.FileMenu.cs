using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SqlConsole2
{
    public partial class MainWindow
    {
        private void FileCommonFilesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;

            if (menuItem == null)
            {
                return;
            }

            if (menuItem.Tag == null)
            {
                return;
            }

            var filePath = menuItem.Tag as string;

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            PerformAction
            (
                tryAction: () =>
                {
                    this.OpenSqlFile(filePath);
                }
            );
        }

        private void FileExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void FileNewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.ContextTextBox.Text = string.Empty;

            this.QueryTextBox.Text = string.Empty;

            this.CurrentFilePath = null;

            this.ReportStatusUpdate("New file has been created successfully.");

            this.QueryTextBox.Focus();
        }

        private void FileOpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            dialog.Title = "Open File...";

            if (dialog.ShowDialog() == true)
            {
                PerformAction
                (
                    tryAction: () =>
                    {
                        this.OpenSqlFile(dialog.FileName);
                    }
                );
            }
        }

        private void FileSaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();

            dialog.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            dialog.Title = "Save File As...";

            if (dialog.ShowDialog() == true)
            {
                PerformAction
                (
                    tryAction: () =>
                    {
                        File.WriteAllText(dialog.FileName, this.QueryTextBox.Text);

                        this.CurrentFilePath = dialog.FileName;

                        this.ReportStatusUpdate("File \"" + this.CurrentFilePath + "\" has been saved successfully.");
                    }
                );
            }
        }

        private void FileSaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.CurrentFilePath))
            {
                this.FileSaveAsMenuItem_Click(sender, e);

                return;
            }

            PerformAction
            (
                tryAction: () =>
                {
                    File.WriteAllText(this.CurrentFilePath, this.QueryTextBox.Text);

                    this.ReportStatusUpdate("File \"" + this.CurrentFilePath + "\" has been saved successfully.");
                }
            );
        }
    }
}