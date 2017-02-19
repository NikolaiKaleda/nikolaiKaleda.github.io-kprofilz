using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;

namespace Nop.Plugin.Widgets.Gallery.Models
{
    [Validator(typeof(ConfigurationGalleryImagesValidator))]
    public class ConfigurationGalleryImagesModel : BaseNopEntityModel, ILocalizedModel<GalleryImagesLocalizedModel>
    {
        public ConfigurationGalleryImagesModel()
        {
            Locales = new List<GalleryImagesLocalizedModel>();
        }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.Description")]
        [AllowHtml]
        public string Description { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.Picture")]
        public int PictureId { get; set; }


        private readonly IList<SelectListItem> _galleries = new List<SelectListItem>();
        public IList<SelectListItem> ListGalleries
        {
            get { return _galleries; }
        }

        [UIHint("Gallery")]
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.GalleryId")]
        public int GalleryId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.GalleryId")]
        public List<int> GalleryIds { get; set; }

        public string[] SelectedOptions { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<GalleryImagesLocalizedModel> Locales { get; set; }

    }

    public class GalleryImagesLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.Description")]
        [AllowHtml]
        public string Description { get; set; }

    }
}
