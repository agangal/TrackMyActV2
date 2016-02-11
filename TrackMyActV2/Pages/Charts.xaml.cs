using System;
using System.Collections.Generic;
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
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrackMyActV2.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Charts : Page
    {
        public Charts()
        {
            this.InitializeComponent();
            Loaded += Charts_Loaded;
        }

        public class temp
        {
            public DateTime seconds;
            public int count;
        }
        /// <summary>
        /// Code taken from : https://social.msdn.microsoft.com/Forums/en-US/e5a7955c-0a14-4ec8-99cb-b2dfe178cf4b/charting-datetimeaxis-range?forum=silverlightcontrols
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Charts_Loaded(object sender, RoutedEventArgs e)
        {
            //myChart.DataContext = new KeyValuePair<int, DateTime>[]
            //{
            //    new KeyValuePair<int, DateTime>(1, DateTime.Today.AddSeconds(10)),
            //    new KeyValuePair<int, DateTime>(2, DateTime.Today.AddSeconds(50)),
            //    new KeyValuePair<int, DateTime>(3, DateTime.Today.AddSeconds(30))
            //};
            
            List<temp> templist = new List<temp>();
            templist.Add(new temp() { seconds = DateTime.Today.AddSeconds(20), count = 1 });
            templist.Add(new temp() { seconds = DateTime.Today.AddSeconds(10), count = 2 });
            templist.Add(new temp() { seconds = DateTime.Today.AddSeconds(6), count = 3 });
            templist.Add(new temp() { seconds = DateTime.Today.AddSeconds(100), count = 4 });
            (myChart.Series[0] as ColumnSeries).ItemsSource = templist;
            DateTimeAxis axis = (DateTimeAxis)myChart.Axes[0];
            axis.Minimum = DateTime.Today.AddSeconds(10);
            axis.Maximum = DateTime.Today.AddSeconds(100);
            axis.Interval = 5;
            axis.IntervalType = DateTimeIntervalType.Seconds;
        }

        

    }
}
