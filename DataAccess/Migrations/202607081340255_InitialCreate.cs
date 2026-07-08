namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Musteris",
                c => new
                    {
                        MUSTERI_ID = c.Int(nullable: false, identity: true),
                        MUST_NO = c.String(nullable: false, maxLength: 8),
                        MUST_AD = c.String(nullable: false, maxLength: 25),
                        MUST_SOYAD = c.String(maxLength: 25),
                        MUST_KIMLIK_NO = c.String(maxLength: 11),
                        MUST_VKNO = c.String(maxLength: 10),
                        MUST_EPOSTA = c.String(maxLength: 50),
                        MUST_TEL_NO = c.String(maxLength: 15),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Int(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Int(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Int(),
                    })
                .PrimaryKey(t => t.MUSTERI_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Musteris");
        }
    }
}
