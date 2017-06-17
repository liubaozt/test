<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BusinessLogic.BasicData.Models.AssetsClass.SelectModel>" %>
<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="WebCommon.Data" %>
<%:Html.AppTreeViewFor(Model.PageId,TreeId.AssetsClassTreeId, Model.AssetsClassTree,false)%>
