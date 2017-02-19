using System;
using System.Collections.Generic;
using System.Globalization;
using Nop.Admin.Controllers;
using Nop.Admin.Models.Catalog;
using Nop.Core.Domain.Common;
using Nop.Plugin.Widgets.Gallery.Domain;
using Nop.Plugin.Widgets.Gallery.Models;
using Nop.Plugin.Widgets.Gallery.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using System.Linq;
using System.Web.Mvc;
using Nop.Plugin.Widgets.Gallery.Extensions.Configure;
using Nop.Web.Framework.Kendoui;

namespace Nop.Plugin.Widgets.Gallery.Controllers
{
    [AdminAuthorize]
    public class GalleryConfigureController : BasePluginController
    {
        #region Fields

        private readonly IGalleryService _galleryService;
        private readonly IPictureService _pictureService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly GallerySettings _gallerySettings;
        private readonly ISettingService _settingService;

        #endregion

        #region Constructors

        public GalleryConfigureController(IGalleryService galleryService,
            IPictureService pictureService,
            ILanguageService languageService, ILocalizationService localizationService, ILocalizedEntityService localizedEntityService,
            AdminAreaSettings adminAreaSettings, GallerySettings gallerySettings, ISettingService settingService)
        {
            _galleryService = galleryService;
            _pictureService = pictureService;
            _languageService = languageService;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _adminAreaSettings = adminAreaSettings;
            _gallerySettings = gallerySettings;
            _settingService = settingService;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected void UpdateLocales(Galleries gallery, ConfigurationGalleryModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(gallery,
                                                               x => x.Name,
                                                               localized.Name,
                                                               localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(gallery,
                                                           x => x.Description,
                                                           localized.Description,
                                                           localized.LanguageId);
            }
        }

        [NonAction]
        protected void UpdateImageLocales(GalleryImages gallery, ConfigurationGalleryImagesModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(gallery,
                                                               x => x.Name,
                                                               localized.Name,
                                                               localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(gallery,
                                                           x => x.Description,
                                                           localized.Description,
                                                           localized.LanguageId);
            }
        }

        [NonAction]
        protected void UpdatePictureSeoNames(Galleries gallery)
        {
            var picture = _pictureService.GetPictureById(gallery.PictureId);
            if (picture != null)
                _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(gallery.Name));
        }

        [NonAction]
        protected void UpdateImagePictureSeoNames(GalleryImages gallery)
        {
            var picture = _pictureService.GetPictureById(gallery.PictureId);
            if (picture != null)
                _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(gallery.Name));
        }

        #endregion

        [HttpGet]
        public ActionResult Configure()
        {
            var galleries = _galleryService.GetAllGalleries();
            var model = new ConfigurationModel
            {
                WidgetZones = _gallerySettings.WidgetZones,
                GalleryWidgetZones = _gallerySettings.GalleryWidgetZones,
                ShowGallery = _gallerySettings.ShowGalleries,
                ShowImageAfterGalleries = _gallerySettings.ShowImageAfterGalleries,
                ProductPerPage = _gallerySettings.ItemPerPage,
                ProductPerWidgetPage = _gallerySettings.ItemPerWidgetPage,
                SortByCreatedDate = _gallerySettings.SortByCreatedDate ? "true" : "false",
                SliderGallery = _gallerySettings.SliderGallery ? "true" : "false",
                GalleryWidgetName = _gallerySettings.GalleryWidgetName,
                ShareImage = _gallerySettings.ShareImage
            };
            model.GalleryNames.Add(new SelectListItem { Text = "*", Value = "-1" });
            if (galleries.Count > 0)
            {
                foreach (var gallery in galleries)
                {
                    model.GalleryNames.Add(new SelectListItem { Text = gallery.Name, Value = gallery.Id.ToString() });
                }
            }
            else
            {
                model.GalleryNames.Add(new SelectListItem { Text = "", Value = "" });

            }
            model.GalleryTypes.Add(new SelectListItem { Text = _localizationService.GetResource("Plugin.Widgets.Gallery.Slider"), Value = "true" });
            model.GalleryTypes.Add(new SelectListItem { Text = _localizationService.GetResource("Plugin.Widgets.Gallery.ImagesList"), Value = "false" });
            model.SortedFilters.Add(new SelectListItem { Text = _localizationService.GetResource("Plugin.Widgets.Gallery.SortByDate"), Value = "true" });
            model.SortedFilters.Add(new SelectListItem { Text = _localizationService.GetResource("Plugin.Widgets.Gallery.SortByDisplayOrder"), Value = "false" });

            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            _gallerySettings.WidgetZones = model.WidgetZones;
            _gallerySettings.GalleryWidgetZones = model.GalleryWidgetZones;
            _gallerySettings.ItemPerPage = model.ProductPerPage;
            _gallerySettings.ItemPerWidgetPage = model.ProductPerWidgetPage;
            _gallerySettings.ShowGalleries = model.ShowGallery;
            _gallerySettings.ShowImageAfterGalleries = model.ShowImageAfterGalleries;
            _gallerySettings.GalleryWidgetName = model.GalleryWidgetName;
            _gallerySettings.ShareImage = model.ShareImage;
            switch (model.SortByCreatedDate)
            {
                case "true":
                    _gallerySettings.SortByCreatedDate = true;
                    break;
                case "false":
                    _gallerySettings.SortByCreatedDate = false;
                    break;
            }

            switch (model.SliderGallery)
            {
                case "true":
                    _gallerySettings.SliderGallery = true;
                    break;
                case "false":
                    _gallerySettings.SliderGallery = false;
                    break;
            }
            _settingService.SaveSetting(_gallerySettings);
            return Configure();
        }

