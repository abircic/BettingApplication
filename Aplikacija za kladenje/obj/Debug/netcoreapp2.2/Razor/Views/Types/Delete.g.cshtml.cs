#pragma checksum "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e474472a8a487af4a552672b29e0e50181fe05ce"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Types_Delete), @"mvc.1.0.view", @"/Views/Types/Delete.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Types/Delete.cshtml", typeof(AspNetCore.Views_Types_Delete))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e474472a8a487af4a552672b29e0e50181fe05ce", @"/Views/Types/Delete.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f94274d7b13490bb6fd81531f10b10f620f693cd", @"/Views/_ViewImports.cshtml")]
    public class Views_Types_Delete : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Aplikacija_za_kladenje.Models.Types>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(44, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
  
    ViewData["Title"] = "Delete";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(135, 175, true);
            WriteLiteral("\r\n<h1>Delete</h1>\r\n\r\n<h3>Are you sure you want to delete this?</h3>\r\n<div>\r\n    <h4>Types</h4>\r\n    <hr />\r\n    <dl class=\"row\">\r\n        <dt class = \"col-sm-2\">\r\n            ");
            EndContext();
            BeginContext(311, 38, false);
#line 16 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model._1));

#line default
#line hidden
            EndContext();
            BeginContext(349, 63, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
            EndContext();
            BeginContext(413, 34, false);
#line 19 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayFor(model => model._1));

#line default
#line hidden
            EndContext();
            BeginContext(447, 62, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
            EndContext();
            BeginContext(510, 38, false);
#line 22 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model._X));

#line default
#line hidden
            EndContext();
            BeginContext(548, 63, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
            EndContext();
            BeginContext(612, 34, false);
#line 25 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayFor(model => model._X));

#line default
#line hidden
            EndContext();
            BeginContext(646, 62, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
            EndContext();
            BeginContext(709, 38, false);
#line 28 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model._2));

#line default
#line hidden
            EndContext();
            BeginContext(747, 63, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
            EndContext();
            BeginContext(811, 34, false);
#line 31 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayFor(model => model._2));

#line default
#line hidden
            EndContext();
            BeginContext(845, 62, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
            EndContext();
            BeginContext(908, 39, false);
#line 34 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model._1X));

#line default
#line hidden
            EndContext();
            BeginContext(947, 63, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
            EndContext();
            BeginContext(1011, 35, false);
#line 37 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayFor(model => model._1X));

#line default
#line hidden
            EndContext();
            BeginContext(1046, 62, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
            EndContext();
            BeginContext(1109, 39, false);
#line 40 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model._X2));

#line default
#line hidden
            EndContext();
            BeginContext(1148, 63, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
            EndContext();
            BeginContext(1212, 35, false);
#line 43 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayFor(model => model._X2));

#line default
#line hidden
            EndContext();
            BeginContext(1247, 62, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
            EndContext();
            BeginContext(1310, 39, false);
#line 46 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model._12));

#line default
#line hidden
            EndContext();
            BeginContext(1349, 63, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
            EndContext();
            BeginContext(1413, 35, false);
#line 49 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
       Write(Html.DisplayFor(model => model._12));

#line default
#line hidden
            EndContext();
            BeginContext(1448, 38, true);
            WriteLiteral("\r\n        </dd>\r\n    </dl>\r\n    \r\n    ");
            EndContext();
            BeginContext(1486, 206, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e474472a8a487af4a552672b29e0e50181fe05ce10743", async() => {
                BeginContext(1512, 10, true);
                WriteLiteral("\r\n        ");
                EndContext();
                BeginContext(1522, 36, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "e474472a8a487af4a552672b29e0e50181fe05ce11136", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 54 "C:\Users\Ante\Documents\Visual Studio 2017\Projects\Aplikacija za kladenje\Aplikacija za kladenje\Views\Types\Delete.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.Id);

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(1558, 83, true);
                WriteLiteral("\r\n        <input type=\"submit\" value=\"Delete\" class=\"btn btn-danger\" /> |\r\n        ");
                EndContext();
                BeginContext(1641, 38, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e474472a8a487af4a552672b29e0e50181fe05ce13086", async() => {
                    BeginContext(1663, 12, true);
                    WriteLiteral("Back to List");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(1679, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1692, 10, true);
            WriteLiteral("\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Aplikacija_za_kladenje.Models.Types> Html { get; private set; }
    }
}
#pragma warning restore 1591