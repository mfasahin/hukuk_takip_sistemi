namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTableNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Avukats", newName: "AVUKAT");
            RenameTable(name: "dbo.Icras", newName: "ICRA");
            RenameTable(name: "dbo.IhtarUruns", newName: "IHTAR_URUN");
            RenameTable(name: "dbo.Ihtars", newName: "IHTAR");
            RenameTable(name: "dbo.Musteris", newName: "MUSTERI");
            RenameTable(name: "dbo.Subes", newName: "SUBE");
            RenameTable(name: "dbo.Uruns", newName: "URUN");
            RenameTable(name: "dbo.Mahkemes", newName: "ICRA_MAHKEME");
            RenameTable(name: "dbo.Kullanicis", newName: "KULLANICI");
            AddColumn("dbo.MUSTERI", "MUST_VKN_NO", c => c.String(maxLength: 10));
            DropColumn("dbo.MUSTERI", "MUST_VKNO");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MUSTERI", "MUST_VKNO", c => c.String(maxLength: 10));
            DropColumn("dbo.MUSTERI", "MUST_VKN_NO");
            RenameTable(name: "dbo.KULLANICI", newName: "Kullanicis");
            RenameTable(name: "dbo.ICRA_MAHKEME", newName: "Mahkemes");
            RenameTable(name: "dbo.URUN", newName: "Uruns");
            RenameTable(name: "dbo.SUBE", newName: "Subes");
            RenameTable(name: "dbo.MUSTERI", newName: "Musteris");
            RenameTable(name: "dbo.IHTAR", newName: "Ihtars");
            RenameTable(name: "dbo.IHTAR_URUN", newName: "IhtarUruns");
            RenameTable(name: "dbo.ICRA", newName: "Icras");
            RenameTable(name: "dbo.AVUKAT", newName: "Avukats");
        }
    }
}
