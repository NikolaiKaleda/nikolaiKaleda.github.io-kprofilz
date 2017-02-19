using Nop.Core.Domain.Localization;
using Nop.Core;

namespace Nop.Plugin.Widgets.Gallery.Domain
{
    /// <summary>
    /// Represents a galleries
    /// </summary>
    public class Galleries : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the parent picture identifier
        /// </summary>
        public virtual int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        public virtual bool SimpleGallery { get; set; }

        public virtual bool FlickrGallery { get; set; }
        public virtual string FlickrKeyword { get; set; }

        public virtual bool PicasaGallery { get; set; }
        public virtual string PicasaKeyword { get; set; }

        public virtual bool VideoGallery { get; set; }
        public virtual string VideoLink { get; set; }

    }
}
