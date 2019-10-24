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
    class PicturesViewModel: INotifyPropertyChanged //DependencyObject потом разобраться в этом
    {
        private PicturesManager _picturesManager;

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<Guid, string> Pictures { get; set; }

        public string FolderPath { get; set; }

        public PicturesViewModel()
        {
            _picturesManager = new PicturesManager();
            Pictures = _picturesManager.Pictures.ToDictionary(t => t.Key, t => t.Value.Name);

            OnPropertyChanged("Pictures");

            #region ICommand init

            selectLoadFolder = new Command(SelectLoadFolder);
            
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

        #endregion

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
