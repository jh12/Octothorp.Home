using Markdig;
using Microsoft.AspNetCore.Html;

namespace Octothorp.Home.Helpers
{
    public static class MarkdownHelper
    {
        private static readonly MarkdownPipeline Pipeline;

        static MarkdownHelper()
        {
            Pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
        }

        public static string ToRawHtml(string markdown)
        {
            return Markdown.ToHtml(markdown, Pipeline);
        }

        public static HtmlString ToHtml(string markdown)
        {
            return new HtmlString(ToRawHtml(markdown));
        }
    }
}