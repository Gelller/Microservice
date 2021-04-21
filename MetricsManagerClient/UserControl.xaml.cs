using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for MaterialCards.xaml
    /// </summary>
    public partial class MaterialCards : UserControl, INotifyPropertyChanged
    {
        private MetricsFromManager _metricsFromManager = new MetricsFromManager();
        public MaterialCards()
        {
            InitializeComponent();

            ColumnServiesValues = new SeriesCollection { new ColumnSeries { Values = new ChartValues<int> { } } };
            GetMetrics();
            DataContext = this;
        }

        public SeriesCollection ColumnServiesValues { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    
        private void UpdateOnСlick(object sender, RoutedEventArgs e)
        {
            GetMetrics();
        }
        private void GetMetrics()
        {
            ColumnServiesValues[0].Values.Clear();
            var metricsFromManager = _metricsFromManager.MetricsFromTable("cpu");
            int averages = 0;
            foreach (var item in metricsFromManager.Metrics)
            {
                ColumnServiesValues[0].Values.Add(item.Value);
                averages = averages + item.Value;
            }
            PercentTextBlock.Text = Convert.ToString(averages / metricsFromManager.Metrics.Count);
        }

    }
}

