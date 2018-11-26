namespace SecuroteckWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeLogArch : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ArchivedLogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ArchivedLogs",
                c => new
                    {
                        ArchivedLogId = c.Int(nullable: false, identity: true),
                        LogMessage = c.String(),
                        ActionDateTime = c.DateTime(nullable: false),
                        ArchivedDateTime = c.DateTime(nullable: false),
                        ArchivedNote = c.String(),
                        UserApiKey = c.String(),
                    })
                .PrimaryKey(t => t.ArchivedLogId);
            
        }
    }
}
