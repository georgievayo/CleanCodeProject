namespace FindAndBook.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialsetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookedTables",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TableId = c.Guid(),
                        BookingId = c.Guid(nullable: false),
                        TablesCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tables", t => t.TableId)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.TableId)
                .Index(t => t.BookingId);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RestaurantId = c.Guid(nullable: false),
                        UserId = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        NumberOfPeople = c.Int(nullable: false),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.RestaurantId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Address = c.String(),
                        ManagerId = c.String(),
                        Name = c.String(),
                        PhotoUrl = c.String(),
                        Contact = c.String(),
                        WeekendHours = c.String(),
                        WeekdayHours = c.String(),
                        Details = c.String(),
                        AverageBill = c.Int(),
                        Manager_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Manager_Id, cascadeDelete: true)
                .Index(t => t.Manager_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RestaurantId = c.Guid(nullable: false),
                        NumberOfPeople = c.Int(nullable: false),
                        NumberOfTables = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.RestaurantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookedTables", "BookingId", "dbo.Bookings");
            DropForeignKey("dbo.Tables", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.BookedTables", "TableId", "dbo.Tables");
            DropForeignKey("dbo.Restaurants", "Manager_Id", "dbo.Users");
            DropForeignKey("dbo.Bookings", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Bookings", "RestaurantId", "dbo.Restaurants");
            DropIndex("dbo.Tables", new[] { "RestaurantId" });
            DropIndex("dbo.Restaurants", new[] { "Manager_Id" });
            DropIndex("dbo.Bookings", new[] { "User_Id" });
            DropIndex("dbo.Bookings", new[] { "RestaurantId" });
            DropIndex("dbo.BookedTables", new[] { "BookingId" });
            DropIndex("dbo.BookedTables", new[] { "TableId" });
            DropTable("dbo.Tables");
            DropTable("dbo.Users");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Bookings");
            DropTable("dbo.BookedTables");
        }
    }
}
