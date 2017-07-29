using Microsoft.AspNetCore.Razor.TagHelpers;
using Orbital.Models;

namespace Orbital.Web.TagHelpers
{
    [HtmlTargetElement("length")]
    public class LengthTagHelper : TagHelper
    {
        public Length Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            
            output.Content.SetContent($"{Value.Value} {Value.Unit}");

            base.Process(context, output);
        }
    }
}
