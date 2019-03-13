namespace ChemicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Balance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Balances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sum = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Balances", "ProductId", "dbo.Products");
            DropIndex("dbo.Balances", new[] { "ProductId" });
            DropTable("dbo.Balances");
        }
    }
}
