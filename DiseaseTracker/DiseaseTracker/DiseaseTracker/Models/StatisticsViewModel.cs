using System;

namespace DiseaseTracker.Models
{
    public class StatisticsViewModel
    {
        public DateTime LastVisit { get; }
        public int TotalVisits { get; }
        public COVID19Statistics Statistics { get; }

        public StatisticsViewModel(COVID19Statistics statistics, DateTime lastVisit, int totalVisits)
        {
            Statistics = statistics;
            LastVisit = lastVisit;
            TotalVisits = totalVisits;
        }
    }
}