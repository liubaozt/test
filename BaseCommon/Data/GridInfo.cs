using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCommon.Data
{
    public  class GridInfo
    {
        public string Caption{ get;set;}
        /// <summary>
        /// 数据源
        /// </summary>
        public string Datasourceurl { get; set; }
        
        /// <summary>
        /// 格式化
        /// </summary>
        public string Formatter { get; set; }
       
        /// <summary>
        /// 编辑类型，即显示的是textbox，还是dropdown等
        /// </summary>
        public string Edittype{ get;set;}
       
        /// <summary>
        /// 能不能编辑
        /// </summary>
        public string Editable{ get;set;}
        
        /// <summary>
        /// 名称，即列名
        /// </summary>
        public string Name{ get;set;}
       
        /// <summary>
        /// 文本对齐方式
        /// </summary>
        public string Align{ get;set;}
        
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public string Hidden{ get;set;}
        
        /// <summary>
        /// 宽度，即列的宽度
        /// </summary>
        public string Width{ get;set;}
        
        /// <summary>
        /// 列的Index
        /// </summary>
        public string Index{ get;set;}

        /// <summary>
        /// 被合并的行
        /// </summary>
        public string IsRowspan { get; set; }
        
    }
}
