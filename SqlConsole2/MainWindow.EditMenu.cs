using System.Text.RegularExpressions;
using System.Windows;

namespace SqlConsole2
{
    public partial class MainWindow
    {
        private void EditSetAsContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedText = this.QueryTextBox.SelectedText;

            if (string.IsNullOrEmpty(selectedText))
            {
                return;
            }

            var filteredSelectedText = Regex.Replace(selectedText, "(\r\n|\n|\r)+", string.Empty, RegexOptions.Singleline);

            if (string.IsNullOrEmpty(filteredSelectedText))
            {
                return;
            }

            this.ContextTextBox.Text = filteredSelectedText;
        }

        private void EditUndoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.QueryTextBox.Undo();
        }

        private void EditWordWrapMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            this.QueryTextBox.TextWrapping = TextWrapping.Wrap;
        }

        private void EditWordWrapMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            this.QueryTextBox.TextWrapping = TextWrapping.NoWrap;
        }
    }
}