        [HttpGet]
        public ActionResult Configuration()
        {
            var galleries = _galleryService.GetAllGalleries();
            var model = new ConfigurationModel
            {
                WidgetZones = _gallerySettings.WidgetZones,
                GalleryWidgetZones = _gallerySettings.GalleryWidgetZones,
                ShowGallery = _gallerySettings.ShowGalleries,
                ShowImageAfterGalleries = _gallerySettings.ShowImageAfterGalleries,
                ProductPerPage = _gallerySettings.ItemPerPage,
                ProductPerWidgetPage = _gallerySettings.ItemPerWidgetPage,
                SortByCreatedDate = _gallerySettings.SortByCreatedDate ? "true" : "false",
                SliderGallery = _gallerySettings.SliderGallery ? "true" : "false",
                GalleryWidgetName = _gallerySettings.GalleryWidgetName,
                ShareImage = _gallerySettings.ShareImage
            };
            model.GalleryNames.Add(new SelectListItem { Text = "*", Value = "-1" });
            if (galleries.Count > 0)
            {
                foreach (var gallery in galleries)
                {
                    model.GalleryNames.Add(new SelectListItem { Text = gallery.Name, Value = gallery.Id.ToString() });
                }
            }
            else
            {
                model.GalleryNames.Add(new SelectListItem { Text = _localizationService.GetResource("Plugin.Widgets.Gallery.AddGallery"), Value = "true" });

            }
            model.GalleryTypes.Add(new SelectListItem { Text = _localizationService.GetResource("Plugin.Widgets.Gallery.Slider"), Value = "true" });
            model.GalleryTypes.Add(new SelectListItem { Text = _localizationService.GetResource("Plugin.Widgets.Gallery.ImagesList"), Value = "false" });
            model.SortedFilters.Add(new SelectListItem { Text = _localizationService.GetResource("Plugin.Widgets.Gallery.SortByDate"), Value = "true" });
            model.SortedFilters.Add(new SelectListItem { Text = _localizationService.GetResource("Plugin.Widgets.Gallery.SortByDisplayOrder"), Value = "false" });
            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/ConfigureWithArea.cshtml", model);
        }

        [HttpPost]
        public ActionResult Configuration(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configuration();

            _gallerySettings.WidgetZones = model.WidgetZones;
            _gallerySettings.GalleryWidgetZones = model.GalleryWidgetZones;
            _gallerySettings.ItemPerPage = model.ProductPerPage;
            _gallerySettings.ItemPerWidgetPage = model.ProductPerWidgetPage;
            _gallerySettings.ShowGalleries = model.ShowGallery;
            _gallerySettings.ShowImageAfterGalleries = model.ShowImageAfterGalleries;
            _gallerySettings.GalleryWidgetName = model.GalleryWidgetName;
            _gallerySettings.ShareImage = model.ShareImage;
            switch (model.SortByCreatedDate)
            {
                case "true":
                    _gallerySettings.SortByCreatedDate = true;
                    break;
                case "false":
                    _gallerySettings.SortByCreatedDate = false;
                    break;
            }

            switch (model.SliderGallery)
            {
                case "true":
                    _gallerySettings.SliderGallery = true;
                    break;
                case "false":
                    _gallerySettings.SliderGallery = false;
                    break;
            }

            _settingService.SaveSetting(_gallerySettings);
            return Configuration();
        }


