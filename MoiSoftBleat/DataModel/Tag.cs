using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiSoftBleat.DataModel
{
    public class Tag
    {
        public Guid TagUid { get; set; }
        public string TagName { get; set; }
        public int PicturesNumber { get; set; }
        public List<PictureData> PicturesData { get; set; }
    }
}
