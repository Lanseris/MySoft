using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiSoftBleat.Data
{
    class Picture
    {
        public readonly Guid pictureUid;

        private string _name;

        public string Name => _name;

        private List<string> _tags;

        public List<string> Tags => _tags;

        public Picture(string name, List<string> tags)
        {
            if (name == null)
                throw new ArgumentNullException();

            pictureUid = Guid.NewGuid();
            _name = name;
            _tags = tags;
        }
    }
}
