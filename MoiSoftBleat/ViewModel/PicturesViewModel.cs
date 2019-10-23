using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using MoiSoftBleat.Data;

namespace MoiSoftBleat.ViewModel
{
    class PicturesViewModel: INotifyPropertyChanged //DependencyObject потом разобраться в этом
    {
        private PicturesManager _picturesManager;

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string prop = "Pictures")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public Dictionary<Guid,Picture> Pictures { get; set; }

        public PicturesViewModel()
        {
            _picturesManager = new PicturesManager();
            Pictures = _picturesManager.Pictures;

            OnPropertyChanged();
           // Pictures = CollectionViewSource.GetDefaultView(_picturesManager.Pictures);
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
