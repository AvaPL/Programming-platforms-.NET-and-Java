using System.Data.Entity;
using DiseaseTracker.Models;

namespace DiseaseTracker.DAL
{
    public class TrackerContext : DbContext
    {
        public virtual DbSet<Visitor> Visitors { get; set; }
    }
}