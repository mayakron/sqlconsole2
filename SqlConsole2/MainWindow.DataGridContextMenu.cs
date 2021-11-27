using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SqlConsole2
{
    public partial class MainWindow
    {
        private void TryViewSelectedCellAs(string tempFileExtension, bool isBinaryFormat = false)
        {
            var tabItemIndex = this.TabControl.SelectedIndex;

            if (!(tabItemIndex > 0))
            {
                return;
            }

            var currentCell = ((DataGrid)(((TabItem)this.TabControl.Items[tabItemIndex]).Content)).CurrentCell;

            if (currentCell == null)
            {
                return;
            }

            var currentCellValue = ((DataRowView)currentCell.Item).Row.ItemArray[currentCell.Column.DisplayIndex];

            if (currentCellValue == null)
            {
                return;
            }

            var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + tempFileExtension);

            if (isBinaryFormat)
            {
                File.WriteAllBytes(tempFilePath, (byte[])currentCellValue);
            }
            else
            {
                File.WriteAllText(tempFilePath, currentCellValue.ToString());
            }

            Process.Start(tempFilePath);
        }

        private void ViewSelectedCellAsBmpDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".bmp", true);
        }

        private void ViewSelectedCellAsDocDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".doc", true);
        }

        private void ViewSelectedCellAsDocxDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".docx", true);
        }

        private void ViewSelectedCellAsGifDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".gif", true);
        }

        private void ViewSelectedCellAsHtmDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".htm");
        }

        private void ViewSelectedCellAsJpgDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".jpg", true);
        }

        private void ViewSelectedCellAsPdfDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".pdf", true);
        }

        private void ViewSelectedCellAsPngDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".png", true);
        }

        private void ViewSelectedCellAsPptDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".ppt", true);
        }

        private void ViewSelectedCellAsPptxDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".pptx", true);
        }

        private void ViewSelectedCellAsTxtDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".txt");
        }

        private void ViewSelectedCellAsXlsDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".xls", true);
        }

        private void ViewSelectedCellAsXlsxDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".xlsx", true);
        }

        private void ViewSelectedCellAsXmlDataGridContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryViewSelectedCellAs(".xml");
        }
    }
}