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

            string text = ((TextBox)sender).Text + e.Text;

            if (command.CanExecute(text))
                command.Execute(text);
            else
                e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.Text = ((TextBox)sender).Text;
        }
    }
}
