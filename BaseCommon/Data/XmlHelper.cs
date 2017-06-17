using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BaseCommon.Basic;
namespace BaseCommon.Data
{
    /// <summary>
    /// XML������
    /// </summary>
    public class XmlHelper
    {
        /// <summary>   
        /// ��ȡ����   
        /// </summary>   
        /// <param name="path">·��</param>   
        /// <param name="node">�ڵ�</param>   
        /// <param name="attribute">���������ǿ�ʱ���ظ�����ֵ�����򷵻ش���ֵ</param>   
        /// <returns>string</returns>   
        /**************************************************  
        * ʹ��ʾ��:  
        * XmlHelper.Read(path, "/Node", "")  
        * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")  
        ************************************************/
        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch { }
            return value;
        }
        /// <summary>   
        /// ��������   
        /// </summary>   
        /// <param name="path">·��</param>   
        /// <param name="node">�ڵ�</param>   
        /// <param name="element">Ԫ�������ǿ�ʱ������Ԫ�أ������ڸ�Ԫ���в�������</param>   
        /// <param name="attribute">���������ǿ�ʱ�����Ԫ������ֵ���������Ԫ��ֵ</param>   
        /// <param name="value">ֵ</param>   
        /// <returns></returns>   
        /**************************************************  
         * ʹ��ʾ��:  
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")  
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")  
        ************************************************/
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch { }
        }
        /// <summary>   
        /// �޸�����   
        /// </summary>   
        /// <param name="path">·��</param>   
        /// <param name="node">�ڵ�</param>   
        /// <param name="attribute">���������ǿ�ʱ�޸ĸýڵ�����ֵ�������޸Ľڵ�ֵ</param>   
        /// <param name="value">ֵ</param>   
        /// <returns></returns>   
        /**************************************************  
         * ʹ��ʾ��:  
         * XmlHelper.Insert(path, "/Node", "", "Value")  
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")  
        ************************************************/
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(path);
            }
            catch { }
        }
        /// <summary>   
        /// ɾ������   
        /// </summary>   
        /// <param name="path">·��</param>   
        /// <param name="node">�ڵ�</param>   
        /// <param name="attribute">���������ǿ�ʱɾ���ýڵ�����ֵ������ɾ���ڵ�ֵ</param>   
        /// <param name="value">ֵ</param>   
        /// <returns></returns>   
        /**************************************************  
        * ʹ��ʾ��:  
         * XmlHelper.Delete(path, "/Node", "")  
       * XmlHelper.Delete(path, "/Node", "Attribute")  
         ************************************************/
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(path);
            }
            catch { }
        }


        public static Dictionary<string, string> GetAppText(string path, string node)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode xn = doc.SelectSingleNode(node);
            XmlNodeList xnl = xn.ChildNodes;
            Dictionary<string, string> textCollection = new Dictionary<string, string>();
            foreach (XmlNode cnode in xnl)
            {
                string key = cnode.Attributes["name"].Value;
                string value = cnode.Attributes["value"].Value;
                textCollection.Add(key, value);
            }
            return textCollection;
        }




        public static GridLayout GetGridLayout(string areaName, string layoutName)
        {
            Dictionary<string, GridInfo> gridLayout = GetGridLayout(areaName, layoutName,  "");
            GridLayout gridInfo = new GridLayout();
            gridInfo.GridLayouts = gridLayout;
            gridInfo.GridTitle = GetGridTitle(areaName, layoutName);
            return gridInfo;
        }

        public static AdvanceGridLayout GetAdvanceGridLayout(string areaName, string layoutName)
        {
            Dictionary<string, GirdGroupHeaderInfo> groupHeader = GetGridGroupHeader(areaName, layoutName);
            Dictionary<string, GridInfo> gridLayout = GetGridLayout(areaName, layoutName, "");
            AdvanceGridLayout advanceGridInfo = new AdvanceGridLayout();
            advanceGridInfo.GridGroupHeader = groupHeader;
            advanceGridInfo.GridLayouts = gridLayout;
            advanceGridInfo.GridTitle = GetGridTitle(areaName, layoutName);
            return advanceGridInfo;
        }

        public static Dictionary<string, GirdGroupHeaderInfo> GetGridGroupHeader(string areaName, string layoutName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppMember.GridPath + areaName + "\\" + layoutName + ".xml");
            Dictionary<string, GirdGroupHeaderInfo> gridHeadersInfoCollection = new Dictionary<string, GirdGroupHeaderInfo>();
            XmlNode xn = doc.SelectSingleNode("gridLayout/groupHeader");
            if (xn == null)
                return null;
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode cnode in xnl)
            {
                GirdGroupHeaderInfo gridHeadersInfo = new GirdGroupHeaderInfo();
                gridHeadersInfo.StartColumnName = cnode.Attributes["startColumnName"].Value;
                if (cnode.Attributes["numberOfColumns"] != null)
                    gridHeadersInfo.NumberOfColumns = cnode.Attributes["numberOfColumns"].Value;
                if (cnode.Attributes["numberOfRows"] != null)
                    gridHeadersInfo.NumberOfRows = cnode.Attributes["numberOfRows"].Value;
                if (cnode.Attributes["titleText"] != null)
                    gridHeadersInfo.TitleText = cnode.Attributes["titleText"].Value;
                gridHeadersInfoCollection.Add(gridHeadersInfo.StartColumnName, gridHeadersInfo);
            }
            return gridHeadersInfoCollection;
        }

        public static string GetGridTitle(string areaName, string layoutName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppMember.GridPath + areaName + "\\" + layoutName + ".xml");
            Dictionary<string, GirdGroupHeaderInfo> gridHeadersInfoCollection = new Dictionary<string, GirdGroupHeaderInfo>();
            XmlNode xn = doc.SelectSingleNode("gridLayout/gridTitle");
            return xn.Attributes["name"].Value; ;
        }


        public static Dictionary<string, GridInfo> GetGridLayout(string areaName, string layoutName, string formMode)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppMember.GridPath + areaName + "\\" + layoutName + ".xml");
            Dictionary<string, GridInfo> gridHeadersInfoCollection = new Dictionary<string, GridInfo>();
            XmlNode xn = doc.SelectSingleNode("/gridLayout/columnLayout");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode cnode in xnl)
            {
                GridInfo gridHeadersInfo = new GridInfo();
                gridHeadersInfo.Name = cnode.Attributes["name"].Value;
                if (cnode.Attributes["caption"] != null)
                    gridHeadersInfo.Caption = cnode.Attributes["caption"].Value;
                if (cnode.Attributes["align"] != null)
                    gridHeadersInfo.Align = cnode.Attributes["align"].Value;
                if (cnode.Attributes["hidden"] != null)
                    gridHeadersInfo.Hidden = DataConvert.ToString(cnode.Attributes["hidden"].Value);
                if (cnode.Attributes["width"] != null)
                    gridHeadersInfo.Width = DataConvert.ToString(cnode.Attributes["width"].Value);
                if (cnode.Attributes["index"] != null)
                    gridHeadersInfo.Index = DataConvert.ToString(cnode.Attributes["index"].Value);
                if (formMode != "approve" && !formMode.Contains("view"))
                {
                    if (cnode.Attributes["editable"] != null)
                        gridHeadersInfo.Editable = DataConvert.ToString(cnode.Attributes["editable"].Value);
                }
                if (cnode.Attributes["edittype"] != null)
                    gridHeadersInfo.Edittype = DataConvert.ToString(cnode.Attributes["edittype"].Value);
                if (cnode.Attributes["formatter"] != null)
                    gridHeadersInfo.Formatter = DataConvert.ToString(cnode.Attributes["formatter"].Value);
                if (cnode.Attributes["datasourceurl"] != null)
                    gridHeadersInfo.Datasourceurl = DataConvert.ToString(cnode.Attributes["datasourceurl"].Value);
                if (cnode.Attributes["isRowspan"] != null)
                    gridHeadersInfo.IsRowspan = DataConvert.ToString(cnode.Attributes["isRowspan"].Value);
                gridHeadersInfoCollection.Add(gridHeadersInfo.Name, gridHeadersInfo);
            }
            return gridHeadersInfoCollection;
        }

    }
}
