using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyActV2.Models;
using System.Diagnostics;
using Windows.Storage;

namespace TrackMyActV2.Libraries
{
    public class Library
    {
        private int countLimit;
        public Library()
        {
            countLimit = 300;
        }
        public async Task<bool> checkIfFileExists(string filename)
        {
            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(filename);
            if (item == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Reads and returns a string containing the contents of file "filename"
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<string> readFile(string filename)
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localFolder = applicationData.LocalFolder;
            string response = null;
            try
            {
                Debug.WriteLine("reading file : " + filename);
                StorageFile sampleFile = await localFolder.GetFileAsync(filename);
                response = await FileIO.ReadTextAsync(sampleFile);
            }
            catch (System.UnauthorizedAccessException e)
            {
                Debug.WriteLine("In reading file :" + filename + " : System Unauthorized exception : " + e);
            }
            return response;
        }
        public string convertSecondsToString(long seconds)
        {            
            string res = (((seconds / 3600) > 0) ? ((seconds / 3600).ToString() + " h ") : "") + ((((seconds / 60) % 60) > 0) ? (((seconds / 60) % 60).ToString() + " m ") : "") + (((seconds % 60) > 0) ? ((seconds % 60).ToString() + " s") : "");
            return res;
        }
        /// <summary>
        /// Writes the "response" to the "filename" file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<bool> writeFile(string filename, string response)
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localFolder = applicationData.LocalFolder;
            Debug.WriteLine("In writeFile : " + filename);
            try
            {
                //Debug.WriteLine("In try of write.");
                StorageFile sampleFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, response);
            }
            catch (System.UnauthorizedAccessException e)
            {
                Debug.WriteLine("in write to file : " + filename + " : " + e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method to delete a file with name "filename"
        /// </summary>
        /// <param name="filename"></param>
        public async void deleteFile(string filename)
        {
            Library library = new Library();
            if (await library.checkIfFileExists(filename))
            {
                StorageFile sampleFile = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                await sampleFile.DeleteAsync(StorageDeleteOption.Default);
            }
        }

        /// <summary>
        /// It's not being used anymore. I divided up it's functionality and moved it to the TimerPage
        /// </summary>
        /// <param name="timerText"></param>
        /// <param name="timerdata"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        public async Task<bool> updateDB(string timerText, TimeSpan timerdata, string activityName)
        {
            bool res = await checkIfFileExists("activityDB");
            RootObjectTrackAct rtrackact = new RootObjectTrackAct();
            if (res)
            {
                string response = await readFile("activityDB");
                rtrackact = TrackAct.trackactDataDeserializer(response);
                int activity_pos = -1;
                for (int i = 0; i < rtrackact.activity.Count; i++)
                {
                    if (rtrackact.activity[i].name == activityName)
                    {
                        activity_pos = i;
                    }
                }
                /// If the activity exists 
                if (activity_pos != -1)
                {
                    TimerData tdata = new TimerData();
                    tdata.position = rtrackact.activity[activity_pos].timer_data[rtrackact.activity[activity_pos].timer_data.Count - 1].position + 1; // The mumbo jumbo is to get the value of 'position' in the last element in the track_data list and adding 1 to it.
                    if (tdata.position >= countLimit)
                    {
                        rtrackact.activity[activity_pos].timer_data.RemoveAt(0);
                    }
                    tdata.time_in_seconds = (long)timerdata.TotalSeconds;

                    SortedSet<long> time_in_seconds = new SortedSet<long>();
                    for (int i = 0; i < rtrackact.activity[activity_pos].timer_data.Count; i++)
                    {
                        time_in_seconds.Add(rtrackact.activity[activity_pos].timer_data[i].time_in_seconds);
                    }
                    time_in_seconds.Add((long)timerdata.TotalSeconds);
                    long mediansec = (time_in_seconds.ElementAtOrDefault(time_in_seconds.Count / 2));//time_in_seconds[time_in_seconds.Count / 2];
                    rtrackact.activity[activity_pos].median = String.Format("{0:00}:{1:00}:{2:00}", mediansec / 3600, (mediansec / 60) % 60, mediansec % 60);
                    int pos = (int)(0.9 * (time_in_seconds.Count - 1) + 1); // 0 1 3 4 5 8
                    long ninentypercentilesecond = (time_in_seconds.ElementAtOrDefault(pos));
                    rtrackact.activity[activity_pos].ninetypercentile = String.Format("{0:00}:{1:00}:{2:00}", ninentypercentilesecond / 3600, (ninentypercentilesecond / 60) % 60, ninentypercentilesecond % 60);
                    long personal_best = (time_in_seconds.ElementAtOrDefault(time_in_seconds.Count - 1));
                    rtrackact.activity[activity_pos].personal_best = String.Format("{0:00}:{1:00}:{2:00}", (personal_best) / 3600, ((personal_best) / 60) % 60, (personal_best) % 60);
                    rtrackact.activity[activity_pos].timer_data.Add(tdata);
                    //findMedian(rtrackact.activity[activity_pos]);
                }
                /// If the activity does not exist
                else
                {
                    ActivityData ractivitydata = new ActivityData();
                    ractivitydata.name = activityName;
                    TimerData tdata = new TimerData();
                    tdata.position = 0;             // Since this is a new activity, it won't have any data already associated with it.
                    tdata.time_in_seconds = (long)timerdata.TotalSeconds;
                    ractivitydata.timer_data.Add(tdata);
                    ractivitydata.personal_best = String.Format("{0:00}:{1:00}:{2:00}", timerdata.Hours, (timerdata.Minutes), (long)timerdata.Seconds);
                    ractivitydata.median = String.Format("{0:00}:{1:00}:{2:00}", timerdata.Hours, timerdata.Minutes, (long)timerdata.Seconds);
                    ractivitydata.ninetypercentile = String.Format("{0:00}:{1:00}:{2:00}", timerdata.Hours, (timerdata.Minutes), (long)timerdata.Seconds);
                    rtrackact.activity.Add(ractivitydata);
                }
            }
            else
            {
                ActivityData ractivitydata = new ActivityData();
                ractivitydata.name = activityName;
                TimerData tdata = new TimerData();
                tdata.position = 0;             // Since this is a new activity, it won't have any data already associated with it.
                tdata.time_in_seconds = (long)timerdata.TotalSeconds;
                ractivitydata.timer_data = new List<TimerData>();
                ractivitydata.timer_data.Add(tdata);
                rtrackact.activity = new List<ActivityData>();
                ractivitydata.personal_best = String.Format("{0:00}:{1:00}:{2:00}", timerdata.Hours, (timerdata.Minutes), (long)timerdata.Seconds);
                ractivitydata.median = String.Format("{0:00}:{1:00}:{2:00}", timerdata.Hours, (timerdata.Minutes), (long)timerdata.Seconds);
                ractivitydata.ninetypercentile = String.Format("{0:00}:{1:00}:{2:00}", timerdata.Hours, (timerdata.Minutes), (long)timerdata.Seconds);
                rtrackact.activity.Add(ractivitydata);
            }
            try
            {
                await writeFile("activityDB", TrackAct.trackactSerializer(rtrackact));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            Debug.WriteLine("DB update finished at : " + DateTime.Now.Millisecond);
            return true;
        }

        private void updateDBFileExists(string timerText, TimeSpan timerdata, int activity_pos)
        {
            if (activity_pos != -1)
            {
                TimerData tdata = new TimerData();
                //tdata.position = 
            }
        }

    }
}
