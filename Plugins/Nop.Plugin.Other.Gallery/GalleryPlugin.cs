using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Nop.Core.Plugins;
using Nop.Plugin.Widgets.Gallery.Data;
using Nop.Services.Cms;
using System.Web.Routing;
using Nop.Services.Configuration;
using Nop.Web.Framework;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.Gallery
{

    public class GalleryPlugin : BasePlugin, IWidgetPlugin //, IAdminMenuPlugin
    {
        private readonly GalleryObjectContext _context;
        private readonly ISettingService _settingService;
        private readonly GallerySettings _gallerySettings;
        private readonly ILocalizationService _localizationService;

        public GalleryPlugin(GalleryObjectContext context, GallerySettings gallerySettings,ISettingService settingService,
            ILocalizationService localizationService)
        {
            _context = context;
            _gallerySettings = gallerySettings;
            _settingService = settingService;
            _localizationService = localizationService;
        }

        public override void Install()
        {
            //settings
            var settings = new GallerySettings
                {
                    WidgetZones = "header_menu_after",
                    ItemPerPage = 6
                };
            _settingService.SaveSetting(settings);
            _context.Install();

            //string to = "info@dev-partner.biz";
            //string from = "support@tenderoff.ru";

            //var message = new MailMessage(from, to)
            //{
            //    Subject = "Nop.Plugin.Widgets.Gallery",
            //    Body = String.Format("Url: {0} ", System.Web.HttpContext.Current.Request.Url)
            //};

            //var client = new SmtpClient("pod51015.outlook.com", 587)
            //{
            //    Credentials = new NetworkCredential("support@tenderoff.ru", "gx3ZLU94FKi"),
            //    EnableSsl = true
            //};
            //try
            //{
            //    client.Send(message);
            //}
            //catch (Exception ex)
            //{

            //}

            base.Install();
        }

        public override void Uninstall()
        {
             //settings
            _settingService.DeleteSetting<GallerySettings>();
            base.Uninstall();
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "GalleryConfigure";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.Gallery.Controllers" }, { "area", null } };
        }

        public IList<string> GetWidgetZones()
        {
            var list = string.IsNullOrEmpty(_gallerySettings.GalleryWidgetZones) ? (IList<string>)new List<string>() : _gallerySettings.GalleryWidgetZones.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (!String.IsNullOrEmpty(_gallerySettings.WidgetZones))
            {
                var widgetZones = _gallerySettings.WidgetZones.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var widgetZone in widgetZones)
                {
                    list.Add(widgetZone);
                }
                //list.InsertRange(_gallerySettings.WidgetZones.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
            return list;
        }

        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            if (_gallerySettings.WidgetZones.Contains(widgetZone))
            {
                actionName = "PublicInfo";
                controllerName = "GalleryPublicInfo";
                routeValues = new RouteValueDictionary
                {
                    {"Namespaces", "Nop.Plugin.Widgets.ProductReview.Controllers"},
                    {"area", null},
                    {"widgetZone", widgetZone}
                };
            }
            else if (_gallerySettings.GalleryWidgetZones.Contains(widgetZone))
            {
                actionName = "WidgetGallery";
                controllerName = "GalleryPublicInfo";
                routeValues = new RouteValueDictionary
                {
                    {"Namespaces", "Nop.Plugin.Widgets.ProductReview.Controllers"},
                    {"area", null},
                    {"widgetZone", widgetZone}
                };
            }
            else
            {
                actionName = "PublicInfo";
                controllerName = "GalleryPublicInfo";
                routeValues = new RouteValueDictionary
                {
                    {"Namespaces", "Nop.Plugin.Widgets.ProductReview.Controllers"},
                    {"area", null},
                    {"widgetZone", "header_menu_after"}
                };
            }
        }

        public bool Authenticate()
        {
            return true;
        }

        //public void BuildMenuItem(MenuItemBuilder menuItemBuilder)
        //{
        //    string actionName, controllerName;
        //    RouteValueDictionary routeValues;
        //    this.GetConfigurationRoute(out actionName, out controllerName, out routeValues);

        //    menuItemBuilder.Text(_localizationService.GetResource("Plugins.Admin.Gallery"));
        //    menuItemBuilder.Url("/Plugins/Gallery/Configuration");
        //}
    }

}
