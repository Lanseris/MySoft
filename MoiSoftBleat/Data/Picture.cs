using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MoiSoftBleat.DataModel;

namespace MoiSoftBleat.Data
{
    public class Picture
    {
        private PictureData _pictureData;

        public PictureData PictureData=>_pictureData;


        private Image _image;

        public Image Image => _image;

        /// <summary>
        /// Если не указать Guid, сгенерит новый
        /// </summary>
        /// <param name="pictureData"></param>
        /// <param name="tags"></param>
        /// <param name="image"></param>
        /// <param name="guid"></param>
        public Picture(PictureData pictureData, List<string> tags, Image image)
        {
            _pictureData = pictureData ?? throw new ArgumentNullException(nameof(pictureData));
            _image = image ?? throw new ArgumentNullException(nameof(image));
        }
    }
}
