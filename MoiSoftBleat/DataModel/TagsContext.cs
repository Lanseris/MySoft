namespace MoiSoftBleat.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TagsContext : DbContext
    {
        public TagsContext()
            : base("name=TagsContext")
        {
        }
        public DbSet<Tag> tags { get; set; }
    }
}