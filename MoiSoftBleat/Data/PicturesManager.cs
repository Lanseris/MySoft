using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MoiSoftBleat.Data
{
    //ПОПРОБОВАТЬ РАЗДЕЛИТЬ НА Accessor и Manager (скорее всего когда буду внедрять Entity)
    public class PicturesManager
    {
        private Dictionary<Guid, Picture> _pictures;

        public Dictionary<Guid, Picture> Pictures => _pictures;

        private Dictionary<int, string> _tags;

        public Dictionary<int, string> Tags => _tags;

        private string _folderPath;

        private string _selfLocation;

        public PicturesManager()
        {
            LoadPictures();
        }

        public void SetFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                throw new ArgumentNullException("Путь не указан");

            if (!Directory.Exists(folder))
                throw new ArgumentException("Папки не сущетсвует");

            if (!folder.Equals(_selfLocation))
            {
                using (StreamWriter sw = new StreamWriter(_selfLocation + "\\PrevFolder.txt"))
                {
                    sw.Write(folder);
                } 
            }

            _folderPath = folder;

        }
        public void LoadPictures()
        {
            Dictionary<string,string> picturesNamesAndPaths;

            try
            {
                if (string.IsNullOrEmpty(_folderPath))
                {
                    _selfLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    if (File.Exists(_selfLocation + "\\PrevFolder.txt"))
                    {
                        using (StreamReader sr = new StreamReader(_selfLocation + "\\PrevFolder.txt"))
                        {
                            _folderPath = sr.ReadLine();
                            
                            if (!Directory.Exists(_folderPath))
                                _folderPath = null;
                        }
                    }

                    if (string.IsNullOrEmpty(_folderPath))
                        _folderPath = _selfLocation; 
                }


                picturesNamesAndPaths = GetPicturesNamesAndPath(_folderPath);
            }
            catch (Exception)
            {
                throw;
            }

            GeneratePicturesByNameAndPath(picturesNamesAndPaths);

        }

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
                image = new Image {Source = new BitmapImage( new Uri( item.Value ))};
                picture = new Picture(item.Key, new List<string>(),image);
                pictures.Add(picture.pictureUid, picture);
            }
            return pictures;
        }

        public Dictionary<string, string> GetPicturesNamesAndPath(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentNullException("Путь не указан");

            Dictionary<string, string> nameVsPath = new Dictionary<string, string>();
            if (Directory.Exists(folderPath))
            {
                foreach (var item in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpg")|| s.EndsWith(".png")))
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
