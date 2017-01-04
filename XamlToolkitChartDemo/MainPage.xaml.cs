using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XamlToolkitChartDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ChartViewModel model = new ChartViewModel();
        private DispatcherTimer _timer;

        public MainPage()
        {
            var rng = new Random();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (o, e) =>
            {
                model.AppendValue(rng.Next(20, 40));
            };
            _timer.Start();

            this.DataContext = model;
            this.InitializeComponent();
        }
    }

    public class ChartViewModel : INotifyPropertyChanged
    {
        private DateTime _minimum;
        private DateTime _maximum;
        private readonly int _seconds = 60;

        public ChartViewModel()
        {
            UpdateMinimumAndMaximum();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<KeyValuePair<DateTime, double>> Items { get; private set; }
            = new ObservableCollection<KeyValuePair<DateTime, double>>();

        public DateTime Minimum
        {
            get { return _minimum; }
            set { _minimum = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Minimum")); }
        }

        public DateTime Maximum
        {
            get { return _maximum; }
            set { _maximum = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Maximum")); }
        }

        public void AppendValue(double value)
        {
            Items.Add(new KeyValuePair<DateTime, double>(DateTime.Now, value));
            UpdateMinimumAndMaximum();

            foreach (var item in Items.Where(i => i.Key < Minimum).ToList())
            {
                Items.Remove(item);
            }
        }

        private void UpdateMinimumAndMaximum()
        {
            Maximum = DateTime.Now;
            Minimum = DateTime.Now - TimeSpan.FromSeconds(_seconds);
        }
    }
}
