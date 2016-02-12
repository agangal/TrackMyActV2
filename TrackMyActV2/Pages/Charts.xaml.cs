using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TrackMyActV2.Models;
using Syncfusion.UI.Xaml.Charts;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrackMyActV2.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Charts : Page
    {
       
        public ObservableCollection<GoldDemand> Demands { get; set; }
        private timerToChartsTransfer transfer;

        public Charts()
        {
            this.InitializeComponent();
            transfer = new timerToChartsTransfer();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Demands = new ObservableCollection<GoldDemand>();
            transfer = (timerToChartsTransfer)e.Parameter;
            var activity = transfer.trackact.activity[(int)transfer.activity_pos];
            Demands.Clear();
            int i = 0;
            foreach(var act in activity.timer_data)
            {
                GoldDemand dem = new GoldDemand();
                dem.Demand = i + 1;
                dem.seconds = act.time_in_seconds;
                Demands.Add(dem);
                i++;
            }
            this.DataContext = this;
            i = 0;
        }
        public class GoldDemand
        {
            public int Demand { get; set; }
            public long seconds { get; set; }
            
        }
        /// <summary>
         ///
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void Charts_Loaded(object sender, RoutedEventArgs e)
        {
           /* SfChart chart = new SfChart();
            
            chart.HorizontalAlignment = HorizontalAlignment.Center;
            chart.VerticalAlignment = VerticalAlignment.Center;
            chart.Header = "Time spent on " + transfer.trackact.activity[(int)transfer.activity_pos];
            chart.Height = 300;
            chart.Width = 500;

            NumericalAxis primaryCategoryAxis = new NumericalAxis();
            primaryCategoryAxis.Header = "Time (in seconds)";

            chart.PrimaryAxis = primaryCategoryAxis;

            NumericalAxis secondaryNumericalAxis = new NumericalAxis();
            secondaryNumericalAxis.Header = "Event";

            chart.SecondaryAxis = secondaryNumericalAxis;

            ColumnSeries series1 = new ColumnSeries();
            series1.EnableAnimation = true;
            series1.ShowTooltip = true;
            series1.Label = "Time (in seconds)";
            series1.ItemsSource = Demands;
            series1.XBindingPath = "Demand";
            series1.YBindingPath = "seconds";           

            chart.Series.Add(series1);
         
            chart.Legend = new ChartLegend() { Visibility = Visibility.Visible };
            MainGrid.Children.Add(chart); */
            
        }        

    }
}
