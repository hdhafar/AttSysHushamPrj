using System;

namespace AttSysHushamPrj.Models
{
    public enum EventTypes
    {
        IN = 1,
        OUT = 2
    }
    public class Event
    {

        public Guid EventID { get; set; }
        public DateTime EventDate  { get; set; }
        public  TimeSpan EventTime { get; set; }

        public int EventType { get; set; }

        public int? BadgeNum { get; set; }
        public  bool? IsLate { get; set; }
        public bool? IsEarly { get; set; }
        public String Justification { get; set; }
        public int? CreateBy { get; set; }
        
    }
}
