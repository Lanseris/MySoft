using System;
using System.Collections.Generic;
using System.IO;
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

        private string _folderPath;

        public PicturesManager()
        {
            LoadPictures();
        }

        public void SetFolder(string folder)
        {
            _folderPath = folder;
        }
        public void LoadPictures()
        {
            List<string> picturesNames;

            try
            {

                if (_folderPath != null)
                    picturesNames = GetPicturesNames(_folderPath);
                else
                    picturesNames = GetPicturesNames(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
            catch (Exception)
            {
                throw;
            }

            GeneratePicturesByName(picturesNames);

        }

        /// <summary>
        /// генерация картинок с простыми именами
        /// </summary>
        /// <param name="picturesNumber"></param>
        /// <returns></returns>
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

        /// <summary>
        /// генерация объектов картинок с переданными именами
        /// </summary>
        /// <param name="names">список имён</param>
        /// <returns></returns>
        public Dictionary<Guid, Picture> GeneratePictures(List<string> names)
        {
            Picture picture;
            Dictionary<Guid, Picture> pictures = new Dictionary<Guid, Picture>();

            foreach (var name in names)
            {
                picture = new Picture(name, new List<string>());
                pictures.Add(picture.pictureUid, picture);
            }
            return pictures;
        }

        public List<string> GetPicturesNames(string folderPath)
        {
            if (Directory.Exists(folderPath))
                return Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpg")).Select(x=>x.Split('\\').Last()).ToList();
            else
                return null;
        }

        public void GeneratePicturesByName(List<string> names)
        {
            _pictures = GeneratePictures(names);
        }

        public bool SavePicture()
        {
            return true;
        }
    }
}
