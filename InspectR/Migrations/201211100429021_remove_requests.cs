namespace InspectR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_requests : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.RequestInfoEntries");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RequestInfoEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InspectorId = c.Guid(nullable: false),
                        Data = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
