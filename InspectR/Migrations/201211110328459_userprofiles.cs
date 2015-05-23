namespace InspectR.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class userprofiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InspectRUserProfiles",
                c => new
                         {
                             Id = c.Guid(nullable: false),
                             UserName = c.String()
                         })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.InspectorInfoes", "InspectRUserProfile_Id", c => c.Guid());
            AddForeignKey("dbo.InspectorInfoes", "InspectRUserProfile_Id", "dbo.InspectRUserProfiles", "Id");
            CreateIndex("dbo.InspectorInfoes", "InspectRUserProfile_Id");
        }

        public override void Down()
        {
            DropIndex("dbo.InspectorInfoes", new[] { "InspectRUserProfile_Id" });
            DropForeignKey("dbo.InspectorInfoes", "InspectRUserProfile_Id", "dbo.InspectRUserProfiles");
            DropColumn("dbo.InspectorInfoes", "InspectRUserProfile_Id");
            DropTable("dbo.InspectRUserProfiles");
        }
    }
}
