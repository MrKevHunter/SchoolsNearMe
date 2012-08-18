using System;

namespace SchoolsNearMe.Models
{
    public class DumperStats
    {
        public int Indexes
        {
            get;
            set;
        }
        public int Documents
        {
            get;
            set;
        }
        public int Attachments
        {
            get;
            set;
        }
        public TimeSpan Elapsed
        {
            get;
            set;
        }
    }
}