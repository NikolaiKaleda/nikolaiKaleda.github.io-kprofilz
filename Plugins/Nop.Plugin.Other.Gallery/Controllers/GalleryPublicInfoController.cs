using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Nop.Plugin.Widgets.Gallery.Domain;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Admin.Controllers;
using Nop.Plugin.Widgets.Gallery.Services;
using Nop.Web.Models.Media;
using Nop.Plugin.Widgets.Gallery.Models;
using System.Linq;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.Gallery.Controllers
{
    public class GalleryPublicInfoController : BasePluginController
    {


        #region Const

        private const string GALLERY_PICTURE_MODEL_KEY = "nop.pres.gallery.picture-{0}-{1}-{2}-{3}-{4}";
        private const int GalleryThumbPictureSize = 300;

        #endregion


        #region Fields

        private readonly IGalleryService _galleryService;
        private readonly IPictureService _pictureService;
        private readonly GallerySettings _gallerySettings;
        private readonly ILocalizationService _localizationService;


        #endregion


        #region Constructors

        public GalleryPublicInfoController(IGalleryService galleryService, IPictureService pictureService,
            GallerySettings gallerySettings, ILocalizationService localizationService)
        {
            _galleryService = galleryService;
            _pictureService = pictureService;
            _gallerySettings = gallerySettings;
            _localizationService = localizationService;

        }

        #endregion


        #region Methods


        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/PublicInfo.cshtml");
        }

        [HttpPost]
        public ActionResult GalleryList()
        {
            var galleries = _galleryService.GetGalleryPages();
            var model = new List<ImageGalleryModel>();
            foreach (var gal in galleries)
            {
                var alt = gal.GetLocalized(x => x.Description) != null ? gal.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "") : "";
                model.Add(new ImageGalleryModel
                {
                    Id = gal.Id,
                    Url = Url.Action("ImageList", "GalleryPublicInfo", new { id = gal.Id }),
                    ImageGalleryPictureModel = new PictureModel
                    {
                        FullSizeImageUrl = _pictureService.GetPictureUrl(gal.PictureId),
                        ImageUrl = _pictureService.GetPictureUrl(gal.PictureId, GalleryThumbPictureSize),
                        Title = gal.GetLocalized(x => x.Name),
                        AlternateText = alt,
                    }
                });
            }
            return Json(new
            {
                success = true,
                data = model
            });
        }

        [HttpPost]
        public ActionResult ImageList(int id)
        {
            //var gallery = _galleryService.GetGalleryById(id);
            //if (gallery == null)
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        message = "Gallery with id '" + id + "' isn't found"
            //    });
            //}
            var galleryImages = _galleryService.GetGalleryImagesByGalleryId(id);
            var model = new List<ImageGalleryModel>();
            foreach (var gi in galleryImages)
            {
                var alt = gi.GetLocalized(x => x.Description) != null ? gi.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "") : "";
                model.Add(new ImageGalleryModel
                {
                    Id = gi.Id,
                    ImageGalleryPictureModel = new PictureModel
                    {
                        FullSizeImageUrl = _pictureService.GetPictureUrl(gi.PictureId),
                        ImageUrl = _pictureService.GetPictureUrl(gi.PictureId, GalleryThumbPictureSize),
                        Title = gi.GetLocalized(x => x.Name),
                        AlternateText = alt,
                    }
                });
            }
            return Json(new
            {
                success = true,
                data = model
            });
        }

        [HttpPost]
        public ActionResult List(int page = 1)
        {
            if (page == 0)
            {
                return RedirectToAction("PageNotFound", "Common");
            }

            var model = new PublicInfoGalleryModel();
            model.ShowGallery = _gallerySettings.ShowGalleries;
            if (_gallerySettings.ShowGalleries)
            {
                var galleries = _galleryService.GetGalleryPages(page, _gallerySettings.ItemPerPage < 1 ? 1 : _gallerySettings.ItemPerPage);
                foreach (var gal in galleries)
                {
                    var alt = gal.GetLocalized(x => x.Description) != null ? gal.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "") : "";
                    model.ImageGallery.Add(new ImageGalleryModel
                        {
                            Id = gal.Id,
                            ImageGalleryPictureModel = new PictureModel
                                {
                                    FullSizeImageUrl = _pictureService.GetPictureUrl(gal.PictureId),
                                    ImageUrl = _pictureService.GetPictureUrl(gal.PictureId, GalleryThumbPictureSize),
                                    Title = gal.GetLocalized(x => x.Name),
                                    AlternateText = alt,
                                }
                        });
                }

                if (_gallerySettings.ShowImageAfterGalleries)
                {
                    var count = Convert.ToInt32(Math.Ceiling(_galleryService.Counter(_gallerySettings.ItemPerPage < 1 ? 1 : _gallerySettings.ItemPerPage)));
                    if (page >= count)
                    {
                        if (count == page) page = 1;
                        if (count < page) page = page - count + 1;

                        model.ShowImages = true;
                        var imagesWithoutGallery = _galleryService.GetImagesPagesWithoutGallery(page, _gallerySettings.ItemPerPage < 1 ? 1 : _gallerySettings.ItemPerPage);
                        foreach (var image in imagesWithoutGallery)
                        {
                            var alt = image.GetLocalized(x => x.Description) != null ? image.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "") : "";
                            model.ImageModel.Add(new ImageModel
                            {
                                ShareImage = _gallerySettings.ShareImage,
                                Id = image.Id,
                                ImageGalleryPictureModel = new PictureModel
                                {
                                    FullSizeImageUrl = _pictureService.GetPictureUrl(image.PictureId),
                                    ImageUrl =
                                        _pictureService.GetPictureUrl(image.PictureId, GalleryThumbPictureSize),
                                    Title = image.GetLocalized(x => x.Name),
                                    AlternateText = alt,
                                }
                            });
                        }
                    }

                    return Json(new
                    {
                        success = true,
                        data = model,
                    });
                    //return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/GalleryWithAloneImages.cshtml", model);
                }

                return Json(new
                {
                    success = true,
                    data = model,
                });
                //return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/GalleryList.cshtml", model);
            }

            var images = _galleryService.GetGalleryImagesPages(page, _gallerySettings.ItemPerPage < 1 ? 1 : _gallerySettings.ItemPerPage);
            foreach (var image in images)
            {
                var alt = image.GetLocalized(x => x.Description) != null ? image.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "") : "";
                model.ImageModel.Add(new ImageModel
                            {
                                ShareImage = _gallerySettings.ShareImage,
                                Id = image.Id,
                                ImageGalleryPictureModel = new PictureModel
                                {
                                    FullSizeImageUrl = _pictureService.GetPictureUrl(image.PictureId),
                                    ImageUrl = _pictureService.GetPictureUrl(image.PictureId, GalleryThumbPictureSize),
                                    Title = image.GetLocalized(x => x.Name),
                                    AlternateText = alt,
                                }
                            });
            }
            return Json(new
            {
                success = true,
                data = model,
            });
            //return View(_gallerySettings.SliderGallery ? "~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/GalleriaList.cshtml" : "~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/List.cshtml.cshtml", model);
        }

        public ActionResult Gallery(int id, int page = 1)
        {
            if (page == 0)
            {
                return RedirectToAction("PageNotFound", "Common");
            }
            ViewBag.GalleryId = id;
            var model = new PublicInfoGalleryModel();
            model.ShowGallery = _gallerySettings.ShowGalleries;
            if (!_gallerySettings.SliderGallery)
            {
                var images = _galleryService.GetGalleryImagesPagesByGalleryId(id, page, _gallerySettings.ItemPerPage);

                foreach (var image in images)
                {
                    var alt = image.GetLocalized(x => x.Description) != null
                                  ? image.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "")
                                  : "";
                    model.ImageModel.Add(new ImageModel
                           {
                               ShareImage = _gallerySettings.ShareImage,
                               Id = image.Id,
                               ImageGalleryPictureModel = new PictureModel
                               {
                                   FullSizeImageUrl = _pictureService.GetPictureUrl(image.PictureId),
                                   ImageUrl = _pictureService.GetPictureUrl(image.PictureId, GalleryThumbPictureSize),
                                   Title = image.GetLocalized(x => x.Name),
                                   AlternateText = alt,
                               }
                           });
                }
                return
                    View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/List.cshtml", model);
            }
            else
            {
                var gallery = _galleryService.GetGalleryById(id);
                if (gallery.SimpleGallery)
                {
                    var images = _galleryService.GetGalleryImagesByGalleryId(id);
                    foreach (var image in images)
                    {
                        var alt = image.GetLocalized(x => x.Description) != null
                                      ? image.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "")
                                      : "";
                        model.ImageModel.Add(new ImageModel
                           {
                               ShareImage = _gallerySettings.ShareImage,
                               Id = image.Id,
                               ImageGalleryPictureModel = new PictureModel
                               {
                                   FullSizeImageUrl = _pictureService.GetPictureUrl(image.PictureId),
                                   ImageUrl = _pictureService.GetPictureUrl(image.PictureId, GalleryThumbPictureSize),
                                   Title = image.GetLocalized(x => x.Name),
                                   AlternateText = alt,
                               }
                           });
                    }
                    return
                    View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/GalleriaList.cshtml", model);
                }
                if (gallery.FlickrGallery)
                {
                    ViewBag.FlickrKeysords = gallery.FlickrKeyword;
                    return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/FlickrList.cshtml");
                }
                if (gallery.PicasaGallery)
                {
                    ViewBag.PicasaKeyword = gallery.PicasaKeyword;
                    return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/PicasaList.cshtml");
                }
                if (gallery.VideoGallery)
                {
                    var videoLinks = new List<string>();
                    if (gallery.VideoLink != null)
                    {
                        videoLinks = gallery.VideoLink.Split(new[] { "%,%" }, StringSplitOptions.None).ToList();
                    }
                    return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/VideoList.cshtml", videoLinks);
                }
                return null;
            }
        }

        public ActionResult SingleImage(int id, int? galleryId)
        {
            var image = _galleryService.GetGalleryImageById(id);
            var alt = image.GetLocalized(x => x.Description) != null
                                      ? image.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "")
                                      : "";
            var model = new PictureModel
            {
                FullSizeImageUrl = _pictureService.GetPictureUrl(image.PictureId),
                ImageUrl = _pictureService.GetPictureUrl(image.PictureId, GalleryThumbPictureSize),
                Title = image.GetLocalized(x => x.Name),
                AlternateText = alt,
            };
            ViewBag.ShowGalleries = _gallerySettings.ShowGalleries;
            ViewBag.Id = galleryId;
            return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/SingleImage.cshtml", model);
        }


        public ActionResult WidgetGallery(int page = 1)
        {
            int id;
            if (page == 0)
            {
                return RedirectToAction("PageNotFound", "Common");
            }

            var model = new PublicInfoGalleryModel();
            if (!_gallerySettings.SliderGallery)
            {
                List<GalleryImages> images;
                if (!Int32.TryParse(_gallerySettings.GalleryWidgetName, out id))
                {
                    images = _galleryService.GetGalleryImagesPages(page, _gallerySettings.ItemPerWidgetPage < 1 ? 1 : _gallerySettings.ItemPerWidgetPage).ToList();
                }
                else if (id == -1)
                {
                    images = _galleryService.GetGalleryImagesPages(page, _gallerySettings.ItemPerWidgetPage < 1 ? 1 : _gallerySettings.ItemPerWidgetPage).ToList();
                }
                else
                {
                    images = _galleryService.GetGalleryImagesByGalleryId(id).ToList();
                }
                ViewBag.GalleryId = id;
                foreach (var image in images)
                {
                    var alt = image.GetLocalized(x => x.Description) != null
                        ? image.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "")
                        : "";
                    if (model.ImageGalleryPictureModel.Count < _gallerySettings.ItemPerWidgetPage)
                    {
                        model.ImageModel.Add(new ImageModel
                            {
                                ShareImage = _gallerySettings.ShareImage,
                                Id = image.Id,
                                ImageGalleryPictureModel = new PictureModel
                                {
                                    FullSizeImageUrl = _pictureService.GetPictureUrl(image.PictureId),
                                    ImageUrl = _pictureService.GetPictureUrl(image.PictureId, GalleryThumbPictureSize),
                                    Title = image.GetLocalized(x => x.Name),
                                    AlternateText = alt,
                                }
                            });
                    }
                }
                return
                    View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/WidgetList.cshtml", model);
            }
            else
            {
                List<GalleryImages> images;
                if (!Int32.TryParse(_gallerySettings.GalleryWidgetName, out id))
                {
                    images = _galleryService.GetGalleryImagesPages(page, _gallerySettings.ItemPerWidgetPage < 1 ? 1 : _gallerySettings.ItemPerWidgetPage).ToList();
                }
                else if (id == -1)
                {
                    images = _galleryService.GetGalleryImagesPages(page, _gallerySettings.ItemPerWidgetPage < 1 ? 1 : _gallerySettings.ItemPerWidgetPage).ToList();
                    foreach (var image in images)
                    {
                        var alt = image.GetLocalized(x => x.Description) != null
                            ? image.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "")
                            : "";
                        model.ImageModel.Add(new ImageModel
                           {
                               ShareImage = _gallerySettings.ShareImage,
                               ImageGalleryPictureModel = new PictureModel
                               {
                                   FullSizeImageUrl = _pictureService.GetPictureUrl(image.PictureId),
                                   ImageUrl = _pictureService.GetPictureUrl(image.PictureId, GalleryThumbPictureSize),
                                   Title = image.GetLocalized(x => x.Name),
                                   AlternateText = alt,
                               }
                           });

                    }
                    return
                        View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/WidgetGalleriaList.cshtml", model);
                }
                else
                {
                    images = _galleryService.GetGalleryImagesByGalleryId(id).ToList();
                }
                var gallery = _galleryService.GetGalleryById(id);
                if (gallery.SimpleGallery)
                {
                    foreach (var image in images)
                    {
                        var alt = image.GetLocalized(x => x.Description) != null
                            ? image.GetLocalized(x => x.Description).Replace("<p>", "").Replace("</p>", "")
                            : "";
                        model.ImageModel.Add(new ImageModel
                            {
                                ShareImage = _gallerySettings.ShareImage,
                                ImageGalleryPictureModel = new PictureModel
                                {
                                    FullSizeImageUrl = _pictureService.GetPictureUrl(image.PictureId),
                                    ImageUrl = _pictureService.GetPictureUrl(image.PictureId, GalleryThumbPictureSize),
                                    Title = image.GetLocalized(x => x.Name),
                                    AlternateText = alt,
                                }
                            });
                    }
                    return
                        View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/WidgetGalleriaList.cshtml", model);
                }
                if (gallery.FlickrGallery)
                {
                    ViewBag.FlickrKeysords = gallery.FlickrKeyword;
                    return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/WidgetFlickrList.cshtml");
                }
                if (gallery.PicasaGallery)
                {
                    ViewBag.PicasaKeyword = gallery.PicasaKeyword;
                    return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/WidgetPicasaList.cshtml");
                }
                if (gallery.VideoGallery)
                {
                    var videoLinks = new List<string>();
                    if (gallery.VideoLink != null)
                    {
                        videoLinks = gallery.VideoLink.Split(new[] { "%,%" }, StringSplitOptions.None).ToList();
                    }
                    return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/WidgetVideoList.cshtml", videoLinks);
                }
            }
            return null;
        }

        public ActionResult LeftSideMenu(int? id)
        {
            var model = new GalleryNavModel();

            var galleries = _galleryService.GetAllGalleries();

            foreach (var gallery in galleries)
            {
                if (id != null && gallery.Id == id)
                {
                    model.CurrentID = gallery.Id;
                }
                model.MenuItems.Add(gallery.Id, gallery.Name);
                model.IDs.Add(gallery.Id);
                model.Names.Add(gallery.Name);
            }

            return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/LeftSideMenu.cshtml", model);
        }

        public ActionResult CenterMenu(int? id)
        {
            var model = new GalleryNavModel();

            var galleries = _galleryService.GetAllGalleries();

            foreach (var gallery in galleries)
            {
                if (id != null && gallery.Id == id)
                {
                    model.CurrentID = gallery.Id;
                }
                model.MenuItems.Add(gallery.Id, gallery.Name);
                model.IDs.Add(gallery.Id);
                model.Names.Add(gallery.Name);
            }

            return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/CenterMenu.cshtml", model);
        }


        public ActionResult GalleryBreadcrumb(int id)
        {
            var model = new GalleryNavModel();

            var gallery = _galleryService.GetGalleryById(id);

            model.CurrentID = gallery.Id;
            model.CurrentName = gallery.Name;

            return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/GalleryBreadcrumb.cshtml", model);
        }

        public ActionResult SingleImageBreadcrumb(string imageTitle, int id = -1)
        {
            var model = new GalleryNavModel();

            var gallery = _galleryService.GetGalleryById(id);
            if (gallery != null)
            {
                model.MenuItems.Add(gallery.Id, gallery.Name);
            }
            model.CurrentName = imageTitle;

            return View("~/Plugins/Widgets.Gallery/Views/GalleryPublicInfo/SingleImageBreadcrumb.cshtml", model);
        }



        public MvcHtmlString Paging()
        {
            var count = Math.Ceiling(_galleryService.Counter(_gallerySettings.ItemPerPage < 1 ? 1 : _gallerySettings.ItemPerPage));

            if (Request.Url == null) throw new ArgumentNullException("request url is null");

            var urlNow = Request.Url.ToString();
            var posQueryString = urlNow.IndexOf('?');
            if (posQueryString >= 0)
            {
                urlNow = urlNow.Remove(posQueryString);
            }
            var page = Request.QueryString["page"] != null ? Convert.ToInt32(Request.QueryString["page"]) : 1;

            if (count > 1)
            {
                var sb = new StringBuilder();
                sb.Append(@"<div class=""pagination pagination-centered""><ul>");
                if (page >= 4)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, 1));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.First"));
                    sb.Append("</a></li>");
                }

                if (page != 1)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, page - 1));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.Previous"));
                    sb.Append("</a></li>");
                }

                if (page < 4)
                {
                    if (count < 5)
                    {
                        for (var i = 1; i <= count; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                    else
                    {
                        for (var i = 1; i <= 5; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                }

                if (page >= 4 && page + 2 < count)
                {
                    for (var i = (page - 2); i <= (page + 2); i++)
                    {
                        sb.Append(@"<li><a href=""");
                        sb.Append(String.Format("{0}?page={1}", urlNow, i));
                        sb.Append(@""">");
                        sb.Append(i);
                        sb.Append("</a></li>");
                    }
                }

                if (page >= 4 && page + 2 >= count)
                {
                    for (var i = (count - 4); i <= count; i++)
                    {
                        sb.Append(@"<li><a href=""");
                        sb.Append(String.Format("{0}?page={1}", urlNow, i));
                        sb.Append(@""">");
                        sb.Append(i);
                        sb.Append("</a></li>");
                    }
                }

                if (page != count)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, page + 1));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.Next"));
                    sb.Append("</a></li>");
                }
                if (page + 2 < count)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, count));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.Last"));
                    sb.Append("</a></li>");
                }

                sb.Append("</ul></div>");
                return MvcHtmlString.Create(sb.ToString());
            }

            return null;
        }

        public MvcHtmlString GalleryPaging(int? id)
        {
            var count = Math.Ceiling(!_gallerySettings.ShowGalleries ? _galleryService.ImagesCounter(_gallerySettings.ItemPerPage < 1 ? 1 : _gallerySettings.ItemPerPage) :
                _galleryService.ImagesCounter(_gallerySettings.ItemPerPage < 1 ? 1 : _gallerySettings.ItemPerPage, Convert.ToInt32(id)));

            if (Request.Url == null) throw new ArgumentNullException("request url is null");

            var urlNow = Request.Url.ToString();
            var posQueryString = urlNow.IndexOf('?');
            if (posQueryString >= 0)
            {
                urlNow = urlNow.Remove(posQueryString);
            }
            var page = Request.QueryString["page"] != null ? Convert.ToInt32(Request.QueryString["page"]) : 1;

            if (count > 1)
            {
                var sb = new StringBuilder();
                sb.Append(@"<div class=""pagination pagination-centered""><ul>");
                if (page >= 4)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, 1));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.First"));
                    sb.Append("</a></li>");
                }

                if (page != 1)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, page - 1));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.Previous"));
                    sb.Append("</a></li>");
                }

                if (page < 4)
                {
                    if (count < 5)
                    {
                        for (var i = 1; i <= count; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                    else
                    {
                        for (var i = 1; i <= 5; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                }

                if (page >= 4 && page + 2 < count)
                {
                    for (var i = (page - 2); i <= (page + 2); i++)
                    {
                        sb.Append(@"<li><a href=""");
                        sb.Append(String.Format("{0}?page={1}", urlNow, i));
                        sb.Append(@""">");
                        sb.Append(i);
                        sb.Append("</a></li>");
                    }
                }

                if (page >= 4 && page + 2 >= count)
                {
                    if (count == 4)
                    {
                        for (var i = 1; i <= 4; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                    else
                    {
                        for (var i = (count - 4); i <= count; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                }

                if (page != count)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, page + 1));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.Next"));
                    sb.Append("</a></li>");
                }
                if (page + 2 < count)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, count));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.Last"));
                    sb.Append("</a></li>");
                }

                sb.Append("</ul></div>");
                return MvcHtmlString.Create(sb.ToString());
            }

            return null;
        }

        public MvcHtmlString GalleryWithAloneImagesPaging()
        {
            var galleryCount = Math.Ceiling(_galleryService.Counter(_gallerySettings.ItemPerPage < 1 ? 1 : _gallerySettings.ItemPerPage));
            var countWithoutGallery = Math.Ceiling(_galleryService.ImagesWithoutGalleryCounter(_gallerySettings.ItemPerPage < 1 ? 1 : _gallerySettings.ItemPerPage));
            var count = galleryCount + countWithoutGallery - 1;
            if (Request.Url == null) throw new ArgumentNullException("request url is null");

            var urlNow = Request.Url.ToString();
            var posQueryString = urlNow.IndexOf('?');
            if (posQueryString >= 0)
            {
                urlNow = urlNow.Remove(posQueryString);
            }
            var page = Request.QueryString["page"] != null ? Convert.ToInt32(Request.QueryString["page"]) : 1;

            if (count > 1)
            {
                var sb = new StringBuilder();
                sb.Append(@"<div class=""pagination pagination-centered""><ul>");
                if (page >= 4)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, 1));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.First"));
                    sb.Append("</a></li>");
                }

                if (page != 1)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, page - 1));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.Previous"));
                    sb.Append("</a></li>");
                }

                if (page < 4)
                {
                    if (count < 5)
                    {
                        for (var i = 1; i <= count; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                    else
                    {
                        for (var i = 1; i <= 5; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                }

                if (page >= 4 && page + 2 < count)
                {
                    for (var i = (page - 2); i <= (page + 2); i++)
                    {
                        sb.Append(@"<li><a href=""");
                        sb.Append(String.Format("{0}?page={1}", urlNow, i));
                        sb.Append(@""">");
                        sb.Append(i);
                        sb.Append("</a></li>");
                    }
                }

                if (page >= 4 && page + 2 >= count)
                {
                    if (page == 4)
                    {
                        for (var i = 2; i <= count; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                    else
                    {
                        for (var i = (count - 4); i <= count; i++)
                        {
                            sb.Append(@"<li><a href=""");
                            sb.Append(String.Format("{0}?page={1}", urlNow, i));
                            sb.Append(@""">");
                            sb.Append(i);
                            sb.Append("</a></li>");
                        }
                    }
                }

                if (page != count)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, page + 1));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.Next"));
                    sb.Append("</a></li>");
                }
                if (page + 2 < count)
                {
                    sb.Append(@"<li><a href=""");
                    sb.Append(String.Format("{0}?page={1}", urlNow, count));
                    sb.Append(@""">");
                    sb.Append(_localizationService.GetResource("Plugin.Widgets.Gallery.Pager.Last"));
                    sb.Append("</a></li>");
                }

                sb.Append("</ul></div>");
                return MvcHtmlString.Create(sb.ToString());
            }

            return null;
        }

        #endregion

    }
}
