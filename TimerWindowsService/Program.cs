using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Reflection;


namespace TimerWindowsService
{
    static class Program
    {
        protected string templetFile = null;
         protected string outputFile = null;
         protected object missing = Missing.Value;
 
         /// <summary>
         /// 构造函数，需指定模板文件和输出文件完整路径
         /// </summary>
         /// <param name="templetFilePath">Excel模板文件路径</param>
         /// <param name="outputFilePath">输出Excel文件路径</param>
         public ExcelHelper1(string templetFilePath,string outputFilePath)
         {
             if(templetFilePath == null)
                 throw new Exception("Excel模板文件路径不能为空！");
 
             if(outputFilePath == null)
                 throw new Exception("输出Excel文件路径不能为空！");
 
             if(!File.Exists(templetFilePath))
                 throw new Exception("指定路径的Excel模板文件不存在！");
 
             this.templetFile = templetFilePath;
             this.outputFile = outputFilePath;
 
         }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new MainService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
