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
using System.Diagnostics;
using Windows.Storage;
using Windows.Graphics.Display;
using System.Collections;
using Microsoft.ApplicationInsights;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrackMyActV2.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Charts : Page
    {

        public ObservableCollection<TimeTable> Demands { get; set; }
        public ObservableCollection<TimeTable> Demands_Event { get; set; }
        private timerToChartsTransfer transfer;

        public Charts()
        {
            this.InitializeComponent();
            Demands = new ObservableCollection<TimeTable>();
            VisualStateManager.GoToState(this, "Landscape", true);
            transfer = new timerToChartsTransfer();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var transfer = (timerToChartsTransfer)e.Parameter;
            var tc = new TelemetryClient();
            tc.TrackEvent("Charts Viewed");
            Demands.Clear();
            //for(int i=0; i<transfer.trackact.activity[(int)transfer.activity_pos].timer_data.Count; i++)
            //{
            //    TimeTable tb = new TimeTable();
            //    tb.eventCount = i + 1;
            //    tb.seconds = transfer.trackact.activity[(int)transfer.activity_pos].timer_data[i].time_in_seconds;
            //    Demands.Add(tb);
            //}
            DateTime dt = (DateTime.Now).AddDays(-10);
            for (int i = 0; i < transfer.trackact.activity[(int)transfer.activity_pos].timer_data.Count; i++)
            {
                TimeTable tb = new TimeTable();
                DateTime da = transfer.trackact.activity[(int)transfer.activity_pos].timer_data[i].startTime;
                tb.Demand = da.ToLocalTime();
                tb.runningMedian = transfer.trackact.activity[(int)transfer.activity_pos].timer_data[i].running_median;
                tb.seconds = transfer.trackact.activity[(int)transfer.activity_pos].timer_data[i].time_in_seconds;
                Demands.Add(tb);
            }
            //SfChart chart = new SfChart();
            myChart.HorizontalAlignment = HorizontalAlignment.Center;
            myChart.VerticalAlignment = VerticalAlignment.Center;
            myChart.Header = "Time and Median";
            myChart.Foreground = new SolidColorBrush(Colors.White);

            DateTimeCategoryAxis primaryCategoryAxis = new DateTimeCategoryAxis();
            primaryCategoryAxis.Interval = 1;
            primaryCategoryAxis.Header = "Timer Started";  
                 
            primaryCategoryAxis.LabelFormat = "dd/MM hh:mm tt";
            primaryCategoryAxis.Foreground = new SolidColorBrush(Colors.White);
            myChart.PrimaryAxis = primaryCategoryAxis;            
            NumericalAxis secondaryNumericalAxis = new NumericalAxis();
            secondaryNumericalAxis.Header = "Time (in seconds)";
            secondaryNumericalAxis.Minimum = 0;
            secondaryNumericalAxis.Foreground = new SolidColorBrush(Colors.White);
            myChart.SecondaryAxis = secondaryNumericalAxis;
            SplineSeries series2 = new SplineSeries();
            series2.ItemsSource = Demands;
            series2.ShowTooltip = true;
            series2.Label = "Median Time";
            series2.XBindingPath = "Demand";
            series2.YBindingPath = "runningMedian";
            series2.EnableAnimation = true;
            
            ColumnSeries series1 = new ColumnSeries();
            series1.EnableAnimation = true;
            series1.ShowTooltip = true;
            series1.Label = "Time (in seconds)";
            series1.ItemsSource = Demands;
            series1.XBindingPath = "Demand";
            series1.YBindingPath = "seconds";

            myChart.Series.Add(series1);
            myChart.Series.Add(series2);
            myChart.Legend = new ChartLegend() { Visibility = Visibility.Visible };
            
            
            //Hashtable datetime_ht = new Hashtable();
            //List<long> total_time_day = new List<long>();
            //datetime_ht.Clear();
            //total_time_day.Clear();
            //Demands = new ObservableCollection<GoldDemand>();
            //transfer = (timerToChartsTransfer)e.Parameter;
            //var activity = transfer.trackact.activity[(int)transfer.activity_pos];
            //int m = 0;
            //for (int l = 0; l < activity.timer_data.Count; l++)
            //{
            //    if (!datetime_ht.Contains(((activity.timer_data[l].startTime).ToLocalTime()).Date))
            //    {
            //        datetime_ht.Add(((activity.timer_data[l].startTime).ToLocalTime()).Date, m);
            //        total_time_day.Add(activity.timer_data[l].time_in_seconds);
            //        m++;
            //    }
            //    else
            //    {
            //        int pos = (int)datetime_ht[((activity.timer_data[l].startTime).ToLocalTime()).Date];
            //        total_time_day[pos] = total_time_day[pos] + activity.timer_data[l].time_in_seconds;
            //    }
            //}
            //Demands.Clear();
            //int i = 0;
            //foreach (var act in activity.timer_data)
            //{
            //    GoldDemand dem_event = new GoldDemand();
            //    if (datetime_ht.Contains(((act.startTime).ToLocalTime()).Date))
            //    {
            //        GoldDemand dem = new GoldDemand();
            //        dem.date = ((act.startTime).ToLocalTime()).Date;
            //        dem.Demand = String.Format("{0:dd/MM}", dem.date);
            //        //myChart.Header = ;

            //        //dem.Demand = i + 1;                  
            //        int pos = (int)datetime_ht[((act.startTime).ToLocalTime()).Date];
            //        dem.seconds = total_time_day[pos];
            //        datetime_ht.Remove(((act.startTime).ToLocalTime()).Date);
            //        Demands.Add(dem);
            //        //total_time_day.RemoveAt(pos);
            //    }
            //    dem_event.event_count = i;
            //    i = i + 1;
            //}
            //this.DataContext = this;
            //i = 0;
        }
        public class TimeTable
        {
            public DateTime Demand { get; set; }
            public long runningMedian { get; set; }
            public long seconds { get; set; }
          
        }
        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Charts_Loaded(object sender, RoutedEventArgs e)
        //{
        //    /* SfChart chart = new SfChart();

        //     chart.HorizontalAlignment = HorizontalAlignment.Center;
        //     chart.VerticalAlignment = VerticalAlignment.Center;
        //     chart.Header = "Time spent on " + transfer.trackact.activity[(int)transfer.activity_pos];
        //     chart.Height = 300;
        //     chart.Width = 500;

        //     NumericalAxis primaryCategoryAxis = new NumericalAxis();
        //     primaryCategoryAxis.Header = "Time (in seconds)";

        //     chart.PrimaryAxis = primaryCategoryAxis;

        //     NumericalAxis secondaryNumericalAxis = new NumericalAxis();
        //     secondaryNumericalAxis.Header = "Event";

        //     chart.SecondaryAxis = secondaryNumericalAxis;

        //     ColumnSeries series1 = new ColumnSeries();
        //     series1.EnableAnimation = true;
        //     series1.ShowTooltip = true;
        //     series1.Label = "Time (in seconds)";
        //     series1.ItemsSource = Demands;
        //     series1.XBindingPath = "Demand";
        //     series1.YBindingPath = "seconds";           

        //     chart.Series.Add(series1);

        //     chart.Legend = new ChartLegend() { Visibility = Visibility.Visible };
        //     MainGrid.Children.Add(chart); */

        //}

        //private void GoBack_Click(object sender, RoutedEventArgs e)
        //{
        //    Frame rootFrame = Window.Current.Content as Frame;
        //    Debug.WriteLine((string)ApplicationData.Current.RoamingSettings.Values["CurrentAct"]);
        //    Debug.WriteLine(ApplicationData.Current.RoamingSettings.Values["NewActivity"].ToString());
        //    if (rootFrame.CanGoBack)
        //    {
        //        rootFrame.GoBack();
        //    }
        //}
    }
}
