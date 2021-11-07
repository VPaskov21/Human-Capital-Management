#pragma checksum "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "990910dba9deafe8cd9eae102b2abdb04d7035e3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_HR_Finished_Absence_Requests), @"mvc.1.0.view", @"/Views/HR/Finished_Absence_Requests.cshtml")]
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
#nullable restore
#line 1 "D:\Visual Studio 2019\Projects\HCMApp\Views\_ViewImports.cshtml"
using HCMApp.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Visual Studio 2019\Projects\HCMApp\Views\_ViewImports.cshtml"
using HCMApp.Data.ViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"990910dba9deafe8cd9eae102b2abdb04d7035e3", @"/Views/HR/Finished_Absence_Requests.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2258d7bd1f768c2ccbf66e4e24d94a1dff61530f", @"/Views/_ViewImports.cshtml")]
    public class Views_HR_Finished_Absence_Requests : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<AbsenceRequestVM>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
  
    ViewData["Title"] = "Приключени заявки за отпуска";
    Layout = "~/Views/Shared/_LayoutHR.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"container my-3 d-flex flex-column justify-content-center align-items-center\">\r\n    <div class=\"d-flex justify-content-center align-items-center\">\r\n        <h3>Приключени заявки за отпуска</h3>\r\n    </div>\r\n    <hr class=\"w-75\">\r\n\r\n");
#nullable restore
#line 12 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
     if (Model.Count() > 0)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <div class=""container table-responsive"">
            <table class=""table"">
                <thead>
                    <tr>
                        <th scope=""col"">Име на служител</th>
                        <th scope=""col"">Дати за отпуска</th>
                        <th scope=""col"">Причина</th>
                        <th scope=""col"">Статус</th>
                    </tr>
                </thead>
                <tbody>
");
#nullable restore
#line 25 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
                     foreach (var absenceRequest in @Model)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr>\r\n                            <td class=\"w-25 align-middle\">");
#nullable restore
#line 28 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
                                                     Write(absenceRequest.EmployeeName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td class=\"w-25 align-middle\">");
#nullable restore
#line 29 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
                                                     Write(absenceRequest.FromDate.ToString("dd.MM.yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral(" - ");
#nullable restore
#line 29 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
                                                                                                       Write(absenceRequest.ToDate.ToString("dd.MM.yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td class=\"w-25 align-middle\">");
#nullable restore
#line 30 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
                                                     Write(absenceRequest.Reason);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td class=\"w-25 align-middle\">");
#nullable restore
#line 31 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
                                                     Write(absenceRequest.Status);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        </tr>\r\n");
#nullable restore
#line 33 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </tbody>\r\n            </table>\r\n        </div>\r\n");
#nullable restore
#line 37 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"container\">\r\n            <div class=\"d-flex justify-content-center align-items-center\">\r\n                <h3>Няма приключени заявки за отпуска.</h3>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 45 "D:\Visual Studio 2019\Projects\HCMApp\Views\HR\Finished_Absence_Requests.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<AbsenceRequestVM>> Html { get; private set; }
    }
}
#pragma warning restore 1591