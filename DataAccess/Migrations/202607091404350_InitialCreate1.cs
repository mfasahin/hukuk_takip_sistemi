namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Icras",
                c => new
                    {
                        ICRA_ID = c.Int(nullable: false, identity: true),
                        IHTAR_URUN_ID = c.Int(nullable: false),
                        MAHKEME_ID = c.Int(nullable: false),
                        ICRA_TAKIP_TAR = c.DateTime(nullable: false),
                        ICRA_DOSYA_NO = c.String(nullable: false, maxLength: 15),
                        GRS_TAR_ZMN = c.DateTime(nullable: false),
                        GRS_KULLANICI_ID = c.Int(nullable: false),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Int(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ICRA_ID)
                .ForeignKey("dbo.IhtarUruns", t => t.IHTAR_URUN_ID, cascadeDelete: true)
                .ForeignKey("dbo.Mahkemes", t => t.MAHKEME_ID, cascadeDelete: true)
                .Index(t => t.IHTAR_URUN_ID)
                .Index(t => t.MAHKEME_ID);
            
            CreateTable(
                "dbo.IhtarUruns",
                c => new
                    {
                        IHTAR_URUN_ID = c.Int(nullable: false, identity: true),
                        URUN_ID = c.Int(nullable: false),
                        IHTAR_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IHTAR_URUN_ID)
                .ForeignKey("dbo.Ihtars", t => t.IHTAR_ID, cascadeDelete: true)
                .ForeignKey("dbo.Uruns", t => t.URUN_ID, cascadeDelete: true)
                .Index(t => t.URUN_ID)
                .Index(t => t.IHTAR_ID);
            
            CreateTable(
                "dbo.Ihtars",
                c => new
                    {
                        IHTAR_ID = c.Int(nullable: false, identity: true),
                        MUSTERI_ID = c.Int(nullable: false),
                        AVUKAT_ID = c.Int(nullable: false),
                        SUBE_ID = c.Int(nullable: false),
                        BORC_TUTAR = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IHTAR_TAR_ZMN = c.DateTime(nullable: false),
                        GRS_TAR_ZMN = c.DateTime(nullable: false),
                        GRS_KULLANICI_ID = c.Int(nullable: false),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Int(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Int(),
                    })
                .PrimaryKey(t => t.IHTAR_ID)
                .ForeignKey("dbo.Avukats", t => t.AVUKAT_ID, cascadeDelete: true)
                .ForeignKey("dbo.Musteris", t => t.MUSTERI_ID, cascadeDelete: true)
                .ForeignKey("dbo.Subes", t => t.SUBE_ID, cascadeDelete: true)
                .Index(t => t.MUSTERI_ID)
                .Index(t => t.AVUKAT_ID)
                .Index(t => t.SUBE_ID);
            
            CreateTable(
                "dbo.Uruns",
                c => new
                    {
                        URUN_ID = c.Int(nullable: false, identity: true),
                        URUN_AD = c.String(nullable: false, maxLength: 25),
                        URUN_KOD = c.String(nullable: false, maxLength: 5),
                        SON_GECERLILIK_TAR = c.DateTime(),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Int(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Int(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Int(),
                    })
                .PrimaryKey(t => t.URUN_ID);
            
            CreateTable(
                "dbo.Mahkemes",
                c => new
                    {
                        MAHKEME_ID = c.Int(nullable: false, identity: true),
                        MAHKEME_AD = c.String(nullable: false, maxLength: 100),
                        GRS_TAR_ZMN = c.DateTime(nullable: false),
                        GRS_KULLANICI_ID = c.Int(nullable: false),
                        GUNCELLE_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Int(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Int(),
                    })
                .PrimaryKey(t => t.MAHKEME_ID);
            
            CreateTable(
                "dbo.Kullanıcı",
                c => new
                    {
                        KULLANICI_ID = c.Int(nullable: false, identity: true),
                        KULLANICI_AD = c.String(nullable: false, maxLength: 25),
                        SIFRE = c.String(nullable: false, maxLength: 25),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Int(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Int(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Int(),
                    })
                .PrimaryKey(t => t.KULLANICI_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Icras", "MAHKEME_ID", "dbo.Mahkemes");
            DropForeignKey("dbo.Icras", "IHTAR_URUN_ID", "dbo.IhtarUruns");
            DropForeignKey("dbo.IhtarUruns", "URUN_ID", "dbo.Uruns");
            DropForeignKey("dbo.IhtarUruns", "IHTAR_ID", "dbo.Ihtars");
            DropForeignKey("dbo.Ihtars", "SUBE_ID", "dbo.Subes");
            DropForeignKey("dbo.Ihtars", "MUSTERI_ID", "dbo.Musteris");
            DropForeignKey("dbo.Ihtars", "AVUKAT_ID", "dbo.Avukats");
            DropIndex("dbo.Ihtars", new[] { "SUBE_ID" });
            DropIndex("dbo.Ihtars", new[] { "AVUKAT_ID" });
            DropIndex("dbo.Ihtars", new[] { "MUSTERI_ID" });
            DropIndex("dbo.IhtarUruns", new[] { "IHTAR_ID" });
            DropIndex("dbo.IhtarUruns", new[] { "URUN_ID" });
            DropIndex("dbo.Icras", new[] { "MAHKEME_ID" });
            DropIndex("dbo.Icras", new[] { "IHTAR_URUN_ID" });
            DropTable("dbo.Kullanıcı");
            DropTable("dbo.Mahkemes");
            DropTable("dbo.Uruns");
            DropTable("dbo.Ihtars");
            DropTable("dbo.IhtarUruns");
            DropTable("dbo.Icras");
        }
    }
}
