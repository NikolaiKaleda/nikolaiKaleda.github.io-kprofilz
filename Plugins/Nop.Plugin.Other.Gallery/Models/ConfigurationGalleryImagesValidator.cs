using FluentValidation;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.Gallery.Models
{
    public class ConfigurationGalleryImagesValidator : AbstractValidator<ConfigurationGalleryImagesModel>
    {
        public ConfigurationGalleryImagesValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(localizationService.GetResource("Plugin.Widgets.Gallery.ConfigurationGalleryImagesModel.Name.Required"));
        }
    }
}