        #region Galleries


        public ActionResult List()
        {
            var model = new ConfigurationGalleryListModel();
            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/List.cshtml", model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, ConfigurationGalleryListModel model)
        {
            var galleries = _galleryService.GetAllGalleries(model.SearchGalleryName,
                command.Page - 1, command.PageSize, true);
            var data = new List<ConfigurationGalleryModel>();
            foreach (var item in galleries)
            {
                data.Add(new ConfigurationGalleryModel
                {
                    Name = item.Name,
                    DisplayOrder = item.DisplayOrder,
                    Id = item.Id
                });
            }
            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = galleries.TotalCount
            };
            return Json(gridModel);
        }

        public ActionResult Create()
        {
            var model = new ConfigurationGalleryModel();
            model.GalleryType = "SimpleGallery";
            if (_gallerySettings.SliderGallery) model.SliderGallery = true;
            //locales
            AddLocales(_languageService, model.Locales);
            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Create(ConfigurationGalleryModel model)
        {
            if (ModelState.IsValid)
            {
                var gallery = new Galleries
                {
                    Name = model.Name,
                    Description = model.Description,
                    DisplayOrder = model.DisplayOrder,
                    PictureId = model.PictureId,
                    VideoLink = model.VideoLink,
                };

                if (!string.IsNullOrEmpty(model.GalleryType))
                {
                    switch (model.GalleryType)
                    {
                        case "SimpleGallery":
                            {
                                gallery.SimpleGallery = true;
                                gallery.FlickrGallery = false;
                                gallery.PicasaGallery = false;
                                gallery.VideoGallery = false;
                            }
                            break;
                        case "FlickrGallery":
                            {
                                gallery.FlickrGallery = true;
                                gallery.SimpleGallery = false;
                                gallery.PicasaGallery = false;
                                gallery.VideoGallery = false;
                            }
                            break;
                        case "PicasaGallery":
                            {
                                gallery.PicasaGallery = true;
                                gallery.SimpleGallery = false;
                                gallery.FlickrGallery = false;
                                gallery.VideoGallery = false;
                            }
                            break;
                        case "VideoGallery":
                            {
                                gallery.VideoGallery = true;
                                gallery.SimpleGallery = false;
                                gallery.FlickrGallery = false;
                                gallery.PicasaGallery = false;
                            }
                            break;
                    }
                }
                else
                {
                    gallery.SimpleGallery = true;
                    gallery.FlickrGallery = false;
                    gallery.PicasaGallery = false;
                    gallery.VideoGallery = false;
                }

                _galleryService.InsertGallery(gallery);
                //locales
                UpdateLocales(gallery, model);
                //update picture seo file name
                UpdatePictureSeoNames(gallery);

                SuccessNotification(_localizationService.GetResource("Plugin.Widgets.Gallery.GalleryConfigureController.Added"));
            }


            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/Create.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            var gallery = _galleryService.GetGalleryById(id);
            if (gallery == null)
                //No gallery found with the specified id
                return RedirectToAction("List");

            var model = gallery.ToModel();
            if (_gallerySettings.SliderGallery) model.SliderGallery = true;
            if (gallery.SimpleGallery) model.GalleryType = "SimpleGallery";
            if (gallery.FlickrGallery) model.GalleryType = "FlickrGallery";
            if (gallery.PicasaGallery) model.GalleryType = "PicasaGallery";
            if (gallery.VideoGallery) model.GalleryType = "VideoGallery";


            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = gallery.GetLocalized(x => x.Name, languageId, false, false);
                locale.Description = gallery.GetLocalized(x => x.Description, languageId, false, false);
            });
            if (gallery.VideoLink != null)
            {
                var videoLinks = gallery.VideoLink.Split(new[] { "%,%" }, StringSplitOptions.None).ToList();
                var i = 1;
                foreach (var vl in videoLinks)
                {
                    model.VideoLinks.Add(new SelectListItem { Text = vl, Value = i.ToString() });
                    i++;
                }
            }

            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Edit(ConfigurationGalleryModel model)
        {
            var gallery = _galleryService.GetGalleryById(model.Id);
            if (gallery == null)
                //No gallery found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                int prevPictureId = gallery.PictureId;

                gallery.Name = model.Name;
                gallery.Description = model.Description;
                gallery.DisplayOrder = model.DisplayOrder;
                gallery.PictureId = model.PictureId;
                gallery.VideoLink = model.VideoLink;


                if (!string.IsNullOrEmpty(model.GalleryType))
                {
                    switch (model.GalleryType)
                    {
                        case "SimpleGallery":
                            {
                                gallery.SimpleGallery = true;
                                gallery.FlickrGallery = false;
                                gallery.PicasaGallery = false;
                                gallery.VideoGallery = false;
                            }
                            break;
                        case "FlickrGallery":
                            {
                                gallery.FlickrGallery = true;
                                gallery.SimpleGallery = false;
                                gallery.PicasaGallery = false;
                                gallery.VideoGallery = false;
                            }
                            break;
                        case "PicasaGallery":
                            {
                                gallery.PicasaGallery = true;
                                gallery.SimpleGallery = false;
                                gallery.FlickrGallery = false;
                                gallery.VideoGallery = false;
                            }
                            break;
                        case "VideoGallery":
                            {
                                gallery.VideoGallery = true;
                                gallery.SimpleGallery = false;
                                gallery.FlickrGallery = false;
                                gallery.PicasaGallery = false;
                            }
                            break;
                    }
                }
                else
                {
                    gallery.SimpleGallery = true;
                    gallery.FlickrGallery = false;
                    gallery.PicasaGallery = false;
                    gallery.VideoGallery = false;
                }


                _galleryService.UpdateGallery(gallery);
                //locales
                UpdateLocales(gallery, model);
                //delete an old picture (if deleted or updated)
                if (prevPictureId > 0 && prevPictureId != gallery.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }
                //update picture seo file name
                UpdatePictureSeoNames(gallery);

                //activity log

                SuccessNotification(_localizationService.GetResource("Plugin.Widgets.Gallery.GalleryConfigureController.Updated"));
            }

            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var gallery = _galleryService.GetGalleryById(id);
            if (gallery == null)
                //No gallery found with the specified id
                return RedirectToAction("List");

