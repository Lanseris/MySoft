namespace MoiSoftBleat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagUid = c.Guid(nullable: false),
                        TagName = c.String(),
                        PicturesNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TagUid);
            
            CreateTable(
                "dbo.PictureDatas",
                c => new
                    {
                        PictureDataUid = c.Guid(nullable: false),
                        Path = c.String(),
                        ImgName = c.String(),
                        Size = c.String(),
                        ImgType = c.String(),
                        Resolution = c.String(),
                    })
                .PrimaryKey(t => t.PictureDataUid);
            
            CreateTable(
                "dbo.PictureDataTags",
                c => new
                    {
                        PictureData_PictureDataUid = c.Guid(nullable: false),
                        Tag_TagUid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PictureData_PictureDataUid, t.Tag_TagUid })
                .ForeignKey("dbo.PictureDatas", t => t.PictureData_PictureDataUid, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.Tag_TagUid, cascadeDelete: true)
                .Index(t => t.PictureData_PictureDataUid)
                .Index(t => t.Tag_TagUid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PictureDataTags", "Tag_TagUid", "dbo.Tags");
            DropForeignKey("dbo.PictureDataTags", "PictureData_PictureDataUid", "dbo.PictureDatas");
            DropIndex("dbo.PictureDataTags", new[] { "Tag_TagUid" });
            DropIndex("dbo.PictureDataTags", new[] { "PictureData_PictureDataUid" });
            DropTable("dbo.PictureDataTags");
            DropTable("dbo.PictureDatas");
            DropTable("dbo.Tags");
        }
    }
}
