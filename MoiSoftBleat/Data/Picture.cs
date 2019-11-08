using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MoiSoftBleat.Data
{
    public class Picture
    {
        public readonly Guid pictureUid;

        //private string _name;

        //public string Name => _name;

        private PictureData _pictureData;

        public PictureData PictureData=>_pictureData;


        private Image _image;

        public Image Image => _image;

        private List<string> _tags;

        public List<string> Tags => _tags;

        /// <summary>
        /// Если не указать Guid, сгенерит новый
        /// </summary>
        /// <param name="pictureData"></param>
        /// <param name="tags"></param>
        /// <param name="image"></param>
        /// <param name="guid"></param>
        public Picture(PictureData pictureData, List<string> tags, Image image, Guid? guid = null)
        {
            if (guid != null)
                pictureUid = (Guid)guid;
            else
                pictureUid = Guid.NewGuid();

            _pictureData = pictureData ?? throw new ArgumentNullException(nameof(pictureData));
            _tags = tags ?? throw new ArgumentNullException(nameof(tags));
            _image = image ?? throw new ArgumentNullException(nameof(image));

        }
    }
}
