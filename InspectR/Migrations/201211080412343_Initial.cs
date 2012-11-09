using System.Data.Entity.Migrations;

namespace InspectR.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InspectorInfoes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UniqueKey = c.String(),
                        IsPrivate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InspectorInfoes", t => t.InspectorInfo_Id)
                .Index(t => t.InspectorInfo_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RequestInfoes", new[] { "InspectorInfo_Id" });
            DropForeignKey("dbo.RequestInfoes", "InspectorInfo_Id", "dbo.InspectorInfoes");
            DropTable("dbo.RequestInfoes");
            DropTable("dbo.InspectorInfoes");
        }
    }
}
