using Nop.Web.Framework;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.Gallery.Models
{
    public partial class ConfigurationGalleryListModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.SearchGallery")]
        [AllowHtml]
        public string SearchGalleryName { get; set; }

        public DataSourceResult Galleries { get; set; }
    }
}