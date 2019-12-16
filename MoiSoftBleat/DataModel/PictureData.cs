using MoiSoftBleat.Auxiliary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MoiSoftBleat.DataModel
{
    public class PictureData
    {
        [Key]
        public Guid PictureDataUid { get; set; }
        public string Path { get; set; }
        public string ImgName { get; set; }
        [NotMapped]
        public string ImgVisibleName { get; set; }
        public string Size { get; set; }
        public string ImgType { get; set; }
        public string Resolution { get; set; }
        public List<Tag> Tags { get; set; }
        [NotMapped]
        public bool Stored { get; set; }

        public PictureData()
        {}

        public PictureData(FileInfo fileInfo,Image image )
        {
            PictureDataUid = Guid.NewGuid(); //оставить так до разборок
            ImgName = fileInfo.Name;
            if (fileInfo.Name.Length > 12)
            {
                ImgVisibleName = fileInfo.Name.Substring(12, fileInfo.Name.Length - 12);
                Stored = fileInfo.Name.Substring(0, 12) == "DragonsCave_";
            }
            else
            {
                ImgVisibleName = fileInfo.Name;
                Stored = false;
            }
            Path = fileInfo.FullName;
            Size = IntToBytesExtension.ToBytes((int)fileInfo.Length);
            Resolution = image.Width.ToString() + " x " + image.Height.ToString();
            ImgType = fileInfo.Extension;

            Tags = new List<Tag>(); // пока пустой
        }
    }
}
