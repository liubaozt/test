/*************************************************************************************************
  Copyright (C) 2011, FANS Corp.
  �ļ���:	DataUpdate.cs
  ����:     ����ӡ
  ������:   ���ݸ�����
  �汾��ʷ:
      <����>		<����>		<�汾>		<���>
      ����ӡ      2011/08/25     1.0		��������  
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using BaseCommon.Basic;
namespace BaseCommon.Data
{
    /// <summary>
    /// ���ݸ���
    /// </summary>
    public class DataUpdate
    {
        protected string connectionString;
        protected DbProviderFactory provider;
        public DataUpdate()
        {
            connectionString = AppMember.ConnectionString;
            provider = AppMember.Provider;
        }

        /// <summary>
        /// ���µ��ű������������
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int Update(DataTable dt)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                using (DbCommand cmd = provider.CreateCommand())
                {
                    string relTableName = dt.TableName;
                    string SQLString = string.Format("select * from {0}  ", relTableName);
                    cmd.Connection = connection;
                    cmd.CommandText = SQLString;
                    try
                    {
                        DbDataAdapter adapter = provider.CreateDataAdapter();
                        DbCommandBuilder objCommandBuilder = provider.CreateCommandBuilder();
                        adapter.SelectCommand = cmd;
                        objCommandBuilder.DataAdapter = adapter;
                        adapter.DeleteCommand = objCommandBuilder.GetDeleteCommand();
                        adapter.InsertCommand = objCommandBuilder.GetInsertCommand();
                        adapter.UpdateCommand = objCommandBuilder.GetUpdateCommand();
                        foreach (DataRow Row in dt.Rows)
                            Row.EndEdit();
                        int count = adapter.Update(dt);
                        dt.AcceptChanges();
                        return count;
                    }
                    catch (DbException ex)
                    {
                        connection.Close();
                        connection.Dispose();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// ͬʱ���¶��ű�ʱ��Ҫ�õ����񣬲���һ�����ݼ��������ű�ʽ����
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public int Update(DataSet ds)
        {
            using (DbConnection connection = provider.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (DbCommand cmd = provider.CreateCommand())
                {
                    cmd.Connection = connection;
                    using (DbTransaction tx = connection.BeginTransaction())
                    {
                        cmd.Transaction = tx;
                        try
                        {
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                Update(dt, cmd);
                            }
                            ds.AcceptChanges();
                            tx.Commit();
                            return 1;
                        }
                        catch (DbException ex)
                        {
                            tx.Rollback();
                            connection.Close();
                            connection.Dispose();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
        }

        public int Update(DataTable dt, DbCommand cmd)
        {
            try
            {
                string relTableName = dt.TableName;
                string SQLString = string.Format("select * from {0}  ", relTableName);
                cmd.CommandText = SQLString;
                DbDataAdapter adapter = provider.CreateDataAdapter();
                DbCommandBuilder objCommandBuilder = provider.CreateCommandBuilder();
                adapter.SelectCommand = cmd;
                objCommandBuilder.DataAdapter = adapter;
                adapter.DeleteCommand = objCommandBuilder.GetDeleteCommand();
                adapter.InsertCommand = objCommandBuilder.GetInsertCommand();
                adapter.UpdateCommand = objCommandBuilder.GetUpdateCommand();
                foreach (DataRow Row in dt.Rows)
                    Row.EndEdit();
                int count = adapter.Update(dt);
                dt.AcceptChanges();
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// �������ı�
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public DataTable GetTable(string tableName, string whereCondition)
        {
            string sqlString = string.Format("select * from {0} where 1=1 and {1} ", tableName, whereCondition);
            DataTable dt = AppMember.DbHelper.GetDataSet(sqlString).Tables[0];
            dt.TableName = tableName;
            return dt;
        }

        /// <summary>
        /// �������ŵ�ͬһ�����ݼ���
        /// </summary>
        /// <param name="dtList"></param>
        /// <returns></returns>
        public DataSet MergeDataTable(params DataTable[] dtList)
        {
            DataSet ds = new DataSet();
            for (int i = 0; i < dtList.Length; i++)
            {
                if (dtList[i] != null)
                {
                    DataTable dt = dtList[i].Clone();
                    foreach (DataRow dr in dtList[i].Rows)
                    {
                        dt.ImportRow(dr);
                    }
                    ds.Tables.Add(dt);
                }
            }
            return ds;
        }

        public DataSet MergeDataTable(List<DataTable> dtList, bool isByList = true)
        {
            DataSet ds = new DataSet();
            for (int i = 0; i < dtList.Count; i++)
            {
                if (dtList[i] != null)
                {
                    DataTable dt = dtList[i].Clone();
                    foreach (DataRow dr in dtList[i].Rows)
                    {
                        dt.ImportRow(dr);
                    }
                    ds.Tables.Add(dt);
                }
            }
            return ds;
        }

        public DbCommand BeginTransaction()
        {
            DbConnection connection = provider.CreateConnection();
            try
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                DbCommand cmd = provider.CreateCommand();
                cmd.Connection = connection;
                DbTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                return cmd;
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Dispose();
                throw new Exception(ex.Message);
            }
        }

        public void Commit(DbCommand cmd)
        {
            cmd.Transaction.Commit();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        public void Rollback(DbCommand cmd)
        {
            cmd.Transaction.Rollback();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }
}
