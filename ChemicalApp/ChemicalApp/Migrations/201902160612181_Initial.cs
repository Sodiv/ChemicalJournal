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
                        Sum = c.Single(nullable: false),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
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
                        Sum = c.Single(nullable: false),
                        Department_Id = c.Int(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Department_Id)
                .Index(t => t.Product_Id);
            
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
            DropForeignKey("dbo.Kredits", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Kredits", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Debets", "Product_Id", "dbo.Products");
            DropIndex("dbo.Kredits", new[] { "Product_Id" });
            DropIndex("dbo.Kredits", new[] { "Department_Id" });
            DropIndex("dbo.Debets", new[] { "Product_Id" });
            DropTable("dbo.Departments");
            DropTable("dbo.Kredits");
            DropTable("dbo.Products");
            DropTable("dbo.Debets");
        }
    }
}
