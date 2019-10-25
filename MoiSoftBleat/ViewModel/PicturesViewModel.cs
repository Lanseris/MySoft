using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using MoiSoftBleat.Data;
using Xamarin.Forms;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MoiSoftBleat.ViewModel
{
    class PicturesViewModel : DependencyObject, INotifyPropertyChanged  //потом разобраться в этом , 
    {
        private static PicturesManager _picturesManager;

        public event PropertyChangedEventHandler PropertyChanged;

        //ПОДУМАТЬ КАК ВЫЗВАТЬ СОБЫТИЕ ПРИ СМЕНЕ ФОКУСА ЭЛЕМЕНТА

        public Dictionary<Guid, string> Pictures { get; set; }

        public string FolderPath { get; set; }

        private static Picture _selectedPicture;


        #region DependencyProperty

        public static event SelectedGridItemChangedEventHandler SelectedItemChanged;

        public KeyValuePair<Guid, string> SelectedItem
        {
            get { return (KeyValuePair<Guid, string>)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(KeyValuePair<Guid, string>), typeof(PicturesViewModel), new PropertyMetadata(new KeyValuePair<Guid, string>(), SelectedItem_Changed));

        private static void SelectedItem_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _selectedPicture = _picturesManager.Pictures.FirstOrDefault(x => x.Key == ((KeyValuePair<Guid, string>)e.NewValue).Key).Value;
            SelectedPictureImage = _selectedPicture.Image;

            //SelectedItemChanged?.Invoke(d,new SelectedGridItemChangedEventArgs((GridItem)e.OldValue, (GridItem)e.NewValue));
        }

        #endregion
        public static System.Windows.Controls.Image SelectedPictureImage
        {
            get;
            set;
        }

        public System.Windows.Controls.Image SelectedPictureImage2 { get; set; }
        


        public PicturesViewModel()
        {
            _picturesManager = new PicturesManager();
            Pictures = _picturesManager.Pictures.ToDictionary(t => t.Key, t => t.Value.Name);

            OnPropertyChanged("Pictures");

            #region ICommand init

            selectLoadFolder = new Command(SelectLoadFolder);

            #endregion

            #region Events

            SelectedItemChanged += OnGridSelectedItemChanged;

            #endregion

            #region ТЕСТЫ!!!!!!

          
            var img = new System.Windows.Controls.Image { Source = new BitmapImage(new Uri("C:/Users/vasiliy.kononov/Desktop/Work/моя прога/MoiSoftBleat/MoiSoftBleat/bin/Debug/2416548.png")) };

            SelectedPictureImage2 = img;



            #endregion

        }

        #region ICommand members

        public ICommand selectLoadFolder { get; }

        #endregion


        #region Property update

        void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void OnGridSelectedItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            OnPropertyChanged("SelectedPictureImage");
        }

        #endregion

        #region MyRegion

        #endregion

        /// <summary>
        /// Обработчик собития выбора папки с картинками
        /// </summary>
        public void SelectLoadFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                {
                    FolderPath = folderBrowserDialog.SelectedPath;
                    OnPropertyChanged("FolderPath");

                    #region Загрузка картинок в манагере

                    _picturesManager.SetFolder(FolderPath);
                    _picturesManager.LoadPictures();

                    #endregion
                    //Изменение источника данных для вьюхи и вызов обновления
                    Pictures = _picturesManager.Pictures.ToDictionary(t => t.Key, t => t.Value.Name);
                    OnPropertyChanged("Pictures");
                }
            }
        }



    }
}
