using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Widgets.Gallery
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Widgets.Gallery.List",
                 "Plugins/GalleryConfigure/List",
                 new { controller = "GalleryConfigure", action = "List", area = "admin" },
                 new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );
            routes.MapRoute("Plugin.Widgets.GalleryList",
                 "GalleryConfigure/List",
                 new { controller = "GalleryConfigure", action = "List", area = "admin" },
                 new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );
            routes.MapRoute("Plugin.Widgets.Gallery.Configure",
                 "Plugins/GalleryConfigure/Configure",
                 new { controller = "GalleryConfigure", action = "Configure" },
                 new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );
            routes.MapRoute("Plugin.Widgets.Gallery.Configuration",
                 "Plugins/Gallery/Configuration",
                 new { controller = "GalleryConfigure", action = "Configuration", area = "admin" },
                 new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );
            routes.MapRoute("GalleryPlugin",
               "Gallery",
               new { controller = "GalleryPublicInfo", action = "List" },
               new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
          );

            routes.MapRoute("ImagesByGalleryId",
               "ImageGallery/{id}",
               new { controller = "GalleryPublicInfo", action = "Gallery" },
               new { productId = @"\d+" },
               new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );

            routes.MapRoute("ImageById",
                   "Gallery/SingleImage/{id}",
                   new { controller = "GalleryPublicInfo", action = "SingleImage" },
                   new { productId = @"\d+" },
                   new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
                );

            routes.MapRoute("Plugin.Widgets.Gallery.Edit",
                 "GalleryConfigure/Edit",
                 new { controller = "GalleryConfigure", action = "Edit", area = "admin" },
                 new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.Gallery.Create",
                 "GalleryConfigure/Create",
                 new { controller = "GalleryConfigure", action = "Create", area = "admin" },
                 new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );




            routes.MapRoute("Plugin.Widgets.Gallery.ImagesList",
                 "Plugins/GalleryConfigure/ImagesList",
                 new { controller = "GalleryConfigure", action = "ImagesList", area = "admin" },
                 new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );


            routes.MapRoute("Plugin.Widgets.Gallery.CreateImage",
                 "Plugins/GalleryConfigure/CreateImage",
                 new { controller = "GalleryConfigure", action = "CreateImage", area = "admin" },
                 new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );


            routes.MapRoute("Plugin.Widgets.Gallery.EditImage",
                 "Plugins/GalleryConfigure/EditImage",
                 new { controller = "GalleryConfigure", action = "EditImage", area = "admin" },
                 new[] { "Nop.Plugin.Widgets.Gallery.Controllers" }
            );



        }
        public int Priority
        {
            get { return 0; }
        }

    }
}
