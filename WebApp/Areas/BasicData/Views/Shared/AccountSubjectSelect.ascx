<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BusinessLogic.BasicData.Models.AccountSubject.SelectModel>" %>
<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%:Html.AppTreeViewFor(Model.PageId,Model.TreeId, Model.AccountSubjectTree,false)%>
