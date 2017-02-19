using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.Gallery.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugin.Widgets.Gallery.WidgetZones")]
        public string WidgetZones { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.GalleryWidgetZones")]
        public string GalleryWidgetZones { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ShowImageAfterGalleries")]
        public bool ShowImageAfterGalleries { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ShowGallery")]
        public bool ShowGallery { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ProductPerPage")]
        public int ProductPerPage { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ProductPerWidgetPage")]
        public int ProductPerWidgetPage { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.GalleryType")]
        public string SliderGallery { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.ShareImage")]
        public bool ShareImage { get; set; }
        private readonly IList<SelectListItem> _galleryTypes = new List<SelectListItem>();
        public IList<SelectListItem> GalleryTypes
        {
            get { return _galleryTypes; }
        }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.SortByCreatedDate")]
        public string SortByCreatedDate { get; set; }
        private readonly IList<SelectListItem> _sortedFilters = new List<SelectListItem>();
        public IList<SelectListItem> SortedFilters
        {
            get { return _sortedFilters; }
        }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.GalleryWidgetName")]
        public string GalleryWidgetName { get; set; }
        private readonly IList<SelectListItem> _galleryWidgetName = new List<SelectListItem>();
        public IList<SelectListItem> GalleryNames
        {
            get { return _galleryWidgetName; }
        }

    }
}