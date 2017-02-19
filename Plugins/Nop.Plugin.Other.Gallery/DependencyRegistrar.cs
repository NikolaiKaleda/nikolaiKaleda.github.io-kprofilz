using System.Data.Entity;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Widgets.Gallery.Data;
using Nop.Plugin.Widgets.Gallery.Domain;
using Nop.Plugin.Widgets.Gallery.Services;


namespace Nop.Plugin.Widgets.Gallery
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Registers the I db context.
        /// </summary>
        /// <param name="componentContext">The component context.</param>
        /// <param name="dataSettings">The data settings.</param>
        /// <returns></returns>
        private GalleryObjectContext RegisterIDbContext(IComponentContext componentContext, DataSettings dataSettings)
        {
            string dataConnectionStrings;

            if (dataSettings != null && dataSettings.IsValid())
            {
                dataConnectionStrings = dataSettings.DataConnectionString;
            }
            else
            {
                dataConnectionStrings = componentContext.Resolve<DataSettings>().DataConnectionString;
            }

            return new GalleryObjectContext(dataConnectionStrings);
        }


        public int Order
        {
            get { return 1; }
        }


        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, Core.Configuration.NopConfig config)
        {
            builder.RegisterType<GalleryService>().As<IGalleryService>().InstancePerHttpRequest();
            Database.SetInitializer<GalleryObjectContext>(null);
            //data layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                //register named context
                builder.Register<IDbContext>(c => new GalleryObjectContext(dataProviderSettings.DataConnectionString))
                    .Named<IDbContext>("nop_object_context_gallery")
                    .InstancePerHttpRequest();

                builder.Register<GalleryObjectContext>(c => new GalleryObjectContext(dataProviderSettings.DataConnectionString))
                    .InstancePerHttpRequest();
            }
            else
            {
                //register named context
                builder.Register<IDbContext>(c => new GalleryObjectContext(c.Resolve<DataSettings>().DataConnectionString))
                    .Named<IDbContext>("nop_object_context_gallery")
                    .InstancePerHttpRequest();

                builder.Register<GalleryObjectContext>(c => new GalleryObjectContext(c.Resolve<DataSettings>().DataConnectionString))
                    .InstancePerHttpRequest();
            }

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Galleries>>()
                .As<IRepository<Galleries>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_gallery"))
                .InstancePerHttpRequest();

            builder.RegisterType<EfRepository<GalleryImages>>()
                .As<IRepository<GalleryImages>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_gallery"))
                .InstancePerHttpRequest();
        }
    }
}
