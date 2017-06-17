using BaseCommon.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace TestProject
{
    
    
    /// <summary>
    ///这是 ExcelHelper1Test 的测试类，旨在
    ///包含所有 ExcelHelper1Test 单元测试
    ///</summary>
    [TestClass()]
    public class ExcelHelper1Test
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///ExcelHelper1 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void ExcelHelper1ConstructorTest()
        {
            string templetFilePath = string.Empty; // TODO: 初始化为适当的值
            string outputFilePath = string.Empty; // TODO: 初始化为适当的值
            ExcelHelper1 target = new ExcelHelper1(templetFilePath, outputFilePath);
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///ArrayToExcel 的测试
        ///</summary>
        [TestMethod()]
        public void ArrayToExcelTest()
        {
            string templetFilePath = string.Empty; // TODO: 初始化为适当的值
            string outputFilePath = string.Empty; // TODO: 初始化为适当的值
            ExcelHelper1 target = new ExcelHelper1(templetFilePath, outputFilePath); // TODO: 初始化为适当的值
            string[,] arr = null; // TODO: 初始化为适当的值
            int rows = 0; // TODO: 初始化为适当的值
            int top = 0; // TODO: 初始化为适当的值
            int left = 0; // TODO: 初始化为适当的值
            string sheetPrefixName = string.Empty; // TODO: 初始化为适当的值
            target.ArrayToExcel(arr, rows, top, left, sheetPrefixName);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///DataTableToExcel 的测试
        ///</summary>
        [TestMethod()]
        public void DataTableToExcelTest()
        {
            ExcelHelper1 eh = new ExcelHelper1(@"d:\templet.xls", @"d:\out.xls");
            DataTable dt = new DataTable("table");
            dt.Columns.Add("col1", System.Type.GetType("System.String"));
            dt.Columns.Add("col2", System.Type.GetType("System.String"));
            dt.Columns.Add("col3", System.Type.GetType("System.String"));
            DataRow dr = dt.NewRow();
            dr["col1"] = "刘宝";
            dr["col2"] = "30";
            dr["col3"] = "男";
            dt.Rows.Add(dr);
            eh.DataTableToExcel(dt, 1, 2, 1, "test");
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///GetSheetCount 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BaseCommon.dll")]
        public void GetSheetCountTest()
        {
            PrivateObject param0 = null; // TODO: 初始化为适当的值
            ExcelHelper1_Accessor target = new ExcelHelper1_Accessor(param0); // TODO: 初始化为适当的值
            int rowCount = 0; // TODO: 初始化为适当的值
            int rows = 0; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.GetSheetCount(rowCount, rows);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
