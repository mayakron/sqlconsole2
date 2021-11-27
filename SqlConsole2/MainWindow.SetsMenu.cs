using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SqlConsole2
{
    public partial class MainWindow
    {
        private static Regex ContextParser = new Regex("^ProviderType={{(.*?)}};ConnectionString={{(.*?)}};CommandTimeout={{(.*?)}}$", RegexOptions.None);

        private static string SetsExportSetToCsvFileMenuItem_BooleanFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; return (bool)value ? "1" : "0";
        }

        private static string SetsExportSetToCsvFileMenuItem_ByteArrayFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; return Convert.ToBase64String((byte[])value);
        }

        private static string SetsExportSetToCsvFileMenuItem_DateTimeFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; DateTime dateTimeValue = (DateTime)value; if ((dateTimeValue.Hour > 0) || (dateTimeValue.Minute > 0) || (dateTimeValue.Second > 0) || (dateTimeValue.Millisecond > 0)) return dateTimeValue.ToString("yyyy-MM-dd HH':'mm':'ss.fff"); else return dateTimeValue.ToString("yyyy-MM-dd");
        }

        private static string SetsExportSetToCsvFileMenuItem_PassthroughFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; return Convert.ToString(value);
        }

        private static string SetsExportSetToCsvFileMenuItem_StringFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; return Convert.ToString(value).TrimEnd().Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"");
        }

        private static string SetsExportSetToTsvFileMenuItem_BooleanFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; return (bool)value ? "1" : "0";
        }

        private static string SetsExportSetToTsvFileMenuItem_ByteArrayFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; return Convert.ToBase64String((byte[])value);
        }

        private static string SetsExportSetToTsvFileMenuItem_DateTimeFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; DateTime dateTimeValue = (DateTime)value; if ((dateTimeValue.Hour > 0) || (dateTimeValue.Minute > 0) || (dateTimeValue.Second > 0) || (dateTimeValue.Millisecond > 0)) return dateTimeValue.ToString("yyyy-MM-dd HH':'mm':'ss.fff"); else return dateTimeValue.ToString("yyyy-MM-dd");
        }

        private static string SetsExportSetToTsvFileMenuItem_PassthroughFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; return Convert.ToString(value);
        }

        private static string SetsExportSetToTsvFileMenuItem_StringFieldFormatter(object value)
        {
            if (SetsFieldIsNull(value)) return string.Empty; return Convert.ToString(value).TrimEnd().Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
        }

        private static bool SetsFieldIsNull(object value)
        {
            return value is DBNull || value == null;
        }

        private string SetsAddSetsFromDataSet(DataSet dataSet, bool selectFirstDataSet = true)
        {
            int newTabItemStartIndex = 1;

            for (int i = 1; i < this.TabControl.Items.Count; i++)
            {
                var oldTabItemInfo = ((TabItem)this.TabControl.Items[i]).Tag as TabItemInfo;

                if (oldTabItemInfo != null)
                {
                    if (oldTabItemInfo.Index >= newTabItemStartIndex)
                    {
                        newTabItemStartIndex = oldTabItemInfo.Index + 1;
                    }
                }
            }

            using (var disabledProcessing = this.Dispatcher.DisableProcessing())
            {
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    var newTabItem = new TabItem();

                    newTabItem.Header = "Set " + (((newTabItemStartIndex + i) < 10) ? "_" : string.Empty) + (newTabItemStartIndex + i);

                    var newTabDataGrid = new DataGrid();

                    newTabDataGrid.GridLinesVisibility = DataGridGridLinesVisibility.Vertical;
                    newTabDataGrid.AlternatingRowBackground = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    newTabDataGrid.IsReadOnly = true;
                    newTabDataGrid.CanUserAddRows = false;
                    newTabDataGrid.CanUserDeleteRows = false;
                    newTabDataGrid.SelectionUnit = DataGridSelectionUnit.CellOrRowHeader;
                    newTabDataGrid.ContextMenu = this.FindResource("DataGridContextMenu") as ContextMenu;
                    newTabDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
                    newTabDataGrid.ItemsSource = dataSet.Tables[i].DefaultView;

                    // It is not possible with DataGrid default style to horizontal scroll it to view all column headers if the DataGrid does not have any rows.
                    // Therefore the following HACK...

                    if (dataSet.Tables[i].Rows.Count > 0)
                    {
                        newTabItem.Content = newTabDataGrid;
                    }
                    else
                    {
                        var newTabOuterScroller = new ScrollViewer();

                        newTabOuterScroller.Content = newTabDataGrid;
                        newTabOuterScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                        newTabOuterScroller.Style = new Style(typeof(ScrollViewer), newTabOuterScroller.Style) { Triggers = { new DataTrigger { Binding = new Binding("HasItems") { Source = newTabDataGrid }, Value = false, Setters = { new Setter(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Visible) } } } };

                        newTabItem.Content = newTabOuterScroller;
                    }

                    newTabItem.Tag = new TabItemInfo { Index = newTabItemStartIndex + i, DataTable = dataSet.Tables[i] };

                    this.TabControl.Items.Add(newTabItem);

                    if (selectFirstDataSet)
                    {
                        if (i == 0)
                        {
                            newTabItem.IsSelected = true;
                        }
                    }
                }
            }

            return dataSet.Tables.Count + " sets, " + string.Join(", ", dataSet.Tables.Cast<DataTable>().Select(dataTable => dataTable.Rows.Count + " rows"));
        }

        private void SetsCloseAllButActiveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.TabControl.Items.Count <= 1)
            {
                return;
            }

            var tabItemIndex = this.TabControl.SelectedIndex;

            using (var disabledProcessing = this.Dispatcher.DisableProcessing())
            {
                var i = this.TabControl.Items.Count - 1; while (i > 0)
                {
                    if (i != tabItemIndex)
                    {
                        this.TabControl.Items.RemoveAt(i);
                    }

                    i--;
                }
            }

            this.ReportStatusUpdate("All sets closed successfully.");
        }

        private void SetsCloseAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.TabControl.Items.Count <= 1)
            {
                return;
            }

            using (var disabledProcessing = this.Dispatcher.DisableProcessing())
            {
                for (int i = this.TabControl.Items.Count - 1; i > 0; i--)
                {
                    this.TabControl.Items.RemoveAt(i);
                }
            }

            this.ReportStatusUpdate("All sets closed successfully.");
        }

        private void SetsCloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.TabControl.Items.Count <= 1)
            {
                return;
            }

            var tabItemIndex = this.TabControl.SelectedIndex;

            if (!(tabItemIndex > 0))
            {
                return;
            }

            var tabItemHeader = ((TabItem)this.TabControl.Items[tabItemIndex]).Header;

            this.TabControl.Items.RemoveAt(tabItemIndex);

            this.ReportStatusUpdate(((string)tabItemHeader).Replace("_", string.Empty) + " closed successfully.");
        }

        private void SetsExecuteQueryDisableAll()
        {
            this.SetsExecuteQueryFromTextBoxMenuItem.IsEnabled = false;
            this.SetsExecuteQueryFromClipboardMenuItem.IsEnabled = false;
        }

        private void SetsExecuteQueryEnableAll()
        {
            this.SetsExecuteQueryFromTextBoxMenuItem.IsEnabled = true;
            this.SetsExecuteQueryFromClipboardMenuItem.IsEnabled = true;
        }

        private void SetsExecuteQueryFromClipboardMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PerformAction
            (
                tryAction: () =>
                {
                    var commandText = System.Windows.Clipboard.GetText();

                    if (string.IsNullOrWhiteSpace(commandText))
                    {
                        throw new Exception("No text from the clipboard has been found.");
                    }

                    var contextMatch = ContextParser.Match(this.ContextTextBox.Text);

                    if (!contextMatch.Success)
                    {
                        throw new Exception("The format of the context text box is invalid. It should be:\nProviderType={{...}};ConnectionString={{...}};CommandTimeout={{...}}");
                    }

                    var providerType = contextMatch.Groups[1].Captures[0].Value;
                    var connectionString = contextMatch.Groups[2].Captures[0].Value;
                    int commandTimeout; if (!int.TryParse(contextMatch.Groups[3].Captures[0].Value, out commandTimeout)) commandTimeout = 120;

                    this.ReportStatusUpdate("Query execution in progress...");

                    this.SetsExecuteQueryDisableAll();

                    PerformActionAsync
                    (
                        tryAction: () =>
                        {
                            var stopwatch = new Stopwatch();

                            stopwatch.Start();

                            var dataSet = DataEngine.ExecuteCommand(providerType, connectionString, commandText, commandTimeout);

                            stopwatch.Stop();

                            this.Dispatcher.Invoke
                            (
                                new Action(() =>
                                {
                                    if ((dataSet != null) && (dataSet.Tables != null) && (dataSet.Tables.Count > 0))
                                    {
                                        var dataSetDescription = this.SetsAddSetsFromDataSet(dataSet);

                                        this.ReportStatusUpdate("Query execution completed successfully in " + stopwatch.ElapsedMilliseconds + " msecs (" + dataSetDescription + ").");
                                    }
                                    else
                                    {
                                        this.ReportStatusUpdate("Query execution completed successfully in " + stopwatch.ElapsedMilliseconds + " msecs.");
                                    }
                                })
                            );
                        },
                        catchAction: null,
                        finallyAction: () =>
                        {
                            this.Dispatcher.Invoke
                            (
                                new Action(() =>
                                {
                                    this.SetsExecuteQueryEnableAll();
                                })
                            );
                        }
                    );
                }
            );
        }

        private void SetsExecuteQueryFromTextBoxMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PerformAction
            (
                tryAction: () =>
                {
                    var commandText = this.QueryTextBox.SelectedText;

                    if (string.IsNullOrWhiteSpace(commandText))
                    {
                        throw new Exception("No text from the query text box has been selected for execution.");
                    }

                    var contextMatch = ContextParser.Match(this.ContextTextBox.Text);

                    if (!contextMatch.Success)
                    {
                        throw new Exception("The format of the context text box is invalid. It should be:\nProviderType={{...}};ConnectionString={{...}};CommandTimeout={{...}}");
                    }

                    var providerType = contextMatch.Groups[1].Captures[0].Value;
                    var connectionString = contextMatch.Groups[2].Captures[0].Value;
                    int commandTimeout; if (!int.TryParse(contextMatch.Groups[3].Captures[0].Value, out commandTimeout)) commandTimeout = 120;

                    this.ReportStatusUpdate("Query execution in progress...");

                    this.SetsExecuteQueryDisableAll();

                    PerformActionAsync
                    (
                        tryAction: () =>
                        {
                            var stopwatch = new Stopwatch();

                            stopwatch.Start();

                            var dataSet = DataEngine.ExecuteCommand(providerType, connectionString, commandText, commandTimeout);

                            stopwatch.Stop();

                            this.Dispatcher.Invoke
                            (
                                new Action(() =>
                                {
                                    if ((dataSet != null) && (dataSet.Tables != null) && (dataSet.Tables.Count > 0))
                                    {
                                        var dataSetDescription = this.SetsAddSetsFromDataSet(dataSet);

                                        this.ReportStatusUpdate("Query execution completed successfully in " + stopwatch.ElapsedMilliseconds + " msecs (" + dataSetDescription + ").");
                                    }
                                    else
                                    {
                                        this.ReportStatusUpdate("Query execution completed successfully in " + stopwatch.ElapsedMilliseconds + " msecs.");
                                    }
                                })
                            );
                        },
                        catchAction: null,
                        finallyAction: () =>
                        {
                            this.Dispatcher.Invoke
                            (
                                new Action(() =>
                                {
                                    this.SetsExecuteQueryEnableAll();
                                })
                            );
                        }
                    );
                }
            );
        }

        private void SetsExportSetToCsvFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var tabItemIndex = this.TabControl.SelectedIndex;

            if (!(tabItemIndex > 0))
            {
                return;
            }

            var tabItemInfo = (((TabItem)this.TabControl.Items[tabItemIndex]).Tag) as TabItemInfo;

            if (tabItemInfo == null)
            {
                return;
            }

            var dialog = new SaveFileDialog();

            dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            dialog.Title = "Export Set To Csv File As...";

            if (dialog.ShowDialog() == true)
            {
                this.ReportStatusUpdate("Export in progress...");

                PerformActionAsync
                (
                    tryAction: () =>
                    {
                        var table = tabItemInfo.DataTable;

                        using (var streamWriter = new StreamWriter(dialog.FileName, false, Encoding.UTF8))
                        {
                            int columnsCount = table.Columns.Count; if (columnsCount > 0)
                            {
                                var fieldLabels = new string[columnsCount];

                                for (int i = 0; i < columnsCount; i++)
                                {
                                    fieldLabels[i] = table.Columns[i].ColumnName;
                                }

                                var fieldFormatters = new Func<object, string>[columnsCount];

                                for (int i = 0; i < columnsCount; i++)
                                {
                                    var type = table.Columns[i].DataType;

                                    if (type.Equals(typeof(string)))
                                    {
                                        fieldFormatters[i] = SetsExportSetToCsvFileMenuItem_StringFieldFormatter;
                                    }
                                    else if (type.Equals(typeof(bool)))
                                    {
                                        fieldFormatters[i] = SetsExportSetToCsvFileMenuItem_BooleanFieldFormatter;
                                    }
                                    else if (type.Equals(typeof(DateTime)))
                                    {
                                        fieldFormatters[i] = SetsExportSetToCsvFileMenuItem_DateTimeFieldFormatter;
                                    }
                                    else if (type.Equals(typeof(byte[])))
                                    {
                                        fieldFormatters[i] = SetsExportSetToCsvFileMenuItem_ByteArrayFieldFormatter;
                                    }
                                    else
                                    {
                                        fieldFormatters[i] = SetsExportSetToCsvFileMenuItem_PassthroughFieldFormatter;
                                    }
                                }

                                for (int i = 0; i < columnsCount; i++) { if (i > 0) streamWriter.Write(","); streamWriter.Write("\"" + fieldLabels[i] + "\""); }

                                streamWriter.WriteLine();

                                foreach (DataRow dataRow in table.Rows)
                                {
                                    for (int i = 0; i < columnsCount; i++) { if (i > 0) streamWriter.Write(","); streamWriter.Write("\"" + fieldFormatters[i](dataRow[i]) + "\""); }

                                    streamWriter.WriteLine();
                                }
                            }
                        }

                        this.Dispatcher.Invoke
                        (
                            new Action(() =>
                            {
                                this.ReportStatusUpdate("Export completed successfully.");
                            })
                        );
                    }
                );
            }
        }

        private void SetsExportSetToExcelFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var tabItemIndex = this.TabControl.SelectedIndex;

            if (!(tabItemIndex > 0))
            {
                return;
            }

            var tabItemInfo = (((TabItem)this.TabControl.Items[tabItemIndex]).Tag) as TabItemInfo;

            if (tabItemInfo == null)
            {
                return;
            }

            var dialog = new SaveFileDialog();

            dialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            dialog.Title = "Export Set To Excel File As...";

            if (dialog.ShowDialog() == true)
            {
                this.ReportStatusUpdate("Export in progress...");

                PerformActionAsync
                (
                    tryAction: () =>
                    {
                        var table = tabItemInfo.DataTable;

                        if (File.Exists(dialog.FileName))
                        {
                            File.Delete(dialog.FileName);
                        }

                        using (var excelPackage = new ExcelPackage(new FileInfo(dialog.FileName)))
                        {
                            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                            int columnsCount = table.Columns.Count; if (columnsCount > 0)
                            {
                                for (int i = 0; i < columnsCount; i++)
                                {
                                    worksheet.Cells[1, i + 1].Value = table.Columns[i].ColumnName;
                                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                                }
                            }

                            int rowIndex = 1; foreach (DataRow dataRow in table.Rows)
                            {
                                rowIndex++; for (int i = 0; i < columnsCount; i++)
                                {
                                    worksheet.Cells[rowIndex, i + 1].Value = dataRow[i];
                                }
                            }

                            excelPackage.Save();
                        }

                        this.Dispatcher.Invoke
                        (
                            new Action(() =>
                            {
                                this.ReportStatusUpdate("Export completed successfully.");
                            })
                        );
                    }
                );
            }
        }

        private void SetsExportSetToTsvFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var tabItemIndex = this.TabControl.SelectedIndex;

            if (!(tabItemIndex > 0))
            {
                return;
            }

            var tabItemInfo = (((TabItem)this.TabControl.Items[tabItemIndex]).Tag) as TabItemInfo;

            if (tabItemInfo == null)
            {
                return;
            }

            var dialog = new SaveFileDialog();

            dialog.Filter = "TSV files (*.tsv)|*.tsv|All files (*.*)|*.*";
            dialog.Title = "Export Set To Tsv File As...";

            if (dialog.ShowDialog() == true)
            {
                this.ReportStatusUpdate("Export in progress...");

                PerformActionAsync
                (
                    tryAction: () =>
                    {
                        var table = tabItemInfo.DataTable;

                        using (var streamWriter = new StreamWriter(dialog.FileName, false, Encoding.UTF8))
                        {
                            int columnsCount = table.Columns.Count; if (columnsCount > 0)
                            {
                                var fieldLabels = new string[columnsCount];

                                for (int i = 0; i < columnsCount; i++)
                                {
                                    fieldLabels[i] = table.Columns[i].ColumnName;
                                }

                                var fieldFormatters = new Func<object, string>[columnsCount];

                                for (int i = 0; i < columnsCount; i++)
                                {
                                    var type = table.Columns[i].DataType;

                                    if (type.Equals(typeof(string)))
                                    {
                                        fieldFormatters[i] = SetsExportSetToTsvFileMenuItem_StringFieldFormatter;
                                    }
                                    else if (type.Equals(typeof(bool)))
                                    {
                                        fieldFormatters[i] = SetsExportSetToTsvFileMenuItem_BooleanFieldFormatter;
                                    }
                                    else if (type.Equals(typeof(DateTime)))
                                    {
                                        fieldFormatters[i] = SetsExportSetToTsvFileMenuItem_DateTimeFieldFormatter;
                                    }
                                    else if (type.Equals(typeof(byte[])))
                                    {
                                        fieldFormatters[i] = SetsExportSetToTsvFileMenuItem_ByteArrayFieldFormatter;
                                    }
                                    else
                                    {
                                        fieldFormatters[i] = SetsExportSetToTsvFileMenuItem_PassthroughFieldFormatter;
                                    }
                                }

                                for (int i = 0; i < columnsCount; i++) { if (i > 0) streamWriter.Write("\t"); streamWriter.Write(fieldLabels[i]); }

                                streamWriter.WriteLine();

                                foreach (DataRow dataRow in table.Rows)
                                {
                                    for (int i = 0; i < columnsCount; i++) { if (i > 0) streamWriter.Write("\t"); streamWriter.Write(fieldFormatters[i](dataRow[i])); }

                                    streamWriter.WriteLine();
                                }
                            }
                        }

                        this.Dispatcher.Invoke
                        (
                            new Action(() =>
                            {
                                this.ReportStatusUpdate("Export completed successfully.");
                            })
                        );
                    }
                );
            }
        }

        private void SetsExportSetToXmlFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var tabItemIndex = this.TabControl.SelectedIndex;

            if (!(tabItemIndex > 0))
            {
                return;
            }

            var tabItemInfo = (((TabItem)this.TabControl.Items[tabItemIndex]).Tag) as TabItemInfo;

            if (tabItemInfo == null)
            {
                return;
            }

            var dialog = new SaveFileDialog();

            dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            dialog.Title = "Export Set To Xml File As...";

            if (dialog.ShowDialog() == true)
            {
                this.ReportStatusUpdate("Export in progress...");

                PerformActionAsync
                (
                    tryAction: () =>
                    {
                        tabItemInfo.DataTable.DataSet.WriteXml(dialog.FileName, XmlWriteMode.WriteSchema);

                        this.Dispatcher.Invoke
                        (
                            new Action(() =>
                            {
                                this.ReportStatusUpdate("Export completed successfully.");
                            })
                        );
                    }
                );
            }
        }

        private void SetsImportSetFromXmlFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            dialog.Title = "Import Set From Xml File As...";

            if (dialog.ShowDialog() == true)
            {
                this.ReportStatusUpdate("Import in progress...");

                PerformActionAsync
                (
                    tryAction: () =>
                    {
                        var dataSet = new DataSet();

                        dataSet.ReadXml(dialog.FileName, XmlReadMode.ReadSchema);

                        this.Dispatcher.Invoke
                        (
                            new Action(() =>
                            {
                                if ((dataSet != null) && (dataSet.Tables != null) && (dataSet.Tables.Count > 0))
                                {
                                    this.SetsAddSetsFromDataSet(dataSet);
                                }

                                this.ReportStatusUpdate("Import completed successfully.");
                            })
                        );
                    }
                );
            }
        }
    }
}