namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AVUKAT",
                c => new
                    {
                        AVUKAT_ID = c.Guid(nullable: false),
                        AVKT_AD = c.String(maxLength: 25),
                        AVKT_SOYAD = c.String(maxLength: 25),
                        TBB_SICIL_NO = c.String(maxLength: 10),
                        AVKT_EPOSTA = c.String(maxLength: 25),
                        AVKT_TEL_NO = c.String(maxLength: 15),
                        HKK_BURO_AD = c.String(maxLength: 50),
                        HKK_BURO_ADRES = c.String(maxLength: 70),
                        OFIS_TEL_NO = c.String(maxLength: 15),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Guid(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Guid(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.AVUKAT_ID);
            
            CreateTable(
                "dbo.IHTAR",
                c => new
                    {
                        IHTAR_ID = c.Guid(nullable: false),
                        MUSTERI_ID = c.Guid(nullable: false),
                        AVUKAT_ID = c.Guid(nullable: false),
                        SUBE_ID = c.Guid(nullable: false),
                        BORC_TUTAR = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IHTAR_TAR_ZMN = c.DateTime(nullable: false),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Guid(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Guid(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.IHTAR_ID)
                .ForeignKey("dbo.AVUKAT", t => t.AVUKAT_ID, cascadeDelete: true)
                .ForeignKey("dbo.MUSTERI", t => t.MUSTERI_ID, cascadeDelete: true)
                .ForeignKey("dbo.SUBE", t => t.SUBE_ID, cascadeDelete: true)
                .Index(t => t.MUSTERI_ID)
                .Index(t => t.AVUKAT_ID)
                .Index(t => t.SUBE_ID);
            
            CreateTable(
                "dbo.IHTAR_URUN",
                c => new
                    {
                        IHTAR_URUN_ID = c.Guid(nullable: false),
                        URUN_ID = c.Guid(nullable: false),
                        IHTAR_ID = c.Guid(nullable: false),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Guid(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Guid(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.IHTAR_URUN_ID)
                .ForeignKey("dbo.IHTAR", t => t.IHTAR_ID, cascadeDelete: true)
                .ForeignKey("dbo.URUN", t => t.URUN_ID, cascadeDelete: true)
                .Index(t => t.URUN_ID)
                .Index(t => t.IHTAR_ID);
            
            CreateTable(
                "dbo.URUN",
                c => new
                    {
                        URUN_ID = c.Guid(nullable: false),
                        URUN_AD = c.String(maxLength: 25),
                        URUN_KOD = c.String(maxLength: 5),
                        SON_GECERLILIK_TAR = c.DateTime(),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Guid(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Guid(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.URUN_ID);
            
            CreateTable(
                "dbo.MUSTERI",
                c => new
                    {
                        MUSTERI_ID = c.Guid(nullable: false),
                        MUST_NO = c.String(maxLength: 8),
                        MUST_AD = c.String(maxLength: 25),
                        MUST_SOYAD = c.String(maxLength: 25),
                        MUST_KIMLIK_NO = c.String(maxLength: 11),
                        MUST_VKN_NO = c.String(maxLength: 10),
                        MUST_EPOSTA = c.String(maxLength: 50),
                        MUST_TEL_NO = c.String(maxLength: 15),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Guid(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Guid(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.MUSTERI_ID);
            
            CreateTable(
                "dbo.SUBE",
                c => new
                    {
                        SUBE_ID = c.Guid(nullable: false),
                        SUBE_KODU = c.String(maxLength: 5),
                        SUBE_ADI = c.String(maxLength: 25),
                        SUBE_BOLGE = c.String(maxLength: 25),
                        SUBE_ADRES = c.String(maxLength: 75),
                        SUBE_TEL_NO = c.String(maxLength: 15),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Guid(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Guid(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.SUBE_ID);
            
            CreateTable(
                "dbo.ICRA",
                c => new
                    {
                        ICRA_ID = c.Guid(nullable: false),
                        IHTAR_URUN_ID = c.Guid(nullable: false),
                        MAHKEME_ID = c.Guid(nullable: false),
                        ICRA_TAKIP_TAR = c.DateTime(nullable: false),
                        ICRA_DOSYA_NO = c.String(maxLength: 15),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Guid(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Guid(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ICRA_ID)
                .ForeignKey("dbo.IHTAR_URUN", t => t.IHTAR_URUN_ID, cascadeDelete: true)
                .ForeignKey("dbo.ICRA_MAHKEME", t => t.MAHKEME_ID, cascadeDelete: true)
                .Index(t => t.IHTAR_URUN_ID)
                .Index(t => t.MAHKEME_ID);
            
            CreateTable(
                "dbo.ICRA_MAHKEME",
                c => new
                    {
                        MAHKEME_ID = c.Guid(nullable: false),
                        MAHKEME_AD = c.String(maxLength: 100),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Guid(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Guid(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.MAHKEME_ID);
            
            CreateTable(
                "dbo.KULLANICI",
                c => new
                    {
                        KULLANICI_ID = c.Guid(nullable: false),
                        KULLANICI_AD = c.String(maxLength: 25),
                        SIFRE = c.String(maxLength: 25),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Guid(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Guid(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.KULLANICI_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ICRA", "MAHKEME_ID", "dbo.ICRA_MAHKEME");
            DropForeignKey("dbo.ICRA", "IHTAR_URUN_ID", "dbo.IHTAR_URUN");
            DropForeignKey("dbo.IHTAR", "SUBE_ID", "dbo.SUBE");
            DropForeignKey("dbo.IHTAR", "MUSTERI_ID", "dbo.MUSTERI");
            DropForeignKey("dbo.IHTAR_URUN", "URUN_ID", "dbo.URUN");
            DropForeignKey("dbo.IHTAR_URUN", "IHTAR_ID", "dbo.IHTAR");
            DropForeignKey("dbo.IHTAR", "AVUKAT_ID", "dbo.AVUKAT");
            DropIndex("dbo.ICRA", new[] { "MAHKEME_ID" });
            DropIndex("dbo.ICRA", new[] { "IHTAR_URUN_ID" });
            DropIndex("dbo.IHTAR_URUN", new[] { "IHTAR_ID" });
            DropIndex("dbo.IHTAR_URUN", new[] { "URUN_ID" });
            DropIndex("dbo.IHTAR", new[] { "SUBE_ID" });
            DropIndex("dbo.IHTAR", new[] { "AVUKAT_ID" });
            DropIndex("dbo.IHTAR", new[] { "MUSTERI_ID" });
            DropTable("dbo.KULLANICI");
            DropTable("dbo.ICRA_MAHKEME");
            DropTable("dbo.ICRA");
            DropTable("dbo.SUBE");
            DropTable("dbo.MUSTERI");
            DropTable("dbo.URUN");
            DropTable("dbo.IHTAR_URUN");
            DropTable("dbo.IHTAR");
            DropTable("dbo.AVUKAT");
        }
    }
}
