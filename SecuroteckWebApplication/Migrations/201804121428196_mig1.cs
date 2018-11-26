namespace SecuroteckWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            //Sql("ALTER TABLE dbo.Logs DROP CONSTRAINT [FK_dbo.Logs_dbo.Users_User_APIKey] FOREIGN KEY ([User_APIKey]) REFERENCES [dbo].[Users] ([APIKey])");
            //Sql("ALTER TABLE dbo.Logs ADD CONSTRAINT [FK_dbo.Logs_dbo.Users_User_APIKey] FOREIGN KEY ([User_APIKey]) REFERENCES [dbo].[Users] ([APIKey]) ON DELETE SET NULL");
        }

        public override void Down()
        {
            //Sql("ALTER TABLE dbo.Logs DROP CONSTRAINT [FK_dbo.Logs_dbo.Users_User_APIKey] FOREIGN KEY ([User_APIKey]) REFERENCES [dbo].[Users] ([APIKey]) ON DELETE SET NULL");
            //Sql("ALTER TABLE dbo.Logs ADD CONSTRAINT [FK_dbo.Logs_dbo.Users_User_APIKey] FOREIGN KEY ([User_APIKey]) REFERENCES [dbo].[Users] ([APIKey])");            
        }
    }
}
