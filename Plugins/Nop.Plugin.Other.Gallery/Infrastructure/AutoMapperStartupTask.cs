using AutoMapper;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.Gallery.Domain;
using Nop.Plugin.Widgets.Gallery.Models;

namespace Nop.Plugin.Widgets.Gallery.Infrastructure
{
    public class AutoMapperStartupTask : IStartupTask
    {
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;

        public void Execute()
        {

            //Mapper.Initialize(main =>
            //{
            //    _mapperConfiguration = new MapperConfiguration(cfg =>
            //    {
            //        //cfg.CreateMap<Galleries, ConfigurationGalleryModel>()
            //        //    .ForMember(dest => dest.Locales, mo => mo.Ignore())
            //        //    .ForMember(dest => dest.VideoLinkPart, mo => mo.Ignore())
            //        //    .ForMember(dest => dest.GalleryType, mo => mo.Ignore())
            //        //    .ForMember(dest => dest.SliderGallery, mo => mo.Ignore())
            //        //    .ForMember(dest => dest.SelectedOptions, mo => mo.Ignore())
            //        //    .ForMember(dest => dest.VideoLink, mo => mo.Ignore())
            //        //    .ForMember(dest => dest.VideoLinks, mo => mo.Ignore())
            //        //   .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
            //        //cfg.CreateMap<ConfigurationGalleryModel, Galleries>();
            //        //cfg.CreateMap<Galleries, PublicInfoGalleryModel>();
            //        //cfg.CreateMap<PublicInfoGalleryModel, Galleries>();
            //        //cfg.CreateMap<GalleryImages, ConfigurationGalleryImagesModel>()
            //        //   .ForMember(dest => dest.Locales, mo => mo.Ignore())
            //        //   .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
            //        //cfg.CreateMap<ConfigurationGalleryImagesModel, GalleryImages>();
            //    });
            //    _mapperConfiguration.AssertConfigurationIsValid();
            //});
        }

        public int Order
        {
            get { return 0; }
        }
    }
}