using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
