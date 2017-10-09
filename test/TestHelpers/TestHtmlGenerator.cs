using System.Collections.Generic;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using Moq;

namespace TestHelpers
{
    public class TestHtmlGenerator : DefaultHtmlGenerator
    {
        public TestHtmlGenerator(
            IModelMetadataProvider metadataProvider,
            IOptions<MvcViewOptions> mvcViewOptions,
            IUrlHelperFactory urlHelperFactory)
                : base(Mock.Of<IAntiforgery>(), mvcViewOptions, metadataProvider, urlHelperFactory, new HtmlTestEncoder(), new ClientValidatorCache(), new DefaultValidationHtmlAttributeProvider(mvcViewOptions, metadataProvider, new ClientValidatorCache()))
        {

        }
        public override IHtmlContent GenerateAntiforgery(ViewContext viewContext)
        {
            return null;
        }
    }
}
