using System;
using System.Collections.Generic;
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
using TrackMyActV2.Models;
using TrackMyActV2.Libraries;
using System.Collections.ObjectModel;
using System.Diagnostics;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrackMyActV2.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllTheData : Page
    {
        public ObservableCollection<ActivityData> activity { get; set; }
        public ObservableCollection<TimerData> tmdata { get; set; }
        public Library library;
        private RootObjectTrackAct rtrackact;
        public AllTheData()
        {
            this.InitializeComponent();
            library = new Library();
            activity = new ObservableCollection<ActivityData>();
            tmdata = new ObservableCollection<TimerData>();
            rtrackact = new RootObjectTrackAct();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (await library.checkIfFileExists("activityDB"))
            {
                string fileres = await library.readFile("activityDB");
                rtrackact = TrackAct.trackactDataDeserializer(fileres);
                var activityD = rtrackact.activity;
                foreach (var actv in activityD)
                {
                    activity.Add(actv);
                }
                var timedata = rtrackact.activity[0].timer_data;
                foreach (var tdata in timedata)
                {
                    tmdata.Add(tdata);
                }
            }
        }

        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Chart Button clicked");
            Frame rootFrame = Window.Current.Content as Frame;
            //rootFrame.Navigate(typeof(Charts), rtrackact);
        }

        private void dataListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ActivityData actdata = (ActivityData)e.ClickedItem;
            Frame rootFrame = Window.Current.Content as Frame;
            //rootFrame.Navigate(typeof(AllTheActivityData), actdata);
        }

    }
}
