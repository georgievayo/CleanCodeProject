namespace FindAndBook.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixmanagerIdtobeGuid : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Restaurants", new[] { "Manager_Id" });
            DropColumn("dbo.Restaurants", "ManagerId");
            RenameColumn(table: "dbo.Restaurants", name: "Manager_Id", newName: "ManagerId");
            AlterColumn("dbo.Restaurants", "ManagerId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Restaurants", "ManagerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Restaurants", new[] { "ManagerId" });
            AlterColumn("dbo.Restaurants", "ManagerId", c => c.String());
            RenameColumn(table: "dbo.Restaurants", name: "ManagerId", newName: "Manager_Id");
            AddColumn("dbo.Restaurants", "ManagerId", c => c.String());
            CreateIndex("dbo.Restaurants", "Manager_Id");
        }
    }
}
