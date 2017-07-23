using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Orbital.Models;

namespace Orbital.Web.Users
{
    [HtmlTargetElement("user")]
    public class UserTagHelper : TagHelper
    {
        private readonly IUrlHelper _urlHelper;

        public User User { get; set; }

        public UserTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
            : this(urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext))
        {
        }
        internal UserTagHelper(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.SetAttribute("href", _urlHelper.Action("Get", "User", new { User.Id }));
            output.Attributes.Add("class", "text-muted");
            output.Content.SetContent(User.Name);
        }
    }
}
