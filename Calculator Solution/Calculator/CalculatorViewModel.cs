using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace Calculator
{
    public class CalculatorViewModel : ViewModelBase
    {
        public int CurrentValue
        {
            get { return this._currentValue; }
            set
            {
                if (this._currentValue == value) { return; }
                this._currentValue = value;
                this.NotifyOfPropertyChange(() => this.CurrentValue);
            }
        }
        private int _currentValue;

        public int StoredValue
        {
            get { return this._storedValue; }
            set
            {
                if (this._storedValue == value) { return; }
                this._storedValue = value;
                this.NotifyOfPropertyChange(() => this.StoredValue);
            }
        }
        private int _storedValue;

        private Operation _pendingOperation;

        public string CurrentTime
        {
            get { return this._currentTime; }
            private set
            {
                if (this._currentTime == value) { return; }
                this._currentTime = value;
                this.NotifyOfPropertyChange(() => this.CurrentTime);
            }
        }
        private string _currentTime;

        public ICommand KeyCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand SubtractCommand { get; private set; }
        public ICommand MultiplyCommand { get; private set; }
        public ICommand DivideCommand { get; private set; }
        public ICommand EquateCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand AddHoursCommand { get; private set; }
        public ICommand AddMinutesCommand { get; private set; }
        public ICommand AddSecondsCommand { get; private set; }
        public ICommand AddRandomCommand { get; private set; }

        private IRandom _random;

        public CalculatorViewModel(IRandom random)
        {
            // Initializing values and resources.
            this._random = random;
            this.CurrentValue = 0;
            this.StoredValue = 0;
            this._pendingOperation = Operation.None;

            // This timer ticks once per second and updates CurrentTime.
            Timer timer = new Timer(1000);
            timer.Elapsed += 
                (_, __) =>
                {
                    this.CurrentTime = DateTime.Now.ToLongTimeString(); 
                };
            timer.Start();

            // Setting commands.
            this.KeyCommand = new RelayCommand(this.AppendKeys);
            this.AddCommand = new RelayCommand(this.Add);
            this.SubtractCommand = new RelayCommand(this.Subtract);
            this.MultiplyCommand = new RelayCommand(this.Multiply);
            this.DivideCommand = new RelayCommand(this.Divide);
            this.EquateCommand = new RelayCommand(this.Equate);
            this.BackCommand = new RelayCommand(this.Back);
            this.AddHoursCommand = new RelayCommand(this.AddHours);
            this.AddMinutesCommand = new RelayCommand(this.AddMinutes);
            this.AddSecondsCommand = new RelayCommand(this.AddSeconds);
            this.AddRandomCommand = new RelayCommand(this.AddRandom);
        }

        private void AppendKeys(object keyString)
        {
            foreach (char key in keyString.ToString())
            {
                if (!char.IsDigit(key))
                {
                    throw new ArgumentException("Invalid key", "key");                    
                }

                this.CurrentValue = (this.CurrentValue * 10) + (int)char.GetNumericValue(key);
            }
        }

        private void Add(object _)
        {
            this.Calculate();
            this._pendingOperation = Operation.Add;
            this.StoredValue = this.CurrentValue;
            this.CurrentValue = 0;
        }

        private void Subtract(object _)
        {
            this.Calculate();
            this._pendingOperation = Operation.Subtract;
            this.StoredValue = this.CurrentValue;
            this.CurrentValue = 0;
        }

        private void Multiply(object _)
        {
            this.Calculate();
            this._pendingOperation = Operation.Multiply;
            this.StoredValue = this.CurrentValue;
            this.CurrentValue = 0;
        }

        private void Divide(object _)
        {
            this.Calculate();
            this._pendingOperation = Operation.Divide;
            this.StoredValue = this.CurrentValue;
            this.CurrentValue = 0;
        }

        private void Equate(object _)
        {
            this.Calculate();
            this.StoredValue = 0;
        }

        private void AddHours(object _)
        {
            this.CurrentValue += DateTime.Now.Hour;
        }

        private void AddMinutes(object _)
        {
            this.CurrentValue += DateTime.Now.Minute;
        }

        private void AddSeconds(object _)
        {
            this.CurrentValue += DateTime.Now.Second;
        }

        private void AddRandom(object _)
        {
            this.CurrentValue += this._random.GetRandomNumber();
        }

        private void Calculate()
        {
            switch (this._pendingOperation)
            {
                case Operation.Add:
                    this.CurrentValue = this.StoredValue + this.CurrentValue;
                    break;
                case Operation.Subtract:
                    this.CurrentValue = this.StoredValue - this.CurrentValue;
                    break;
                case Operation.Multiply:
                    this.CurrentValue = this.StoredValue * this.CurrentValue;
                    break;
                case Operation.Divide:
                    this.CurrentValue = this.StoredValue / this.CurrentValue;
                    break;
                case Operation.None: return;
            }

            this._pendingOperation = Operation.None;
        }

        private void Back(object _)
        {
            if (this.CurrentValue == 0)
            {
                return;
            }

            this.CurrentValue = this.CurrentValue / 10;
        }
    }
}
