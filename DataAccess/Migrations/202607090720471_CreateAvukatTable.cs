namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAvukatTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Avukats",
                c => new
                    {
                        AVUKAT_ID = c.Int(nullable: false, identity: true),
                        AVKT_AD = c.String(nullable: false, maxLength: 25),
                        AVKT_SOYAD = c.String(nullable: false, maxLength: 25),
                        TBB_SICIL_NO = c.String(nullable: false, maxLength: 10),
                        AVKT_EPOSTA = c.String(nullable: false, maxLength: 25),
                        AVKT_TEL_NO = c.String(nullable: false, maxLength: 15),
                        HKK_BURO_AD = c.String(maxLength: 50),
                        HKK_BURO_ADRES = c.String(maxLength: 70),
                        OFIS_TEL_NO = c.String(maxLength: 15),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Int(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Int(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Int(),
                    })
                .PrimaryKey(t => t.AVUKAT_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Avukats");
        }
    }
}
