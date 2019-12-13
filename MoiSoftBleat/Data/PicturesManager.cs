using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MoiSoftBleat.Auxiliary;
using MoiSoftBleat.DataModel;
using System.Threading;


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
            #region CODE FIRST TESTS!!!

            //очистка теперь только тегов, т к картинки загружаются только те, которых ещё в базе нет
            ClearTables();
            //Создание бд, если её нет, базовое заполнение таблиц
            //Картинками из выбранной папки и простыми сгенерированными тегами
            //передаются отлько те картинки , которые ещё не сохранены в базе
            GenerateDBTest(_pictures.Where(x=>x.Value.PictureData.Stored == false).ToDictionary(y=>y.Key,y=>y.Value)); 
            #endregion
        }
        #region ТЕСТОВАЯ ХЕРНЯ ДЛЯ CODE FIRST

        public void ClearTables()
        {
            //ОЧЕНЬ ХУЁВОЕ РЕШЕНИЕ, но альтернатив пока не найдено
            //удаляет построчно
            //truncate ругается на FK, а отметять его проверку не хочу

            using (PicturesContext picturesContext = new PicturesContext())
            {
               // picturesContext.picturesData.RemoveRange(picturesContext.picturesData);
                picturesContext.tags.RemoveRange(picturesContext.tags);

                picturesContext.SaveChanges();
            }
        }

        /// <summary>
        /// //Создание бд, если её нет, заполнение таблиц
        /// </summary>
        /// <param name="pictures"></param>
        ///TODO 
        ///1 - в дальнейшем будет неободимо ввести границы размеров разовой операции
        public void GenerateDBTest(Dictionary<Guid, Picture> pictures) 
        {
            #region Генерация тегов
            List<Tag> genaratedTags = new List<Tag>();

            genaratedTags = GenerateTags(10);
            #endregion


            //переименовываем файл для отслеживания его хранения в бд 
            foreach (var picture in pictures.Values)
            {
                if (File.Exists(picture.PictureData.Path))
                {

                    FileInfo fileInfo = new FileInfo(picture.PictureData.Path);

                    picture.PictureData.ImgName = "DragonsCave_" + picture.PictureData.ImgName;
                    string newPath = Path.Combine(fileInfo.DirectoryName + "\\", picture.PictureData.ImgName);
                    picture.PictureData.Path = newPath;
                    fileInfo.MoveTo(newPath);
                }
            }

            //Сохранение в базу
            using (PicturesContext picturesContext = new PicturesContext())
            {
                picturesContext.picturesData.AddRange(pictures.Select(x => x.Value.PictureData).ToList());
                picturesContext.SaveChanges();

                picturesContext.tags.AddRange(genaratedTags);
                int i = picturesContext.SaveChanges();
            }

        } 

        /// <summary>
        /// Генерация простых тегов для тестов
        /// </summary>
        /// <param name="count">Необходимое количество</param>
        /// <returns></returns>
        public List<Tag> GenerateTags(int count)
        {
            List<Tag> tags = new List<Tag>();

            for (int i = 0; i < count; i++)
            {
                tags.Add(new Tag() { TagUid = Guid.NewGuid(), PicturesNumber = i, TagName = "Tag" + i.ToString(), PicturesData = new List<PictureData>()  });
            }

            return tags;
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

                    //вытягивание самой картинки из папки
                    //ПРОВЕРИТЬ КЕШИРОВАНИЕ НА БОЛЬШОМ КОЛИЧЕСТВЕ КАРТИНОК!!!
                    //скорее всего всё пойдёт по пизде и надо делать динамическую выгрузку кеша при загрузке картинок
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Path);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    image = new Image { Source = bitmap };



                    fileInfo = new FileInfo(Path);
                    pictureData = new PictureData
                    {
                        PictureDataUid = Guid.NewGuid(), //оставить так до разборок
                        ImgName = fileInfo.Name,
                        Stored = fileInfo.Name.Length>12 && fileInfo.Name.Substring(0,12)=="DragonsCave_", 
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

                    newImage.Dispose();

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
