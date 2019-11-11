using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using TzCA.Common.JsonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.CustomMvcCoreExtension
{
    public static class PageCommonItemHelper
    {
        public static HtmlString NncqSetListPageParameter(this IHtmlHelper helper, ListPageParameter pageParameter)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<input type='hidden' name='对应的类型' id='nncqTypeId' value='" + pageParameter.ObjectTypeId + "' />");
            htmlContent.Append(
                "<input type='hidden' name='当前页码' id='nncqPageIndex' value='" + pageParameter.PageIndex + "' />");
            htmlContent.Append(
                "<input type='hidden' name='每页数据条数' id='nncqPageSize' value='" + pageParameter.PageSize +
                    "' /> ");
            htmlContent.Append(
                "<input type='hidden' name='分页数量' id='nncqPageAmount' value='" + pageParameter.PageAmount +
                    "' />");
            htmlContent.Append(
                "<input type='hidden' name='相关的对象的总数' id='nncqObjectAmount' value='" + pageParameter.ObjectAmount +
                    "' />");
            htmlContent.Append(
                "<input type='hidden' name='当前的检索关键词' id='nncqKeyword' value='" + pageParameter.Keyword + "' />");
            htmlContent.Append(
                "<input type='hidden' name='排序属性' id='nncqSortProperty' value='" + pageParameter.SortProperty +
                    "' /> ");
            htmlContent.Append(
                "<input type='hidden' name='排序方向' id='nncqSortDesc' value='" + pageParameter.SortDesc + "' />");
            htmlContent.Append(
                "<input type='hidden' name='当前焦点对象 Id' id='nncqSelectedObjectId' value='" +
                    pageParameter.SelectedObjectId + "' />");
            htmlContent.Append(
                "<input type='hidden' name='当前是否为检索' id='nncqIsSearch' value='" +
                    pageParameter.IsSearch + "' />");
            return new HtmlString(htmlContent.ToString());

        }
    }
}
