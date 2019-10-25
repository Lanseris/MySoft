using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoiSoftBleat.Data
{
    public class Picture
    {
        public readonly Guid pictureUid;

        private string _name;

        public string Name => _name;

        private Image _image;

        public Image Image => _image;

        private List<string> _tags;

        public List<string> Tags => _tags;

        public Picture(string name, List<string> tags, Image image)
        {
            pictureUid = Guid.NewGuid();
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _tags = tags ?? throw new ArgumentNullException(nameof(tags));
            _image = image ?? throw new ArgumentNullException(nameof(image));

        }
    }
}
