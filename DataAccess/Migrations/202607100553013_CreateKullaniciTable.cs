namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateKullaniciTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Kullanıcı", newName: "Kullanicis");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Kullanicis", newName: "Kullanıcı");
        }
    }
}
