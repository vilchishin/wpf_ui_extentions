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

        private Func<float> toNumericCalculator;
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

            MaxValue = 100;
            MinValue = 0;
            SmallChange = 1;
            LargeChange = 10;

            CalculateFuncs();
        }

        #endregion

        #region Properties

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

        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("MaxValue", "MaxValue can't be less than or equal to zero.");

                maxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }

        public int MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                OnPropertyChanged("MinValue");
            }
        }

        public int LargeChange
        {
            get { return largeChange; }
            set
            {
                if (value < smallChange)
                    throw new ArgumentOutOfRangeException("LargeChange", "LargeChange can't be less than SmallChange");

                largeChange = value;
                OnPropertyChanged("LargeChange");
            }
        }

        public int SmallChange
        {
            get { return smallChange; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("SmallChange", "SmallChange can't be less than or equal to zero.");

                smallChange = value;
                OnPropertyChanged("SmallChange");
            }
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (value < minValue || value > maxValue)
                    throw new ArgumentOutOfRangeException("Value", "Value must be between MinValue and MaxValue");

                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        #endregion

        #region Private logic

        private void CalculateFuncs()
        {
            CalculateToNumeric();
            CalculateToSlider();
        }

        private void CalculateToNumeric()
        {

        }

        private void CalculateToSlider()
        {

        }

        #endregion
    }
}
