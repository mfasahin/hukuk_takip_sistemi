namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIhtarUrun : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IHTAR_URUN", "GRS_TAR_ZMN", c => c.DateTime());
            AddColumn("dbo.IHTAR_URUN", "GRS_KULLANICI_ID", c => c.Int());
            AddColumn("dbo.IHTAR_URUN", "GNC_TAR_ZMN", c => c.DateTime());
            AddColumn("dbo.IHTAR_URUN", "GNC_KULLANICI_ID", c => c.Int());
            AddColumn("dbo.IHTAR_URUN", "SIL_TAR_ZMN", c => c.DateTime());
            AddColumn("dbo.IHTAR_URUN", "SIL_KULLANICI_ID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IHTAR_URUN", "SIL_KULLANICI_ID");
            DropColumn("dbo.IHTAR_URUN", "SIL_TAR_ZMN");
            DropColumn("dbo.IHTAR_URUN", "GNC_KULLANICI_ID");
            DropColumn("dbo.IHTAR_URUN", "GNC_TAR_ZMN");
            DropColumn("dbo.IHTAR_URUN", "GRS_KULLANICI_ID");
            DropColumn("dbo.IHTAR_URUN", "GRS_TAR_ZMN");
        }
    }
}
