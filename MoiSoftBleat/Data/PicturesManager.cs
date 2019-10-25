using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
            Dictionary<string,string> picturesNamesAndPaths;

            try
            {

                if (_folderPath != null)
                    picturesNamesAndPaths = GetPicturesNamesAndPath(_folderPath);
                else
                    picturesNamesAndPaths = GetPicturesNamesAndPath(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
            catch (Exception)
            {
                throw;
            }

            GeneratePicturesByNameAndPath(picturesNamesAndPaths);

        }
        #region УСТАРЕЛО
        /// <summary>
        /// генерация объектов картинок с простыми именами
        /// </summary>
        /// <param name="picturesNumber"></param>
        /// <returns></returns>
        //public Dictionary<Guid, Picture> GenerateTestPictures(int picturesNumber)
        //{
        //    Picture picture;
        //    Dictionary<Guid, Picture> pictures = new Dictionary<Guid, Picture>();

        //    for (int i = 0; i < picturesNumber; i++)
        //    {
        //        picture = new Picture("Picture_" + i, new List<string>());
        //        pictures.Add(picture.pictureUid, picture);
        //    }

        //    return pictures;
        //} 
        #endregion

        /// <summary>
        /// генерация объектов картинок с переданными именами
        /// </summary>
        /// <param name="names">список имён</param>
        /// <returns></returns>
        public Dictionary<Guid, Picture> GeneratePictures(Dictionary<string,string> picturesNamesAndPath)
        {
            Picture picture;
            Image image;
            Dictionary<Guid, Picture> pictures = new Dictionary<Guid, Picture>();

            foreach (var item in picturesNamesAndPath)
            {
                image = new Image {Source = item.Value };
                picture = new Picture(item.Key, new List<string>(),image);
                pictures.Add(picture.pictureUid, picture);
            }
            return pictures;
        }

        public Dictionary<string, string> GetPicturesNamesAndPath(string folderPath)
        {
            Dictionary<string, string> nameVsPath = new Dictionary<string, string>();
            if (Directory.Exists(folderPath))
            {
                foreach (var item in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpg")))
                {
                    nameVsPath.Add(item.Split('\\').Last(),item);
                }

                return nameVsPath;
            }
            else
                return null;
        }

        public void GeneratePicturesByNameAndPath(Dictionary<string,string> picturesNamesAndPath)
        {
            _pictures = GeneratePictures(picturesNamesAndPath);
        }

        public bool SavePicture()
        {
            return true;
        }
    }
}
