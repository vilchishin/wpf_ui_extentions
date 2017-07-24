using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfUiControls.ViewModels
{
    public class NumericUpDownViewModel : ViewModelBase
    {
        #region Private fields

        private int fontSize;
        private int decimalPlaces;
        private float increment;
        private float maxValue;
        private float minValue;
        private float value;
        private string text;

        // Actions
        private ICommand textInputCommand;
        private ICommand upButtonClickCommand;
        private ICommand downButtonClickCommand;
        private ICommand lostFocusCommand;

        #endregion

        #region Constructors

        public NumericUpDownViewModel()
        {
            decimalPlaces = 2;
            increment = 10;
            maxValue = 100;
            minValue = 0;
            Value = 50;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Size of textboxe's font.
        /// </summary>
        public int FontSize
        {
            get { return fontSize; }
            set
            {
                if (value < 1 || value > 128)
                    throw new ArgumentOutOfRangeException("FontSize", "FontSize values should be in range 1..128 inclusively.");

                fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        /// <summary>
        /// Number of digits to display after decimal point.
        /// </summary>
        public int DecimalPlaces
        {
            get { return decimalPlaces; }
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentOutOfRangeException("DecimalPlaces", "DecimalPlaces values should be in range 0..5 inclusively.");

                decimalPlaces = value;
                OnPropertyChanged("DecimalPlaces");
                // Re-call value property to redraw decimal places.
                Value = this.value;
            }
        }

        /// <summary>
        /// Number on which value in the textbox will change on button click.
        /// </summary>
        public float Increment
        {
            get { return increment; }
            set
            {
                if (value < DataTypeUtils.FZeroTolerance || value > maxValue)
                    throw new ArgumentOutOfRangeException("Increment", "Increment values should be in range 1e-6..MaxValue inclusively.");

                increment = value;
                OnPropertyChanged("Increment");
            }
        }

        public float MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }

        public float MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                OnPropertyChanged("MinValue");
            }
        }

        public float Value
        {
            get { return value; }
            set
            {
                if (value < minValue || value > maxValue)
                    throw new ArgumentOutOfRangeException("Value", "Value property should be in ranges of MinValue and MaxValue");

                this.value = value;
                text = value.ToString(String.Format("F{0}", decimalPlaces));

                OnPropertyChanged("Value");
                OnPropertyChanged("Text");
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (value == String.Empty)
                {
                    Value = MinValue;
                }
                else if (value.Equals("-", StringComparison.OrdinalIgnoreCase))
                {
                    text = value;
                    OnPropertyChanged("Text");
                }
                else
                {
                    float temp;
                    temp = float.Parse(value);  // Exception may be thrown.

                    text = value;

                    try
                    {
                        Value = temp;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Value = (temp < MinValue) ? MinValue : MaxValue;
                    }

                    OnPropertyChanged("Text");
                }
            }
        }

        public ICommand UpButtonClickCommand
        {
            get
            {
                if (upButtonClickCommand == null)
                    upButtonClickCommand = new BaseCommand(UpButtonAction);

                return upButtonClickCommand;
            }
        }

        public ICommand DownButtonClickCommand
        {
            get
            {
                if (downButtonClickCommand == null)
                    downButtonClickCommand = new BaseCommand(DownButtonAction);

                return downButtonClickCommand;
            }
        }

        public ICommand TextInputCommand
        {
            get
            {
                if (textInputCommand == null)
                    textInputCommand = new BaseCommand<string>(TextInputAction, CanExecuteTextInput);

                return textInputCommand;
            }
        }

        public ICommand LostFocusCommand
        {
            get
            {
                if (lostFocusCommand == null)
                    lostFocusCommand = new BaseCommand(LostFocusAction);

                return lostFocusCommand;
            }
        }

        #endregion

        #region Private logic

        private void UpButtonAction()
        {
            try
            {
                Value += Increment;
            }
            catch (ArgumentOutOfRangeException)
            {
                Value = MaxValue;
            }
        }

        private void DownButtonAction()
        {
            try
            {
                Value -= Increment;
            }
            catch (ArgumentOutOfRangeException)
            {
                Value = MinValue;
            }
        }

        private void TextInputAction(string e)
        {
            Text = e;
        }

        private bool CanExecuteTextInput(string e)
        {
            float dummy;

            return e.Equals("-", StringComparison.OrdinalIgnoreCase) ||
                float.TryParse(e, out dummy);
        }

        private void LostFocusAction()
        {
            Value = value;  // Re-set value to update text.
        }

        #endregion
    }
}
