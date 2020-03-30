namespace DiseaseTracker.Models
{
    public class COVID19Statistics
    {
        public class LatestStatistics
        {
            public long Confirmed { get; set; }
            public long Deaths { get; set; }
            public long Recovered { get; set; }
        }

        public LatestStatistics Latest { get; set; }
    }
}