            _galleryService.DeleteGallery(gallery);

            SuccessNotification(_localizationService.GetResource("Plugin.Widgets.Gallery.GalleryConfigureController.Deleted"));
            return RedirectToAction("List");
        }

        #endregion

        #region Gallery Images


        public ActionResult ImagesList()
        {
            var model = new ConfigurationGalleryImagesListModel();
            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/ImagesList.cshtml", model);
        }

        [HttpPost]
        public ActionResult ImagesList(DataSourceRequest command, ConfigurationGalleryImagesListModel model)
        {
            var galleries = _galleryService.GetAllGalleryImages(model.SearchGalleryName,
                command.Page - 1, command.PageSize, true);
            var data = new List<ConfigurationGalleryImagesModel>();
            foreach (var item in galleries)
            {
                data.Add(new ConfigurationGalleryImagesModel
                {
                    Name = item.Name,
                    DisplayOrder = item.DisplayOrder,
                    Id = item.Id
                });
            }
            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = galleries.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult CreateImage()
        {
            var model = new ConfigurationGalleryImagesModel();

            var galleries = _galleryService.GetAllGalleries();

            foreach (var gallery in galleries)
            {
                model.ListGalleries.Add(new SelectListItem { Text = gallery.Name, Value = gallery.Id.ToString(CultureInfo.InvariantCulture) });
            }

            //locales
            AddLocales(_languageService, model.Locales);
            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/CreateImage.cshtml", model);
        }


        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult CreateImage(ConfigurationGalleryImagesModel model)
        {
            if (ModelState.IsValid)
            {
                //var gallery = model.ToEntity();
                 var gallery = new GalleryImages
                {
                    Name = model.Name,
                    Description = model.Description,
                    DisplayOrder = model.DisplayOrder,
                    PictureId = model.PictureId,
                    UpdatedOnUtc  = DateTime.UtcNow,
                    CreatedOnUtc = DateTime.UtcNow,
                };

                if (model.SelectedOptions != null)
                {
                    foreach (var option in model.SelectedOptions)
                    {
                        var gal = _galleryService.GetGalleryById(int.Parse(option));

                        if (gallery.Galleries.Count(g => g.Id == gallery.Id) == 0)
                            gallery.Galleries.Add(gal);
                    }
                }
                _galleryService.InsertGalleryImage(gallery);

                //locales
                UpdateImageLocales(gallery, model);
                //update picture seo file name
                UpdateImagePictureSeoNames(gallery);

                SuccessNotification(_localizationService.GetResource("Plugin.Widgets.Gallery.GalleryConfigureController.ImageAdded"));
            }
            var galleries = _galleryService.GetAllGalleries();

            foreach (var gallery in galleries)
            {
                model.ListGalleries.Add(new SelectListItem { Text = gallery.Name, Value = gallery.Id.ToString(CultureInfo.InvariantCulture) });
            }

            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/CreateImage.cshtml", model);
        }

        public ActionResult EditImage(int id)
        {
            var gallery = _galleryService.GetGalleryImageById(id);
            if (gallery == null)
                //No gallery found with the specified id
                return RedirectToAction("ImagesList");

            var galleries = _galleryService.GetAllGalleries();

            var model = gallery.ToModel();

            model.ListGalleries.Add(new SelectListItem { Text = "Remove from all galleries", Value = "-1" });
            foreach (var g in galleries)
            {
                if (gallery.Galleries.Contains(g))
                {
                    model.ListGalleries.Add(new SelectListItem
                        {
                            Text = g.Name,
                            Value = g.Id.ToString(CultureInfo.InvariantCulture),
                            Selected = true
                        });
                }
                else
                {
                    model.ListGalleries.Add(new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString(CultureInfo.InvariantCulture),
                        Selected = false
                    });
                }
            }

            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = gallery.GetLocalized(x => x.Name, languageId, false, false);
                locale.Description = gallery.GetLocalized(x => x.Description, languageId, false, false);
            });


            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/EditImage.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult EditImage(ConfigurationGalleryImagesModel model)
        {
            var gallery = _galleryService.GetGalleryImageById(model.Id);
            if (gallery == null)
                //No gallery found with the specified id
                return RedirectToAction("ImagesList");

            var galleries = _galleryService.GetAllGalleries();
            model.ListGalleries.Add(new SelectListItem { Text = "Remove from all galleries", Value = "-1" });
            foreach (var g in galleries)
            {
                if (gallery.Galleries.Contains(g))
                {
                    model.ListGalleries.Add(new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString(CultureInfo.InvariantCulture),
                        Selected = true
                    });
                }
                else
                {
                    model.ListGalleries.Add(new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString(CultureInfo.InvariantCulture),
                        Selected = false
                    });
                }
            }


            if (ModelState.IsValid)
            {
                int prevPictureId = gallery.PictureId;
                gallery = model.ToEntity(gallery);

                gallery.Galleries.Clear();
                if (model.SelectedOptions != null)
                {
                    if (!model.SelectedOptions.Contains("-1"))
                    {
                        foreach (var option in model.SelectedOptions)
                        {
                            var gal = _galleryService.GetGalleryById(int.Parse(option));
                            gallery.Galleries.Add(gal);
                        }
                    }
                }
                gallery.UpdatedOnUtc = DateTime.UtcNow;
                _galleryService.UpdateGalleryImage(gallery);
                //locales
                UpdateImageLocales(gallery, model);
                //delete an old picture (if deleted or updated)
                if (prevPictureId > 0 && prevPictureId != gallery.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }
                //update picture seo file name
                UpdateImagePictureSeoNames(gallery);

                //activity log

                SuccessNotification(_localizationService.GetResource("Plugin.Widgets.Gallery.GalleryConfigureController.ImageUpdated"));
            }

            return View("~/Plugins/Widgets.Gallery/Views/GalleryConfigure/EditImage.cshtml", model);
        }

        [HttpPost]
        public ActionResult DeleteImage(int id)
        {
            var gallery = _galleryService.GetGalleryImageById(id);
            if (gallery == null)
                //No gallery found with the specified id
                return RedirectToAction("ImagesList");

            _galleryService.DeleteGalleryImage(gallery);

            SuccessNotification(_localizationService.GetResource("Plugin.Widgets.Gallery.GalleryConfigureController.ImageDeleted"));
            return RedirectToAction("ImagesList");
        }

        #endregion
    }
}
