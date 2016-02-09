using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrackMyActV2.Models
{
    public class TrackAct
    {
        public static RootObjectTrackAct trackactDataDeserializer(string response)
        {
            RootObjectTrackAct rtrackact = JsonConvert.DeserializeObject<RootObjectTrackAct>(response);
            return rtrackact;
        }

        public static string trackactSerializer(RootObjectTrackAct rtrackact)
        {
            string response = JsonConvert.SerializeObject(rtrackact);
            return response;
        }
    }
    public class formatTimeData
    {
        public long pos;
        public string time_in_string;
        public string datetime;
    }
    [DataContract]
    public class TimerData
    {
        [DataMember]
        public long time_in_seconds { get; set; }
        [DataMember]
        public int position { get; set; }   // keep track of in which order the position was added.
        [DataMember]
        public DateTime startTime { get; set; }
        [DataMember]
        public DateTime endTime { get; set; }
    }

    [DataContract]
    public class ActivityData
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string median { get; set; }
        [DataMember]
        public string ninetypercentile { get; set; }
        [DataMember]
        public string personal_best { get; set; }
        [DataMember]
        public DateTime createdTime { get; set; }
        [DataMember]
        public string lastattempt { get; set; }
        [DataMember]
        public long totalTime { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public List<TimerData> timer_data { get; set; }
    }

    [DataContract]
    public class RootObjectTrackAct
    {
        [DataMember]
        public List<ActivityData> activity { get; set; }
    }


}
