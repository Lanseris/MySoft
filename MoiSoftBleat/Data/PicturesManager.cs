using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiSoftBleat.Data
{
    public class PicturesManager
    {
        private Dictionary<Guid, Picture> _pictures;

        public Dictionary<Guid, Picture> Pictures => _pictures;

        private Dictionary<int, string> _tags;

        public Dictionary<int, string> Tags => _tags;

        public PicturesManager()
        {
           LoadPictures();
        }

        public void LoadPictures()
        {
            //генерация объектов каритнок для первичных тестов без загрузки откуда либо
            _pictures = GenerateTestPictures(10);
        }

        public Dictionary<Guid, Picture> GenerateTestPictures(int picturesNumber)
        {
            Picture picture;
            Dictionary<Guid, Picture> pictures = new Dictionary<Guid, Picture>();

            for (int i = 0; i < picturesNumber; i++)
            {
                picture = new Picture("Picture_" + i, new List<string>());
                pictures.Add(picture.pictureUid, picture);
            }

            return pictures;
        }

        public bool SavePicture()
        {
            return true;
        }
    }
}
