<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Report.Master" AutoEventWireup="true"
    CodeBehind="AssetsClassChangeReportView.aspx.cs" Inherits="WebApp.WebForms.AssetsClassChangeReportView" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>资产分类变动表</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    </div>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="682px">
    </rsweb:ReportViewer>
</asp:Content>
