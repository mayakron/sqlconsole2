using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SqlConsole2
{
    public partial class MainWindow
    {
        private void Window_Closed(object sender, EventArgs e)
        {
            // Nothing to do.
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Nothing to do.
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var isCtrlDown = KeyboardUtility.IsCtrlDown();

            if (isCtrlDown)
            {
                var isAltDown = KeyboardUtility.IsAltDown();
                var isShiftDown = KeyboardUtility.IsShiftDown();

                switch (e.Key)
                {
                    case Key.B:

                        if (!isShiftDown && !isAltDown)
                        {
                            if (this.EditSetAsContextMenuItem.IsEnabled)
                            {
                                this.EditSetAsContextMenuItem_Click(sender, null);
                            }

                            e.Handled = true;

                            break;
                        }

                        break;

                    case Key.N:

                        if (!isShiftDown && !isAltDown)
                        {
                            if (this.FileNewMenuItem.IsEnabled)
                            {
                                this.FileNewMenuItem_Click(sender, null);
                            }

                            e.Handled = true;

                            break;
                        }

                        break;

                    case Key.O:

                        if (!isShiftDown && !isAltDown)
                        {
                            if (this.FileOpenMenuItem.IsEnabled)
                            {
                                this.FileOpenMenuItem_Click(sender, null);
                            }

                            e.Handled = true;

                            break;
                        }

                        break;

                    case Key.S:

                        if (!isShiftDown && !isAltDown)
                        {
                            if (this.FileSaveMenuItem.IsEnabled)
                            {
                                this.FileSaveMenuItem_Click(sender, null);
                            }

                            e.Handled = true;

                            break;
                        }

                        break;

                    case Key.W:

                        if (isShiftDown && !isAltDown)
                        {
                            if (this.EditWordWrapMenuItem.IsEnabled)
                            {
                                this.EditWordWrapMenuItem.IsChecked = !this.EditWordWrapMenuItem.IsChecked;
                            }

                            e.Handled = true;

                            break;
                        }

                        break;

                    case Key.F4:

                        if (!isShiftDown && !isAltDown)
                        {
                            if (this.SetsCloseMenuItem.IsEnabled)
                            {
                                this.SetsCloseMenuItem_Click(sender, null);
                            }

                            e.Handled = true;

                            break;
                        }

                        if (isShiftDown && !isAltDown)
                        {
                            if (this.SetsCloseAllMenuItem.IsEnabled)
                            {
                                this.SetsCloseAllMenuItem_Click(sender, null);
                            }

                            e.Handled = true;

                            break;
                        }

                        if (isShiftDown && isAltDown)
                        {
                            if (this.SetsCloseAllButActiveMenuItem.IsEnabled)
                            {
                                this.SetsCloseAllButActiveMenuItem_Click(sender, null);
                            }

                            e.Handled = true;

                            break;
                        }

                        break;

                    case Key.F5:

                        if (!isShiftDown && !isAltDown)
                        {
                            if (this.SetsExecuteQueryFromTextBoxMenuItem.IsEnabled)
                            {
                                this.SetsExecuteQueryFromTextBoxMenuItem_Click(sender, null);
                            }

                            e.Handled = true;

                            break;
                        }

                        break;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Args.Length > 0)
            {
                string filePath = App.Args[0];

                if (File.Exists(filePath))
                {
                    this.OpenSqlFile(filePath);
                }
            }

            if (App.Args.Length > 1)
            {
                this.ContextTextBox.Text = App.Args[1];
            }

            if (!string.IsNullOrEmpty(Configuration.CommonFilesDirectoryPath))
            {
                if (Directory.Exists(Configuration.CommonFilesDirectoryPath))
                {
                    MenuItem fileCommonFilesMenuItem = null;

                    foreach (var filePath in Directory.GetFiles(Configuration.CommonFilesDirectoryPath, "*.sql", SearchOption.TopDirectoryOnly).OrderBy(x => x))
                    {
                        if (fileCommonFilesMenuItem == null)
                        {
                            fileCommonFilesMenuItem = MenuItemUtility.CreateMenuItem("_Common Files...", null, "pack://siteoforigin:,,,/icons/FileOpenMenuItem.png", null);
                        }

                        fileCommonFilesMenuItem.Items.Add(MenuItemUtility.CreateMenuItem(Path.GetFileName(filePath), FileCommonFilesMenuItem_Click, "pack://siteoforigin:,,,/icons/FileOpenMenuItem.png", filePath));
                    }

                    if (fileCommonFilesMenuItem != null)
                    {
                        var insertAt = this.FileMenuItem.Items.Count - 1;

                        this.FileMenuItem.Items.Insert(insertAt, new Separator());
                        this.FileMenuItem.Items.Insert(insertAt, fileCommonFilesMenuItem);
                    }
                }
            }
        }
    }
}