using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.Gallery
{

    public class GallerySettings : ISettings
    {
        public string WidgetZones { get; set; }
        public string GalleryWidgetZones { get; set; }
        public string GalleryWidgetName { get; set; }
        public bool ShowImageAfterGalleries { get; set; }
        public bool ShowGalleries { get; set; }
        public int ItemPerPage { get; set; }
        public int ItemPerWidgetPage { get; set; }
        public bool SliderGallery { get; set; }
        public bool SortByCreatedDate { get; set; }
        public bool ShareImage { get; set; }

    }
}
