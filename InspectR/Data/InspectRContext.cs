using System.Data.Entity;

namespace InspectR.Data
{
    public class InspectRContext : DbContext
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
    }
}