using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Report;
using BusinessLogic.Report.Repositorys;

namespace WebApp.WebForms
{
    public partial class AssetsClassChangeReportView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ////创建报表文档   
            //ReportDocument myReport = new ReportDocument();
            ////取到报表文件物理路径   
            //string reportPath = Server.MapPath("~/bin/AssetsClassChangeReport.rpt");
            ////加载报表文件   
            //myReport.Load(reportPath);
            //string formvar = Request.QueryString["formVar"];
            //AssetsChangeReportRepository repository = new AssetsChangeReportRepository();
            ////AssetsClassChangeReportDS ds = repository.GetReportSource(formvar);
            //AssetsClassChangeReportDS ds = new AssetsClassChangeReportDS();
            ////为新的报表文档设置数据源   
            //myReport.SetDataSource(ds);
            ////将创建的新的报表文档绑定   
            //this.CrystalReportViewer1.ReportSource = myReport;
            //this.CrystalReportViewer1.DataBind();
        }
    }
}