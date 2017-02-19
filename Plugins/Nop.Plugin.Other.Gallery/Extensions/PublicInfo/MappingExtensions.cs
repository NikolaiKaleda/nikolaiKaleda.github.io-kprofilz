using Nop.Plugin.Widgets.Gallery.Domain;
using Nop.Plugin.Widgets.Gallery.Models;
using AutoMapper;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.Gallery.Extensions.PublicInfo
{
    public static class MappingExtensions
    {

        public static PublicInfoGalleryModel ToModel(this Galleries entity)
        {
            if (entity == null)
                return null;

            var model = new PublicInfoGalleryModel
            {
                Id = entity.Id,
                GalleryName = entity.GetLocalized(x => x.Name),
                GalleryDescription = entity.GetLocalized(x => x.Description),
            };
            return model;
        }

        public static Galleries ToEntity(this PublicInfoGalleryModel model)
        {
            return Mapper.Map<PublicInfoGalleryModel, Galleries>(model);
        }

        public static Galleries ToEntity(this PublicInfoGalleryModel model, Galleries destination)
        {
            return Mapper.Map(model, destination);
        }

    }
}
