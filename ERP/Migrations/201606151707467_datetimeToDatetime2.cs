namespace ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datetimeToDatetime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "CreatedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Orders", "CompletedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Orders", "ShippedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Orders", "DeliveredAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Orders", "CanceledAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "CanceledAt", c => c.DateTime());
            AlterColumn("dbo.Orders", "DeliveredAt", c => c.DateTime());
            AlterColumn("dbo.Orders", "ShippedAt", c => c.DateTime());
            AlterColumn("dbo.Orders", "CompletedAt", c => c.DateTime());
            AlterColumn("dbo.Orders", "CreatedAt", c => c.DateTime(nullable: false));
        }
    }
}
