using Nop.Web.Framework;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.Gallery.Models
{
    public class ConfigurationGalleryImagesListModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesListModel.SearchGalleryImages")]
        [AllowHtml]
        public string SearchGalleryName { get; set; }

        public DataSourceResult Galleries { get; set; }
    }
}