using Nop.Plugin.Widgets.Gallery.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.Widgets.Gallery.Data
{
    public class GalleryMap : EntityTypeConfiguration<Galleries>
    {
        public GalleryMap()
        {
            this.ToTable("Gallery");
            this.HasKey(m => m.Id);
            this.Property(m => m.Name).IsRequired().HasMaxLength(400);
            this.Property(m => m.Description);
            this.Property(m => m.SimpleGallery);
            this.Property(m => m.FlickrGallery);
            this.Property(m => m.PicasaGallery);
            this.Property(m => m.VideoGallery);
        }
    }
}
