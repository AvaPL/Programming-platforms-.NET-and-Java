using System;

namespace DiseaseTracker.Models
{
    public class StatisticsViewModel
    {
        public DateTime LastVisit { get; set; }
        public int TotalVisits { get; set; }
        public COVID19Statistics Statistics { get; set; }

        public StatisticsViewModel()
        {
        }

        public StatisticsViewModel(COVID19Statistics statistics, DateTime lastVisit, int totalVisits)
        {
            Statistics = statistics;
            LastVisit = lastVisit;
            TotalVisits = totalVisits;
        }
    }
}