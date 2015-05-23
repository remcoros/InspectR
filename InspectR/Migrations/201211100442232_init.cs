namespace InspectR.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init : DbMigration
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
                             DateCreated = c.DateTime(nullable: false)
                         })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.InspectorInfoes");
        }
    }
}
