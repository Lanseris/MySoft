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

namespace MoiSoftBleat.ViewModel
{
    class PicturesViewModel: DependencyObject, INotifyPropertyChanged  //потом разобраться в этом
    {
        private  PicturesManager _picturesManager;

        public event PropertyChangedEventHandler PropertyChanged;

        //ПОДУМАТЬ КАК ВЫЗВАТЬ СОБЫТИЕ ПРИ СМЕНЕ ФОКУСА ЭЛЕМЕНТА
        //public event SelectedGridItemChangedEventHandler SelectedItemChanged;

        public Dictionary<Guid, string> Pictures { get; set; }

        public string FolderPath { get; set; }


        public KeyValuePair<Guid,string> SelectedItem
        {
            get { return (KeyValuePair<Guid,string>)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(KeyValuePair<Guid,string>), typeof(PicturesViewModel), new PropertyMetadata(null,SelectedItem_Changed));

        private static void SelectedItem_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _selectedPicture = _picturesManager.Pictures.FirstOrDefault(x => x.Key == ((KeyValuePair<Guid, string>)e.NewValue).Key).Value;
            SelectedPictureImage = _selectedPicture.Image;
            OnPropertyChanged("SelectedPicture");
            OnPropertyChanged("SelectedPictureImage");
        }

        public Image SelectedPictureImage { get; set; }

        public PicturesViewModel()
        {
            _picturesManager = new PicturesManager();
            Pictures = _picturesManager.Pictures.ToDictionary(t => t.Key, t => t.Value.Name);

            OnPropertyChanged("Pictures");

            #region ICommand init

            selectLoadFolder = new Command(SelectLoadFolder);

            #endregion

            #region Events

          // SelectedItemChanged += OnGridSelectedItemChanged;

            #endregion


            #region DependencyObject
            // Pictures = CollectionViewSource.GetDefaultView(_picturesManager.Pictures); 
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
            //SelectedPicture = _picturesManager.Pictures.FirstOrDefault(x=>x.Key == ((KeyValuePair<Guid,string>)e.NewSelection.Value).Key).Value;
            //SelectedPictureImage = SelectedPicture.Image;
            //OnPropertyChanged("SelectedPicture");
            //OnPropertyChanged("SelectedPictureImage");
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

        #region DependencyObject
        //public string FilterTest
        //{
        //    get { return (string)GetValue(FilterTestProperty); }
        //    set { SetValue(FilterTestProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for FilterTest.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty FilterTestProperty =
        //    DependencyProperty.Register("FilterTest", typeof(string), typeof(PicturesViewModel), new PropertyMetadata(""));



        //public ICollectionView Pictures
        //{
        //    get { return (ICollectionView)GetValue(PicturesProperty); }
        //    set { SetValue(PicturesProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Pictures.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PicturesProperty =
        //    DependencyProperty.Register("Pictures", typeof(ICollectionView), typeof(PicturesViewModel), new PropertyMetadata(null));

        #endregion



    }
}
