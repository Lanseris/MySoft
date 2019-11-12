namespace MoiSoftBleat.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class PicturesContext : DbContext
    {
        public PicturesContext()
            : base("name=PicturesContext")
        {
        }

        public DbSet<PictureData> picturesData { get; set; }

    }
}