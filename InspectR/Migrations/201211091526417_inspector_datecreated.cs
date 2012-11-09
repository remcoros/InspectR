namespace InspectR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inspector_datecreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectorInfoes", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectorInfoes", "DateCreated");
        }
    }
}
