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
        private int numberOfSteps = 1;
        private double increment;
        private double maxValue = 1;
        private double minValue;
        private double value;
        private string text;

        // Actions
        private ICommand textInputCommand;
        private ICommand upButtonClickCommand;
        private ICommand downButtonClickCommand;
        private ICommand lostFocusCommand;

        #endregion

        #region Events

        public EventHandler<double> ValueChanged;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether numeric control works in reversed mode.
        /// </summary>
        public bool IsReversed
        {
            get { return increment < 0; }
        }

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
                StateChanged();
            }
        }

        /// <summary>
        /// Number of times user has to click certain button to move from 
        /// one side of the numeric range to the other one.
        /// </summary>
        public int NumberOfSteps
        {
            get { return numberOfSteps; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("NumberOfSteps", "NumberOfSteps can't be less than one.");

                numberOfSteps = value;

                Increment = Math.Round((MaxValue - MinValue) / numberOfSteps, decimalPlaces);
                StateChanged();
            }
        }

        /// <summary>
        /// Number on which value in the textbox will change on button click.
        /// </summary>
        public double Increment
        {
            get { return increment; }
            private set
            {
                if (Math.Abs(value) < DataTypeUtils.FZeroTolerance || Math.Abs(value) > Math.Abs(MaxValue - MinValue))
                    throw new ArgumentOutOfRangeException("Increment", "Increment values should be in range 1e-6..MaxValue inclusively.");

                increment = value;
                OnPropertyChanged("Increment");
            }
        }

        public double MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged("MaxValue");
                StateChanged();
            }
        }

        public double MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                OnPropertyChanged("MinValue");
                StateChanged();
            }
        }

        public double Value
        {
            get { return value; }
            set
            {
                if (!IsReversed && (value < MinValue || value > MaxValue) ||
                    IsReversed && (value > MinValue || value < MaxValue))
                    throw new ArgumentOutOfRangeException("Value", "Value property should be in ranges of MinValue and MaxValue");

                this.value = value;
                text = value.ToString(String.Format("F{0}", decimalPlaces));

                OnPropertyChanged("Value");
                OnPropertyChanged("Text");

                RaiseOnValueChanged();
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

        #region Public logic

        public void SetValueWithoutRaisingEvent(double value)
        {
            if (!IsReversed && (value < MinValue || value > MaxValue) ||
                IsReversed && (value > MinValue || value < MaxValue))
                throw new ArgumentOutOfRangeException("value", "value should be in ranges of MinValue and MaxValue");

            this.value = value;
            text = value.ToString(String.Format("F{0}", decimalPlaces));

            OnPropertyChanged("Value");
            OnPropertyChanged("Text");
        }

        #endregion

        #region Private logic

        private void StateChanged()
        {
            Increment = Math.Round((MaxValue - MinValue) / NumberOfSteps, DecimalPlaces);

            try
            {
                // Initiate value refresh.
                Value = value;
            }
            catch (ArgumentOutOfRangeException)
            {
                Value = (value > MaxValue) ? MaxValue : MinValue;
            }
        }

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
            double dummy;

            return e.Equals("-", StringComparison.OrdinalIgnoreCase) ||
                double.TryParse(e, out dummy);
        }

        private void LostFocusAction()
        {
            Value = value;  // Re-set value to update text.
        }

        private void RaiseOnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, Value);
        }

        #endregion
    }
}
