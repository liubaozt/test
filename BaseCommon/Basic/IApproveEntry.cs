using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using System.Data;

namespace BaseCommon.Basic
{
    public interface IApproveEntry :IEntry
    {
        DataTable GetEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey, string formVar);

        int InitFirstApproveTask(string tableName, string pkFiled, string refId, string viewTitle, string formMode = "", string createUser = "");

        int InitNextApproveTask(string tableName, string nodeId, string currentApproveLevel, string currentApprover, string pkFiled, string refId, string viewTitle, ref bool isEndNode);

        int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle);

       
    }
}
