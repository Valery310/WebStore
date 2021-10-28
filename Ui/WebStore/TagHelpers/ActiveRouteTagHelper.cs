using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper: TagHelper
    {
        private IDictionary<string, string> _routeValue;

        /// <summary>The name of the action method.</summary>
        /// <remarks>Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-<c>null</c>.</remarks>
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        /// <summary>The name of the controller.</summary>
        /// <remarks>Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-<c>null</c>.</remarks>
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        /// <summary>Additional parameters for the route.</summary>
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get => _routeValue ?? (this._routeValue = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            set => _routeValue = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.Rendering.ViewContext" /> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            bool ignoreAction = context.AllAttributes.TryGetAttribute("ignore-action", out _);


            if (ShouldBeActive(ignoreAction))
            {
                MakeActive(output);
            }

            output.Attributes.RemoveAll("is-active-route");
        }

        private bool ShouldBeActive(bool ignoreAction) 
        {
            var currentController = ViewContext.RouteData.Values["Controller"].ToString();
            var currentAction = ViewContext.RouteData.Values["Action"].ToString();
            var route_values = ViewContext.RouteData.Values;

            if (Controller is { Length: > 0 } controller && !string.Equals(controller, currentController))
            {
                return false;
            }

            if (Action is { Length: > 0 } action && !string.Equals(action, currentAction))
            {
                return false;
            }

            foreach (var (key, value) in RouteValues)
            {
                if (!route_values.ContainsKey(key) || route_values[key]?.ToString() != value)
                {
                    return false;
                }
            }

            return true;
        }

        private void MakeActive(TagHelperOutput output) 
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");

            if (classAttr is null)
            {
                output.Attributes.Add("class", "active");
            }
            else 
            {
                if (classAttr.Value?.ToString()?.Contains("active") ?? false)
                    return;

                output.Attributes.SetAttribute("class", classAttr.Value == null ? "active" : classAttr.Value + " active");
            }
        }
    }
}
