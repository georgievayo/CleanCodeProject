namespace FindAndBook.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addrolecolumntouserstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Role");
        }
    }
}
