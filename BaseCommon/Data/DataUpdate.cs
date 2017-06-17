
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using BaseCommon.Basic;
namespace BaseCommon.Data
{
    /// <summary>
    /// 数据更新
    /// </summary>
    public class DataUpdate
    {
        protected string connectionString;
        protected DbProviderFactory provider;
        protected DbConnection connection;
        public DbCommand cmd;
        public DataUpdate()
        {
            connectionString = AppMember.ConnectionString;
            provider = AppMember.Provider;
            cmd = provider.CreateCommand();
            connection = provider.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            cmd.Connection = connection;
        }

        public int Update(DataTable dt)
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



        public void BeginTransaction()
        {
            try
            {
                DbTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Dispose();
                throw new Exception(ex.Message);
            }
        }

        public void Commit()
        {
            cmd.Transaction.Commit();
        }
        public void Rollback()
        {
            cmd.Transaction.Rollback();
        }

        public void Close()
        {
            if (this.connection.State == ConnectionState.Open)
            {
                this.connection.Close();
            }
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }
        }

        public int Update(DataTable dt,bool hasTransaction)
        {
            int updateCount = 0;
            try
            {
                BeginTransaction();
                updateCount = Update(dt);
                Commit();
            }
            catch (Exception ex)
            {
                Rollback();
                throw ex;
            }
            finally
            {
                Close();
            }
            return updateCount;
        }

        public int Update(List< DataTable> dtList, bool hasTransaction)
        {
            int updateCount = 0;
            try
            {
                BeginTransaction();
                for (int i = 0; i < dtList.Count; i++)
                {
                    updateCount += Update(dtList[i]);
                }
                Commit();
            }
            catch (Exception ex)
            {
                Rollback();
                throw ex;
            }
            finally
            {
                Close();
            }
            return updateCount;
        }
    }
}
