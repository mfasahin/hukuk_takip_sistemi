namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSubeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subes",
                c => new
                    {
                        SUBE_ID = c.Int(nullable: false, identity: true),
                        SUBE_KODU = c.String(nullable: false, maxLength: 5),
                        SUBE_ADI = c.String(nullable: false, maxLength: 25),
                        SUBE_BOLGE = c.String(maxLength: 25),
                        SUBE_ADRES = c.String(maxLength: 75),
                        SUBE_TEL_NO = c.String(maxLength: 15),
                        GRS_TAR_ZMN = c.DateTime(),
                        GRS_KULLANICI_ID = c.Int(),
                        GNC_TAR_ZMN = c.DateTime(),
                        GNC_KULLANICI_ID = c.Int(),
                        SIL_TAR_ZMN = c.DateTime(),
                        SIL_KULLANICI_ID = c.Int(),
                    })
                .PrimaryKey(t => t.SUBE_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Subes");
        }
    }
}
