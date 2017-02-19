using FluentValidation;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.Gallery.Models
{
    public class ConfigurationGalleryValidator : AbstractValidator<ConfigurationGalleryModel>
    {
        public ConfigurationGalleryValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(localizationService.GetResource("Plugin.Widgets.Gallery.ConfigurationGalleryModel.Name.Required"));
        }
    }
}
