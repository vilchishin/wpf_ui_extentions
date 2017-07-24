using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfUiControls.ViewModels;

namespace WpfUiControls.Views
{
    /// <summary>
    /// Interaction logic for NumericUpDownControl.xaml
    /// </summary>
    public partial class NumericUpDownControl : UserControl
    {
        private NumericUpDownViewModel viewModel;

        public NumericUpDownControl()
        {
            InitializeComponent();
            viewModel = new NumericUpDownViewModel();

            DataContext = viewModel;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var command = viewModel.TextInputCommand;
            var textBox = sender as TextBox;
            var textBoxString = textBox.Text;

            string text =
                textBoxString.Substring(0, textBox.SelectionStart) +
                e.Text +
                textBoxString.Substring(textBox.SelectionStart + textBox.SelectionLength);

            if (command.CanExecute(text))
                command.Execute(text);
            else
                e.Handled = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            viewModel.LostFocusCommand.Execute(null);
        }
    }
}
