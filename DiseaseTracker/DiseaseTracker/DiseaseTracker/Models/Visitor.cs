using System;

namespace DiseaseTracker.Models
{
    public class Visitor
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public DateTime LastVisit { get; set; }
        public int TotalVisits { get; set; }
    }
}