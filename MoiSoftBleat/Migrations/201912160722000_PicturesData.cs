namespace MoiSoftBleat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PicturesData : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Tags",
                c => new
                    {
                        TagUid = c.Guid(nullable: false),
                        TagName = c.String(),
                        PicturesNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TagUid);
            
            CreateTable(
                "dbo.TagPictureDatas",
                c => new
                    {
                        Tag_TagUid = c.Guid(nullable: false),
                        PictureData_PictureDataUid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagUid, t.PictureData_PictureDataUid })
                .ForeignKey("dbo.Tags", t => t.Tag_TagUid, cascadeDelete: true)
                .ForeignKey("dbo.PictureDatas", t => t.PictureData_PictureDataUid, cascadeDelete: true)
                .Index(t => t.Tag_TagUid)
                .Index(t => t.PictureData_PictureDataUid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagPictureDatas", "PictureData_PictureDataUid", "dbo.PictureDatas");
            DropForeignKey("dbo.TagPictureDatas", "Tag_TagUid", "dbo.Tags");
            DropIndex("dbo.TagPictureDatas", new[] { "PictureData_PictureDataUid" });
            DropIndex("dbo.TagPictureDatas", new[] { "Tag_TagUid" });
            DropTable("dbo.TagPictureDatas");
            DropTable("dbo.Tags");
            DropTable("dbo.PictureDatas");
        }
    }
}
