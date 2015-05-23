namespace InspectR.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class bin_title : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectorInfoes", "Title", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.InspectorInfoes", "Title");
        }
    }
}
