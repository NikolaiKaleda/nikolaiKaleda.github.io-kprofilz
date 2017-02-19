using System;
using System.Collections.Generic;
using Nop.Core.Domain.Localization;
using Nop.Core;

namespace Nop.Plugin.Widgets.Gallery.Domain
{
    /// <summary>
    /// Represents a gallery images
    /// </summary>
    public class GalleryImages : BaseEntity, ILocalizedEntity
    {
        private ICollection<Galleries> _galleries;
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

        /// <summary>
        /// Gets or sets the date and time of product creation
        /// </summary>
        public DateTime? CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of product update
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual ICollection<Galleries> Galleries
        {
            get { return _galleries ?? (_galleries = new List<Galleries>()); }
            protected set { _galleries = value; }
        }
    }
}
