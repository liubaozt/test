using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SQLite;

namespace BaseCommon.Data
{
    public class SqliteHelper
    {
        SQLiteConnection conn = null;
        SQLiteTransaction trans = null;
        SQLiteCommand command;

        public SqliteHelper()
        {
            conn = new SQLiteConnection(string.Format("Data Source = {0}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Content\\uploads\\sqlite\\", "PDA.db")));
        }

        public SqliteHelper(string DBName)
        {
            conn = new SQLiteConnection(string.Format("Data Source = {0}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Content\\uploads\\sqlite\\", DBName)));
        }

        public void Open()
        {
            conn.Open();
        }
        public void Close()
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public DataSet FillDataSet(string sql)
        {
            DataSet ds = new DataSet();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);

            try
            {
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                adapter.Dispose();
            }
        }
        public object ExecuteScalar(string sql)
        {
            try
            {
                command = new SQLiteCommand(sql, conn);
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Dispose();
            }
        }
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                command = new SQLiteCommand(sql, conn);
                return ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Dispose();
            }
        }
        public int ExecuteNonQuery(SQLiteCommand command)
        {
            try
            {
                command.Connection = conn;
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Dispose();
            }
        }
        public void BeginTransaction()
        {
            if (conn != null && conn.State != ConnectionState.Closed)
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
        }
        public void Commit()
        {
            if (trans != null)
                trans.Commit();
        }
        public void RollBack()
        {
            if (trans != null)
                trans.Rollback();
        }
    }
}
