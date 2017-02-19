using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;

namespace Nop.Plugin.Widgets.Gallery.Models
{
    [Validator(typeof(ConfigurationGalleryValidator))]
    public partial class ConfigurationGalleryModel : BaseNopEntityModel, ILocalizedModel<GalleryLocalizedModel>
    {
        public ConfigurationGalleryModel()
        {
            Locales = new List<GalleryLocalizedModel>();
        }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.Description")]
        [AllowHtml]
        public string Description { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.SimpleGallery")]
        public bool SimpleGallery { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.FlickrGallery")]
        public bool FlickrGallery { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.FlickrKeyword")]
        public virtual string FlickrKeyword { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.VideoGallery")]
        public bool VideoGallery { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.VideoLink")]
        public string VideoLinkPart { get; set; }

        public string VideoLink { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.PicasaGallery")]
        public bool PicasaGallery { get; set; }
        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.PicasaKeyword")]
        public virtual string PicasaKeyword { get; set; }

        public IList<GalleryLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.GalleryType")]
        public string GalleryType { get; set; }
        public bool SliderGallery { get; set; }


        public string[] SelectedOptions { get; set; }
        private IList<SelectListItem> _videoLinks = new List<SelectListItem>();
        public IList<SelectListItem> VideoLinks
        {
            get { return _videoLinks; }
             set { _videoLinks=value; }
        }


    }

    public partial class GalleryLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.Gallery.ConfigurationGalleryModel.Description")]
        [AllowHtml]
        public string Description { get; set; }

    }
}
