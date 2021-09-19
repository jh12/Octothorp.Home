using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Octothorp.Home.Helpers.Tags
{
    [HtmlTargetElement("markdown")]
    public class MarkdownTagHelper : TagHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        [HtmlAttributeName("markdown")]
        public ModelExpression Markdown { get; set; }

        [HtmlAttributeName("file")]
        public string FilePath { get; set; }

        public MarkdownTagHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            string content = null;

            if (Markdown != null)
                content = Markdown.Model?.ToString();
            else if (!string.IsNullOrEmpty(FilePath))
            {
                string fullFilePath = Path.Combine(_webHostEnvironment.WebRootPath, FilePath);
                content = await File.ReadAllTextAsync(fullFilePath);
            }

            if (content == null)
                content = (await output.GetChildContentAsync(NullHtmlEncoder.Default))
                    .GetContent(NullHtmlEncoder.Default);

            if (string.IsNullOrEmpty(content))
                return;

            string markdown = content;

            HtmlString html = MarkdownHelper.ToHtml(markdown);

            output.TagName = null;
            output.Content.SetHtmlContent(html);
        }
    }
}