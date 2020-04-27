using System;

namespace DiseaseTracker.Models
{
    public class Visitor
    {
        public Visitor(string ip, long confirmed)
        {
            Ip = ip;
            LastVisit = DateTime.Now;
            TotalVisits = 1;
            LastConfirmed = confirmed;
        }

        public Visitor()
        {
        }

        public int Id { get; set; }
        public string Ip { get; set; }
        public DateTime LastVisit { get; set; }
        public int TotalVisits { get; set; }
        public long LastConfirmed { get; set; }

        public void UpdateVisitor(long confirmed)
        {
            LastVisit = DateTime.Now;
            TotalVisits += 1;
            LastConfirmed = confirmed;
        }
    }
}