namespace InspectR.Data
{
    using System.Data.Entity;

    using InspectR.Models;

    public class InspectRContext : ApplicationDbContext
    {
        public InspectRContext()
            : base("DefaultConnection")
        {
        }

        public InspectRContext(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public DbSet<InspectorInfo> Inspectors { get; set; }

        public DbSet<InspectRUserProfile> UserProfiles { get; set; }
    }
}
