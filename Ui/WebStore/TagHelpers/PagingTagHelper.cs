using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebStore.Domain.ViewModel;

namespace WebStore.TagHelpers
{
    public class PagingTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _helperFactory;

        public PagingTagHelper(IUrlHelperFactory helperFactory)
        {
            _helperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageModel { get; set; }

        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _helperFactory.GetUrlHelper(ViewContext);
            var tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");
            for (var i = 1; i <= PageModel.TotalPages; i++)
            {
                var item = CreateTag(i, urlHelper);
                tag.InnerHtml.AppendHtml(item);
            }
            base.Process(context, output);
            output.Content.AppendHtml(tag);
        }

        private TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            var item = new TagBuilder("li");
            var link = new TagBuilder("a");
            if (pageNumber == PageModel.PageNumber)
            {
                link.MergeAttribute("data-page", PageModel.PageNumber.ToString());
                item.AddCssClass("active");
            }
            else
            {
                PageUrlValues["page"] = pageNumber;
                link.Attributes["href"] = "#";
                foreach (var pageUrlValue in PageUrlValues)
                {
                    if (pageUrlValue.Value != null)
                    {
                        link.MergeAttribute("data-" + pageUrlValue.Key, pageUrlValue.Value.ToString());
                    }
                }
            }
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}

