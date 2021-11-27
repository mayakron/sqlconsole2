using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace SqlConsole2
{
    partial class MainWindow : Window
    {
        private const string WindowTitle = "SQL Console";

        private string currentFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        public string CurrentFilePath
        {
            get
            {
                return this.currentFilePath;
            }

            set
            {
                this.currentFilePath = value;

                this.Title = ((value != null) ? value + " - " : string.Empty) + WindowTitle;
            }
        }

        public void OpenSqlFile(string filePath)
        {
            var queryTextBoxContents = File.ReadAllText(filePath);

            using (var disabledProcessing = this.Dispatcher.DisableProcessing())
            {
                this.QueryTextBox.Text = queryTextBoxContents;

                this.ContextTextBox.Text = string.Empty;

                this.CurrentFilePath = filePath;

                this.ReportStatusUpdate($"File \"{filePath}\" has been opened successfully.");

                this.QueryTextBox.Focus();
            }
        }

        public void PerformAction(Action tryAction, Action catchAction = null, Action finallyAction = null)
        {
            try
            {
                tryAction();
            }
            catch (Exception ex)
            {
                catchAction?.Invoke();

                this.ReportError(ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        public void PerformActionAsync(Action tryAction, Action catchAction = null, Action finallyAction = null)
        {
            using (var worker = new BackgroundWorker())
            {
                worker.DoWork += delegate (object doWorkSender, DoWorkEventArgs doWorkEventArgs)
                {
                    try
                    {
                        tryAction();
                    }
                    catch (Exception ex)
                    {
                        catchAction?.Invoke();

                        this.Dispatcher.Invoke(new Action(() => { this.ReportError(ex); }));
                    }
                    finally
                    {
                        finallyAction?.Invoke();
                    }
                };

                worker.RunWorkerAsync();
            }
        }

        public void ReportError(Exception ex)
        {
            this.ReportStatusUpdate(ex.GetType().ToString());

            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        public void ReportStatusUpdate(string text)
        {
            this.StatusBarTextBlock.Text = $"{text} @ {DateTime.Now:yyyy-MM-dd HH':'mm':'ss'.'fff}";
        }
    }
}