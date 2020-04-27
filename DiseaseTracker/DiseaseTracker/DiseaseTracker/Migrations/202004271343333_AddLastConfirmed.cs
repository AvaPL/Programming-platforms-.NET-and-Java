namespace DiseaseTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastConfirmed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visitors", "LastConfirmed", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Visitors", "LastConfirmed");
        }
    }
}
