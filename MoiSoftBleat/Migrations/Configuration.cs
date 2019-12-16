namespace MoiSoftBleat.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MoiSoftBleat.DataModel.PicturesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MoiSoftBleat.DataModel.PicturesContext context)
        {
        }
    }
}
