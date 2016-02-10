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
using TrackMyActV2.Pages;
using Windows.Storage;
using TrackMyActV2.Libraries;
using TrackMyActV2.Models;
using System.Collections.ObjectModel;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TrackMyActV2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Library library;
        public ObservableCollection<ActivityData> activity { get; set; }
        public ObservableCollection<TimerData> tmdata { get; set; }
        private RootObjectTrackAct rtrackact;
        public MainPage()
        {
            this.InitializeComponent();
            library = new Library();
            activity = new ObservableCollection<ActivityData>();
            tmdata = new ObservableCollection<TimerData>();
            rtrackact = new RootObjectTrackAct();
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values["FirstLaunch"] == null)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(TimerPage));
            }
            else
            {
                if(await library.checkIfFileExists("activityDB"))
                {
                    ApplicationData.Current.LocalSettings.Values["FirstLaunch"] = false;
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
                else
                {
                    ApplicationData.Current.LocalSettings.Values["FirstLaunch"] = false;
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(TimerPage));
                }
                               
            }
        }

        private void dataListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ActivityData actd = (ActivityData)e.ClickedItem;
            Frame rootFrame = Window.Current.Content as Frame;
            ApplicationData.Current.LocalSettings.Values["NewActivity"] = false;
            rootFrame.Navigate(typeof(TimerPage), actd.name);            
        }

        private void addNewAct_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["NewActivity"] = true;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(TimerPage));
        }
    }

    public class BooleanToVisibilityConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((string)value == "")
            {
                return Visibility.Collapsed;
            }
            else if (value == null)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }

}
