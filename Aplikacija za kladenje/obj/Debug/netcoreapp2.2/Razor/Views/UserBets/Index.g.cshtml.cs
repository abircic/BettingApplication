#pragma checksum "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "67db38a6b65d3e98e4e05acd9b92dd4781c3e571"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_UserBets_Index), @"mvc.1.0.view", @"/Views/UserBets/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/UserBets/Index.cshtml", typeof(AspNetCore.Views_UserBets_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\_ViewImports.cshtml"
using Aplikacija_za_kladenje;

#line default
#line hidden
#line 2 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\_ViewImports.cshtml"
using Aplikacija_za_kladenje.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"67db38a6b65d3e98e4e05acd9b92dd4781c3e571", @"/Views/UserBets/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f94274d7b13490bb6fd81531f10b10f620f693cd", @"/Views/_ViewImports.cshtml")]
    public class Views_UserBets_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Aplikacija_za_kladenje.Models.UserBets>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(60, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(150, 106, true);
            WriteLiteral("\r\n<h1>Index</h1>\r\n\r\n\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(257, 45, false);
#line 15 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.TimeStamp));

#line default
#line hidden
            EndContext();
            BeginContext(302, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(358, 45, false);
#line 18 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.BetAmount));

#line default
#line hidden
            EndContext();
            BeginContext(403, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(459, 44, false);
#line 21 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.TotalOdd));

#line default
#line hidden
            EndContext();
            BeginContext(503, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(559, 43, false);
#line 24 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.CashOut));

#line default
#line hidden
            EndContext();
            BeginContext(602, 86, true);
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
            EndContext();
#line 30 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
 foreach (var item in Model) {

#line default
#line hidden
            BeginContext(720, 48, true);
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(769, 44, false);
#line 33 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.TimeStamp));

#line default
#line hidden
            EndContext();
            BeginContext(813, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(869, 44, false);
#line 36 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.BetAmount));

#line default
#line hidden
            EndContext();
            BeginContext(913, 59, true);
            WriteLiteral(" kn \r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(973, 43, false);
#line 39 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.TotalOdd));

#line default
#line hidden
            EndContext();
            BeginContext(1016, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(1072, 42, false);
#line 42 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.CashOut));

#line default
#line hidden
            EndContext();
            BeginContext(1114, 58, true);
            WriteLiteral(" kn\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(1172, 59, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "67db38a6b65d3e98e4e05acd9b92dd4781c3e5718294", async() => {
                BeginContext(1220, 7, true);
                WriteLiteral("Details");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 45 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
                                          WriteLiteral(item.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1231, 37, true);
            WriteLiteral(" \r\n            </td>\r\n        </tr>\r\n");
            EndContext();
#line 48 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\UserBets\Index.cshtml"
}

#line default
#line hidden
            BeginContext(1271, 24, true);
            WriteLiteral("    </tbody>\r\n</table>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Aplikacija_za_kladenje.Models.UserBets>> Html { get; private set; }
    }
}
#pragma warning restore 1591
