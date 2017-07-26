using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUiControls.ViewModels
{
    public class ExtendedSliderViewModel : ViewModelBase
    {
        #region Private fields

        private NumericUpDownViewModel numericViewModel;

        private int numericControlWidth;
        private int maxValue;
        private int minValue;
        private int largeChange;
        private int smallChange;
        private int value;

        private Func<double> multiplier;
        private Func<double> toNumericCalculator;
        private Func<int> toSliderCalculator;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor that creates a valid instance.
        /// </summary>
        public ExtendedSliderViewModel(NumericUpDownViewModel numericViewModel)
        {
            this.numericViewModel = numericViewModel;

            NumericControlWidth = 80;

            SetRange();
            value = toSliderCalculator();

            numericViewModel.ValueChanged += NumericValueChanged;
        }

        #endregion

        #region Properties

        /// <summary>
        /// View model of the numeric control that works as a base for the Slider control.
        /// </summary>
        public NumericUpDownViewModel NumericViewModel
        {
            get { return numericViewModel; }
        }

        /// <summary>
        /// Indicates whether numeric control is reversed.
        /// </summary>
        public bool IsReversed 
        {
            get { return NumericViewModel.IsReversed; }
        }

        /// <summary>
        /// Width of the numeric control.
        /// </summary>
        public int NumericControlWidth
        {
            get { return numericControlWidth; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("NumericControlWidth", "NumericControlWidth can't be less than or equal to zero.");

                numericControlWidth = value;
                OnPropertyChanged("NumericControlWidth");
            }
        }

        /// <summary>
        /// Maximum value that Slider can get.
        /// </summary>
        public int MaxValue
        {
            get { return maxValue; }
            private set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("MaxValue", "MaxValue can't be less than or equal to zero.");

                maxValue = value;
                OnPropertyChanged("MaxValue");
                CalculateFuncs();
            }
        }

        /// <summary>
        /// Minimum possible value the Slider can get.
        /// </summary>
        public int MinValue
        {
            get { return minValue; }
            private set
            {
                minValue = value;
                OnPropertyChanged("MinValue");
                CalculateFuncs();
            }
        }

        /// <summary>
        /// Number on which value changes when user clicks on the slider line.
        /// </summary>
        public int LargeChange
        {
            get { return largeChange; }
            private set
            {
                if (value < smallChange)
                    throw new ArgumentOutOfRangeException("LargeChange", "LargeChange can't be less than SmallChange");

                largeChange = value;
                OnPropertyChanged("LargeChange");
            }
        }

        /// <summary>
        /// Number on which value changes when user pulls slider bar.
        /// </summary>
        public int SmallChange
        {
            get { return smallChange; }
            private set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("SmallChange", "SmallChange can't be less than or equal to zero.");

                smallChange = value;
                OnPropertyChanged("SmallChange");
            }
        }

        /// <summary>
        /// Current value of the slider.
        /// </summary>
        public int Value
        {
            get { return value; }
            set
            {
                if (value < MinValue || value > MaxValue)
                    throw new ArgumentOutOfRangeException("Value", "Value must be between MinValue and MaxValue");

                this.value = value;
                OnPropertyChanged("Value");

                numericViewModel.SetValueWithoutRaisingEvent(toNumericCalculator());
            }
        }

        #endregion

        #region Private logic

        /// <summary>
        /// Sets ranges of the slider according to the Numeric View model.
        /// </summary>
        private void SetRange()
        {
            var numIncrement = NumericViewModel.Increment;
            int iterations = 0;

            while ((int)numIncrement == 0)
            {
                numIncrement *= 10;
                iterations++;
            }

            int factor = (int)Math.Pow(10, iterations);

            MaxValue = factor * 
                (NumericViewModel.IsReversed ? (int)NumericViewModel.MinValue : (int)NumericViewModel.MaxValue);
            MinValue = factor *
                (NumericViewModel.IsReversed ? (int)NumericViewModel.MaxValue : (int)NumericViewModel.MinValue);
            LargeChange = (int)Math.Abs(numIncrement);
            SmallChange = 1;
        }

        /// <summary>
        /// Calculates conversion functions to bind values of slider and numeric controls.
        /// </summary>
        private void CalculateFuncs()
        {
            CalculateMultiplier();
            CalculateToNumeric();
            CalculateToSlider();
        }

        private void CalculateMultiplier()
        {
            multiplier = new Func<double>(() =>
                Math.Round(((numericViewModel.MaxValue - numericViewModel.MinValue) / (MaxValue - MinValue)),
                numericViewModel.DecimalPlaces, MidpointRounding.AwayFromZero)
            );
        }

        private void CalculateToNumeric()
        {
            toNumericCalculator = new Func<double>(() => Value * multiplier());
        }

        private void CalculateToSlider()
        {
            toSliderCalculator = new Func<int>(() => (int)Math.Round(numericViewModel.Value / multiplier(), 0));
        }

        /// <summary>
        /// Callback that handles the event of the numeric value changing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">New value.</param>
        private void NumericValueChanged(object sender, double e)
        {
            Value = toSliderCalculator();
        }

        #endregion
    }
}
