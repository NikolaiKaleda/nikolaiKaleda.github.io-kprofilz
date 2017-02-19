using Nop.Plugin.Widgets.Gallery.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.Widgets.Gallery.Data
{
    public partial class ImageGalleryMap : EntityTypeConfiguration<GalleryImages>
    {
        public ImageGalleryMap()
        {
            this.ToTable("ImageGallery");
            this.HasKey(m => m.Id);
            this.Property(m => m.Name).IsRequired().HasMaxLength(400);
            this.Property(m => m.Description);
            this.Property(m => m.CreatedOnUtc);
            this.Property(m => m.UpdatedOnUtc);

            this.HasMany(m => m.Galleries)
                .WithMany()
                .Map(m => m.ToTable("Galleries_GalleryImagesMapping"));
        }
    }
}
