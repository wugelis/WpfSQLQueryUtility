using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WpfSQLQueryUtility.Datas
{
    /// <summary>
    /// �إߤ���G2007/10/26. by Gelis.
    /// ��Ƽh��¦���O(��SQLServer)
    /// </summary>
    public class DAL
    {
        //��Ʈw�s���r��(�ϥ� web.config�Ӱt�m
        protected static string connectionString = "";
        protected SqlConnection rd_conn;

        public DAL()
        {
            connectionString = MainWindow.ConnectionInfo.ConnectionString; //(new DBConn()).Connect();
        }
        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        /// <summary>
        /// ����SQL Statement
        /// </summary>
        /// <param name="SQLString">SQL Statement</param>
        /// <returns></returns>
        public DataSet Query(string SQLString)
        {
            return Query(SQLString, null);
        }
        /// <summary>
        /// ����SQL Statement (�ݭn�Ѽƭ�)
        /// </summary>
        /// <param name="SQLString">SQL Statement</param>
        /// <param name="cmdParms">�Ѽƭ�</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                        return ds;
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        //throw new Exception(ex.Message);
                        throw ex;
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ExecuteSQL(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);

                try
                {
                    if(connection.State!=ConnectionState.Open)
                        connection.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception("�s��SQL Server�o�Ϳ��~. SysInfo=" + ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                    connection.Dispose();
                    cmd.Dispose();
                }
            }
        }
        /// <summary>
        /// ���o��@��.
        /// Add by Gelis at 2011/3/22.
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public object GetExecuteScalar(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection CN = null;
            SqlCommand CMD = null;

            try
            {
                CN = new SqlConnection((new DBConn()).Connect());
                PrepareCommand(CMD, CN, null, SQLString, cmdParms);

                CN.Open();
                object result = CMD.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    result = CMD.ExecuteScalar();
                }
                return int.Parse(result.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (CN.State != ConnectionState.Closed)
                {
                    CN.Close();
                    CN.Dispose();
                    CN = null;
                }
                if (CMD != null)
                {
                    CMD.Dispose();
                    CMD = null;
                }
            }
        }
        /// <summary>
        /// ���o�o�� SqlConnection �����c�y�z��T.
        /// </summary>
        /// <param name="SchemaName"></param>
        /// <returns></returns>
        public DataTable GetSchemaDataTable(string SchemaName)
        {
            SqlConnection cnn = new SqlConnection(connectionString);
            try
            {
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
                return cnn.GetSchema(SchemaName);
            }
            finally
            {
                if (cnn.State != ConnectionState.Closed)
                    cnn.Close();
                cnn.Dispose();
            }
        }
    }
}