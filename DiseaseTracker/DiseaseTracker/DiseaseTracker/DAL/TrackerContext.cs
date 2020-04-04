using System.Data.Entity;
using DiseaseTracker.Models;

namespace DiseaseTracker.DAL
{
    public class TrackerContext : DbContext
    {
        public DbSet<Visitor> Visitors { get; set; }
    }
}