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
    class PicturesViewModel : INotifyPropertyChanged  //потом разобраться в этом , DependencyObject
    {
        private static PicturesManager _picturesManager;

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<Guid, Picture> Pictures { get; set; }

        public string FolderPath { get; set; }

        private KeyValuePair<Guid, Picture> selectedItem;
        public KeyValuePair<Guid, Picture> SelectedItem 
        {
            get { return selectedItem; }
            set 
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            } 
        }

        #region DependencyProperty РЕАЛИЗОВАНО БЕЗ НЕГО

        //public KeyValuePair<Guid, Picture> SelectedItem
        //{
        //    get { return (KeyValuePair<Guid, Picture>)GetValue(SelectedItemProperty); }
        //    set { SetValue(SelectedItemProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        //public readonly DependencyProperty SelectedItemProperty =
        //    DependencyProperty.Register("SelectedItem", typeof(KeyValuePair<Guid, Picture>), typeof(PicturesViewModel), new PropertyMetadata(new KeyValuePair<Guid, Picture>(), SelectedItem_Changed));

        //private static void SelectedItem_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //}

        #endregion

        public static System.Windows.Controls.Image SelectedPictureImage { get; set; }

        public PicturesViewModel()
        {
            _picturesManager = new PicturesManager();

            Pictures = _picturesManager.Pictures;

            OnPropertyChanged("Pictures");

            #region ICommand init

            selectLoadFolder = new Command(SelectLoadFolder);

            #endregion

            #region Events //пусто

            //SelectedGridItemChanged += OnGridSelectedItemChanged;

            #endregion

            #region ТЕСТЫ!!!!!! //пусто

            #endregion

        }

        #region ICommand members

        //команда, вызывающаяся при нажатии кнопки выбора папки с картинками
        public ICommand selectLoadFolder { get; }

        #endregion


        #region Property update

        //Список свойств, обновляющихся через этот метод:
        //1 - FolderPath - путь к папке с картинками
        void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //public void OnGridSelectedItemChanged()
        //{
        //    OnPropertyChanged("SelectedItem");
        //}

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
                    Pictures = _picturesManager.Pictures;
                    OnPropertyChanged("Pictures");
                }
            }
        }



    }
}
