//stock.cs
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace Stocks
{
    public class Stock
    {
        public event EventHandler<StockNotification> StockEvent;

        private string _name; //Name of our stock.
        private int _initialValue; //Starting value of the stock.
        private int _maxChange; //Max change of the stock that is possible.
        private int _threshold; //Threshold value where we notify subscribers to the event.
        private int _numChanges; //Amount of changes the stock goes through.
        private int _currentValue; //Current value of the stock.

        private readonly Thread _thread;
        public string StockName { get => _name; set => _name = value; }
        public int InitialValue { get => _initialValue; set => _initialValue = value; }
        public int CurrentValue { get => _currentValue; set => _currentValue = value; }
        public int MaxChange { get => _maxChange; set => _maxChange = value; }
        public int Threshold { get => _threshold; set => _threshold = value; }
        public int NumChanges { get => _numChanges; set => _numChanges = value; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stock class that contains all the information and changes of the stock
        /// </summary>
        /// <param name="name">Stock name</param>
        /// <param name="startingValue">Starting stock value</param>
        /// <param name="maxChange">The max value change of the stock</param>
        /// <param name="threshold">The range for the stock</param>
        public Stock(string name, int startingValue, int maxChange, int threshold)
        {
            _name = name;
            _initialValue = startingValue;
            _currentValue = InitialValue;
            _maxChange = maxChange;
            _threshold = threshold;
            this._thread = new Thread(new ThreadStart(Activate));
            _thread.Start();
        }

        //-----------------------------------------------------------------------------------
        /// <summary>
        /// Activates the threads synchronizations
        /// </summary>
        public void Activate()
        {
            for (int i = 0; i < 25; i++)
            {
                Thread.Sleep(500); // 1/2 second
                ChangeStockValue();
            }
        }

        //--------------------------------------------------------------------------------------
        // delegate
        //public delegate void StockNotification(String stockName, int currentValue, int numberChanges);
        // event
        //public event StockNotification ProcessComplete;
        //------------------------------------------------------------------------------------------
        /// <summary>
        /// Changes the stock value and also raising the event of stock value changes
        /// </summary>
        public void ChangeStockValue()
        {
            //Console.WriteLine("Changing stock value for " + StockName);
            var rand = new Random();
            CurrentValue += rand.Next(0, MaxChange);
            NumChanges++;

            //Console.WriteLine($"{CurrentValue} - {InitialValue} > {Threshold} ??? ==> {(CurrentValue - InitialValue) > Threshold}"); //+ (CurrentValue - InitialValue)
            if ((CurrentValue - InitialValue) > Threshold)
            { //RAISE THE EVENT
                //Console.WriteLine("Raising event for " + StockName);
                StockEvent?.Invoke(this, new StockNotification(StockName, CurrentValue, NumChanges));
            }
        }
    }
}

