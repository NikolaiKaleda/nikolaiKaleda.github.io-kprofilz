using System.Collections.Generic;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;

namespace Nop.Plugin.Widgets.Gallery.Models
{
    public class PublicInfoGalleryModel : BaseNopEntityModel
    {
        public PublicInfoGalleryModel()
        {
            GalleryPictureModel = new List<PictureModel>();
            ImageGalleryPictureModel = new List<PictureModel>();
            ImageGallery=new List<ImageGalleryModel>();
            ImageModel = new List<ImageModel>();
        }

        public string GalleryName { get; set; }
        public string GalleryDescription { get; set; }

        public string ImageName { get; set; }
        public string ImageDescription { get; set; }

        public List<PictureModel> GalleryPictureModel { get; set; }
        public List<PictureModel> ImageGalleryPictureModel { get; set; }

        public List<ImageGalleryModel> ImageGallery { get; set; }
        public List<ImageModel> ImageModel { get; set; }

        public bool ShowImageAfterGalleries { get; set; }
        public bool ShowGallery { get; set; }

        public bool ShowImages { get; set; }

    }

    public class ImageGalleryModel
    {
        public ImageGalleryModel()
        {
            ImageGalleryPictureModel = new PictureModel();
        }

        public PictureModel ImageGalleryPictureModel { get; set; }
        public int Id { get; set; }
        public string Url { get; set; }
    }

    public class ImageModel
    {
        public ImageModel()
        {
            ImageGalleryPictureModel = new PictureModel();
        }

        public PictureModel ImageGalleryPictureModel { get; set; }
        public bool ShareImage { get; set; }
        public int Id { get; set; }
    }

    public class PagingModel
    {
        public int Page { get; set; }
        public double Counter { get; set; }
        public string UrlNow { get; set; }
        public string UrlPrev { get; set; }
        public string UrlNext { get; set; }
        public string UrlPrelast { get; set; }
        public string UrlLast { get; set; }
    }

    public class GalleryNavModel
    {
        public GalleryNavModel()
        {
            IDs= new List<int>();
            Names=new List<string>();
            MenuItems=new Dictionary<int, string>();
        }

        public List<int> IDs { get; set; }
        public List<string> Names { get; set; }
        public Dictionary<int, string> MenuItems { get; set; }
        public int CurrentID { get; set; }
        public string CurrentName { get; set; }
    }

}
  