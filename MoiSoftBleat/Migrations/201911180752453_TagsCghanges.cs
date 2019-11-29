namespace MoiSoftBleat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagsCghanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PictureDataTags", "PictureData_PictureDataUid", "dbo.PictureDatas");
            DropForeignKey("dbo.PictureDataTags", "Tag_TagUid", "dbo.Tags");
            DropIndex("dbo.PictureDataTags", new[] { "PictureData_PictureDataUid" });
            DropIndex("dbo.PictureDataTags", new[] { "Tag_TagUid" });
            AddColumn("dbo.Tags", "Tag_TagUid", c => c.Guid());
            CreateIndex("dbo.Tags", "Tag_TagUid");
            AddForeignKey("dbo.Tags", "Tag_TagUid", "dbo.Tags", "TagUid");
            DropTable("dbo.PictureDatas");
            DropTable("dbo.PictureDataTags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PictureDataTags",
                c => new
                    {
                        PictureData_PictureDataUid = c.Guid(nullable: false),
                        Tag_TagUid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PictureData_PictureDataUid, t.Tag_TagUid });
            
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
            
            DropForeignKey("dbo.Tags", "Tag_TagUid", "dbo.Tags");
            DropIndex("dbo.Tags", new[] { "Tag_TagUid" });
            DropColumn("dbo.Tags", "Tag_TagUid");
            CreateIndex("dbo.PictureDataTags", "Tag_TagUid");
            CreateIndex("dbo.PictureDataTags", "PictureData_PictureDataUid");
            AddForeignKey("dbo.PictureDataTags", "Tag_TagUid", "dbo.Tags", "TagUid", cascadeDelete: true);
            AddForeignKey("dbo.PictureDataTags", "PictureData_PictureDataUid", "dbo.PictureDatas", "PictureDataUid", cascadeDelete: true);
        }
    }
}
