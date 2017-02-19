using Nop.Plugin.Widgets.Gallery.Domain;
using Nop.Plugin.Widgets.Gallery.Models;
using AutoMapper;

namespace Nop.Plugin.Widgets.Gallery.Extensions.Configure
{
    public static class MappingExtensions
    {
        public static ConfigurationGalleryModel ToModel(this Galleries entity)
        {
            return Mapper.Map<Galleries, ConfigurationGalleryModel>(entity);
        }

        public static Galleries ToEntity(this ConfigurationGalleryModel model)
        {
            return Mapper.Map<ConfigurationGalleryModel, Galleries>(model);
        }

        public static Galleries ToEntity(this ConfigurationGalleryModel model, Galleries destination)
        {
            return Mapper.Map(model, destination);
        }

        public static ConfigurationGalleryImagesModel ToModel(this GalleryImages entity)
        {
            return Mapper.Map<GalleryImages, ConfigurationGalleryImagesModel>(entity);
        }

        public static GalleryImages ToEntity(this ConfigurationGalleryImagesModel model)
        {
            return Mapper.Map<ConfigurationGalleryImagesModel, GalleryImages>(model);
        }

        public static GalleryImages ToEntity(this ConfigurationGalleryImagesModel model, GalleryImages destination)
        {
            return Mapper.Map(model, destination);
        }
    }
}
