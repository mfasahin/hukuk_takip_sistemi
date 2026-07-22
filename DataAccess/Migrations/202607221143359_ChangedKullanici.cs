namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedKullanici : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KULLANICI", "SIFRE", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KULLANICI", "SIFRE", c => c.String(maxLength: 25));
        }
    }
}
