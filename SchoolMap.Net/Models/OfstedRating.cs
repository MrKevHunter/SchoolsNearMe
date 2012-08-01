using System;

namespace SchoolMap.Net.Models
{
    public class OfstedRating
    {
        public DateTime LastInspection { get; set; }

        public int OverallEffectiveness { get; set; }

        public int PupilAchievement { get; set; }

        public int HowWellLearnerAchieve { get; set; }

        public int PupilBehaviorAndSafety { get; set; }

        public int QualityOfTeaching { get; set; }

        public int LeadershipAndManagement { get; set; }
    }
}