using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Common
{
    public partial class SocialModel : BaseNopModel
    {
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string YoutubeLink { get; set; }
        public string GooglePlusLink { get; set; }
        public int WorkingLanguageId { get; set; }
        public bool NewsEnabled { get; set; }

        public string InstagramLink { get; set; }

        public string VkLink { get; set; }
        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Skype { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

    }
}