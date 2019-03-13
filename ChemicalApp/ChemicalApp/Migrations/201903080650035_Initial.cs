namespace ChemicalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Debets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Sum = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Kredits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Sum = c.Int(nullable: false),
                        Comment = c.String(),
                        DepartmentId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.DepartmentId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Kredits", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Kredits", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Debets", "ProductId", "dbo.Products");
            DropIndex("dbo.Kredits", new[] { "ProductId" });
            DropIndex("dbo.Kredits", new[] { "DepartmentId" });
            DropIndex("dbo.Debets", new[] { "ProductId" });
            DropTable("dbo.Departments");
            DropTable("dbo.Kredits");
            DropTable("dbo.Products");
            DropTable("dbo.Debets");
        }
    }
}
