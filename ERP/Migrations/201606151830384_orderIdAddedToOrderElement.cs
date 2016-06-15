namespace ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderIdAddedToOrderElement : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderElements", "Order_Id", "dbo.Orders");
            DropIndex("dbo.OrderElements", new[] { "Order_Id" });
            RenameColumn(table: "dbo.OrderElements", name: "Order_Id", newName: "OrderId");
            AlterColumn("dbo.OrderElements", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderElements", "OrderId");
            AddForeignKey("dbo.OrderElements", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderElements", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderElements", new[] { "OrderId" });
            AlterColumn("dbo.OrderElements", "OrderId", c => c.Int());
            RenameColumn(table: "dbo.OrderElements", name: "OrderId", newName: "Order_Id");
            CreateIndex("dbo.OrderElements", "Order_Id");
            AddForeignKey("dbo.OrderElements", "Order_Id", "dbo.Orders", "Id");
        }
    }
}
