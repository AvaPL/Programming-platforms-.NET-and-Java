using System;
using System.Net;

namespace DiseaseTracker.Models
{
    public class Visitor
    {
        public int Id { get; set; }
        public IPAddress Ip { get; set; }
        public DateTime LastVisit { get; set; }
        public int TotalVisits { get; set; }
    }
}