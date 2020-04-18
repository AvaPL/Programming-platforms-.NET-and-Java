using System;

namespace DiseaseTracker.Models
{
    public class Visitor
    {
        public Visitor(string ip)
        {
            Ip = ip;
            LastVisit = DateTime.Now;
            TotalVisits = 1;
        }

        public Visitor()
        {
        }

        public int Id { get; set; }
        public string Ip { get; set; }
        public DateTime LastVisit { get; set; }
        public int TotalVisits { get; set; }

        public void UpdateVisitor()
        {
            LastVisit = DateTime.Now;
            TotalVisits += 1;
        }
    }
}