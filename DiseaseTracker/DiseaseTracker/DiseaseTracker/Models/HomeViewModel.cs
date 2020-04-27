using System;

namespace DiseaseTracker.Models
{
    public class HomeViewModel
    {
        public DateTime LastVisit { get; set; }
        public int TotalVisits { get; set; }
        public COVID19Statistics Statistics { get; set; }

        public HomeViewModel()
        {
        }

        public HomeViewModel(COVID19Statistics statistics, DateTime lastVisit, int totalVisits)
        {
            Statistics = statistics;
            LastVisit = lastVisit;
            TotalVisits = totalVisits;
        }
    }
}