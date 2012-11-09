using System.Data.Entity.Migrations;

namespace InspectR.Migrations
{
    public partial class requestinfo_entry : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestInfoes", "InspectorInfo_Id", "dbo.InspectorInfoes");
            DropIndex("dbo.RequestInfoes", new[] { "InspectorInfo_Id" });
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
            
            DropTable("dbo.RequestInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RequestInfoes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InspectorId = c.Guid(nullable: false),
                        Client_HostAddress = c.String(),
                        Client_HostName = c.String(),
                        Client_Agent = c.String(),
                        ContentLength = c.Int(nullable: false),
                        ContentType = c.String(),
                        HttpMethod = c.String(),
                        RequestType = c.String(),
                        RawUrl = c.String(),
                        QueryString = c.String(),
                        UrlReferrer = c.String(),
                        InspectorInfo_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.RequestInfoEntries");
            CreateIndex("dbo.RequestInfoes", "InspectorInfo_Id");
            AddForeignKey("dbo.RequestInfoes", "InspectorInfo_Id", "dbo.InspectorInfoes", "Id");
        }
    }
}
