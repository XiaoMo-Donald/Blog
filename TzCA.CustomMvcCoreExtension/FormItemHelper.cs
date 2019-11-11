using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TzCA.Common.ViewModelComponents.ForMvcHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.CustomMvcCoreExtension
{
    public static class FormItemHelper
    {
        /// <summary>
        /// 使用 BootStrap 框架简单的 Helper
        /// </summary>
        /// <param name="helper">扩展的实例</param>
        /// <param name="inputItemSpec">显示的名称</param>
        /// <returns>用于前端的 HtmlString </returns>
        public static HtmlString NncqBoorStrapInputSimple(this IHtmlHelper helper, HtmlInputItemSpecification inputItemSpec)
        {
            var itemDisplayName = inputItemSpec.ItemDisplayName;
            var itemId = inputItemSpec.ItemId;
            var itemVale = inputItemSpec.ItemValue;
            var placeholderString = inputItemSpec.PlaceholderString;

            var htmlContentBuilder = new StringBuilder();

            htmlContentBuilder.Append("<div class='form-group' id='"+ itemId + "_DIV'>");
            htmlContentBuilder.Append("<div class='col-sm-3'>");
            htmlContentBuilder.Append("<label for='" + itemId + "' class='pull-right control-label'>" + itemDisplayName+"：</label>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-sm-9'>");
            htmlContentBuilder.Append("<input type='text' class='form-control' " +
                "name='" + itemId + "' " +
                "id='" + itemId + "' " +
                "placeholder='" + placeholderString + "' " +
                "value='" + itemVale + "' " +
                "onfocus='' " +
                "onBlur='' " +
                "autofocus=''>");
            htmlContentBuilder.Append("<div id='" + itemId + "_Help'></div>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("</div>");
            return new HtmlString(htmlContentBuilder.ToString());
        }

        /// <summary>
        /// 使用 BootStrap 框架简单的 Helper
        /// </summary>
        /// <param name="helper">扩展的实例</param>
        /// <param name="inputItemSpec">显示的名称</param>
        /// <returns>用于前端的 HtmlString </returns>
        public static HtmlString NncqBoorStrapInputValidate(this IHtmlHelper helper, HtmlInputItemSpecification inputItemSpec)
        {
            var itemDisplayName   = inputItemSpec.ItemDisplayName;
            var itemId            = inputItemSpec.ItemId;
            var itemVale          = inputItemSpec.ItemValue;
            var placeholderString = inputItemSpec.PlaceholderString;
            var onfocusFuntion    = inputItemSpec.OnfocusFuntion;
            var onBlurFunction    = inputItemSpec.OnBlurFunction;
            var autofocusName     = inputItemSpec.AutofocusName;

            var htmlContentBuilder = new StringBuilder();

            htmlContentBuilder.Append("<div class='form-group' id='" + itemId + "_DIV'>");
            htmlContentBuilder.Append("<div class='col-sm-3'>");
            htmlContentBuilder.Append("<label for='" + itemId + "' class='pull-right  control-label'>" + itemDisplayName + "：</label>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-sm-9'>");

            htmlContentBuilder.Append("<input type='text' class='form-control' " +
                "name='" + itemId + "' " +
                "id='" + itemId + "' " +
                "placeholder='" + placeholderString + "' " +
                "value='" + itemVale + "' " +
                "onfocus='" + onfocusFuntion + "' " +
                "onBlur='" + onBlurFunction + "' " +
                "autofocus='" + autofocusName + "'>");

            htmlContentBuilder.Append("<div id='" + itemId + "_Help'></div>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("</div>");
            return new HtmlString(htmlContentBuilder.ToString());
        }

        /// <summary>
        /// 使用 BootStrap 框架简单的 Helper
        /// </summary>
        /// <param name="helper">扩展的实例</param>
        /// <param name="inputItemSpec">显示的名称</param>
        /// <returns>用于前端的 HtmlString </returns>
        public static HtmlString NncqBoorStrapInputPassword(this IHtmlHelper helper, HtmlInputItemSpecification inputItemSpec)
        {
            var itemDisplayName = inputItemSpec.ItemDisplayName;
            var itemId = inputItemSpec.ItemId;
            var itemVale = inputItemSpec.ItemValue;
            var placeholderString = inputItemSpec.PlaceholderString;
            var onfocusFuntion = inputItemSpec.OnfocusFuntion;
            var onBlurFunction = inputItemSpec.OnBlurFunction;
            var autofocusName = inputItemSpec.AutofocusName;

            var htmlContentBuilder = new StringBuilder();

            htmlContentBuilder.Append("<div class='form-group' id='" + itemId + "_DIV'>");
            htmlContentBuilder.Append("<div class='col-sm-3'>");
            htmlContentBuilder.Append("<label for='" + itemId + "' class='pull-right control-label'>" + itemDisplayName + "：</label>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-sm-9'>");

            htmlContentBuilder.Append("<input type='password' class='form-control' " +
                "name='" + itemId + "' " +
                "id='" + itemId + "' " +
                "placeholder='" + placeholderString + "' " +
                "value='" + itemVale + "' " +
                "onfocus='" + onfocusFuntion + "' " +
                "onBlur='" + onBlurFunction + "' " +
                "autofocus='" + autofocusName + "'>");

            htmlContentBuilder.Append("<div id='" + itemId + "_Help'></div>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("</div>");
            return new HtmlString(htmlContentBuilder.ToString());
        }

    }
}
