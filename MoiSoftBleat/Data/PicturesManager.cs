using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MoiSoftBleat.Auxiliary;
using MoiSoftBleat.DataModel;


namespace MoiSoftBleat.Data
{
    //ПОПРОБОВАТЬ РАЗДЕЛИТЬ НА Accessor и Manager (скорее всего когда буду внедрять Entity)
    public class PicturesManager
    {
        private Dictionary<Guid, Picture> _pictures;

        public Dictionary<Guid, Picture> Pictures => _pictures;

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

                _pictures = GetPicturesNamesAndPath(_folderPath);
            }
            catch (Exception)
            {
                throw;
            }
            //CODE FIRST TESTS!!!
            GenerateDBTest(_pictures);
        }
        #region ТЕСТОВАЯ ХЕРНЯ ДЛЯ ГЕНЕРАЦИИ БД

        public void GenerateDBTest(Dictionary<Guid, Picture> pictures)
        {
            #region Генерация тегов
            List<Tag> tags = new List<Tag>();

            for (int i = 0; i < 10; i++)
            {
                tags.Add(new Tag() { TagUid = Guid.NewGuid(), PicturesNumber = i, TagName = "Tag" + i.ToString() });
            }
            #endregion

            using (PicturesContext picturesContext = new PicturesContext())
            {
                picturesContext.picturesData.AddRange(pictures.Select(x => x.Value.PictureData).ToList());
                picturesContext.SaveChanges();
            }

            using (TagsContext tagsContext = new TagsContext())
            {
                tagsContext.tags.AddRange(tags);
                tagsContext.SaveChanges();
            }


        } 
        #endregion

        /// <summary>
        /// Формирует Dictionary из имени и полного пути
        /// </summary>
        /// <param name="folderPath">место, откуда грузит</param>
        /// <returns></returns>
        public Dictionary<Guid, Picture> GetPicturesNamesAndPath(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentNullException("Путь не указан");

            PictureData pictureData = null;
            FileInfo fileInfo = null;

            Picture picture = null;
            System.Windows.Controls.Image image = null;

            Dictionary<Guid, Picture> pictures = new Dictionary<Guid, Picture>();

            if (Directory.Exists(folderPath))
            {
                foreach (var Path in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".bmp")))
                {

                    #region Заполнение информации о картинке

                    //необходимо для получения разрешения картинки
                    System.Drawing.Image newImage = System.Drawing.Image.FromFile(Path);

                    //вытягивание самой картинки
                    image = new System.Windows.Controls.Image { Source = new BitmapImage(new Uri(Path)) };

                    fileInfo = new FileInfo(Path);
                    pictureData = new PictureData
                    {
                        PictureDataUid = Guid.NewGuid(), //оставить так до разборок
                        ImgName = fileInfo.Name,
                        Path = Path,
                        Size = IntToBytesExtension.ToBytes((int)fileInfo.Length),
                        Resolution = newImage.Width.ToString() + " x " + newImage.Height.ToString(),
                        ImgType = fileInfo.Extension,
                        Tags = new List<Tag>() // пока пустой
                        
                    };

                    #endregion

                    
                    //формирование конечного объекта
                    picture = new Picture(pictureData, new List<string>(), image);

                    //добавление проинициализированного объекта в итоговую коллекцию
                    pictures.Add(picture.PictureData.PictureDataUid, picture);

                }

                return pictures;
            }
            else
                return null;
        } 

        public bool SavePicture()
        {
            return true;
        }
    }
}
