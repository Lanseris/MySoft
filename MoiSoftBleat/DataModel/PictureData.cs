﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiSoftBleat.DataModel
{
    public class PictureData
    {
        public Guid guid { get; set; }
        public string Path { get; set; }
        public string ImgName { get; set; }
        public string Size { get; set; }
        public string ImgType { get; set; }
        public string Resolution { get; set; }
        public List<Tag> Tags { get; set; }
    }
}