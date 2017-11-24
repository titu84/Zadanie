namespace TwoWaySynonyms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Synonyms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Term = c.String(nullable: false, maxLength: 50),
                        Synonyms = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Synonyms");
        }
    }
}
