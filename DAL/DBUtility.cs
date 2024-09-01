using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DBUtility
    {
        private static string vSrvName = ConfigurationManager.AppSettings["SrvName"];
        private static string vDBName = ConfigurationManager.AppSettings["DBName"];
        private static string vDBUser = ConfigurationManager.AppSettings["DBUser"];
        private static string vPw = ConfigurationManager.AppSettings["PassPW"];

        private DBUtility()
        { }

        public static string GetConStr()
        {
            string ConStr;
            ConStr = "Data Source=" + vSrvName + "; User Id=" + vDBUser + "; Password=" + vPw + "; Database=" + vDBName + "; Connect Timeout=360000; pooling='true'; Max Pool Size=200;";
          //  ConStr = "data source="+ vSrvName + "; Initial Catalog=" + vDBName + ";Integrated Security=True;";
            return ConStr;
        }

        public static SqlConnection GetConnection()
        {
            try
            {
                string conStr = GetConStr();
                SqlConnection connection = new SqlConnection(conStr);
                if (connection != null)
                    connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CloseConnection()
        {
            string conStr = GetConStr();
            SqlConnection oCon = new SqlConnection(conStr);
            if (oCon.State == ConnectionState.Open)
                oCon.Close();
        }

        public static SqlTransaction GetTransaction(SqlConnection pConn)
        {
            return pConn.BeginTransaction();
        }

        public static void AddParameter(SqlParameterCollection pPrms, ParameterDirection pDirection, SqlDbType pDbType, int pSize, string pName, object pValue)
        {
            SqlParameter tmpParam = new SqlParameter(pName, pDbType, pSize);
            tmpParam.Direction = pDirection;
            tmpParam.Value = pValue;
            pPrms.Add(tmpParam);
        }

        public static void Execute(SqlCommand pCommand)
        {
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            try
            {
                pCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }

        public static void ExecuteSql(string pSqlString)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = pSqlString;
                oCmd.Connection = GetConnection();
                oCmd.CommandTimeout = 3600;
                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Connection.Close();
            }
        }

        public static void Execute(SqlCommand pCommand, SqlConnection pConn, SqlTransaction pTransaction)
        {
            pCommand.Connection = pConn;
            pCommand.Transaction = pTransaction;
            try
            {
                pCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static void ExecuteScaler(SqlCommand pCommand)
        {
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            try
            {
                pCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }

        public static double ExecDblScaler(SqlCommand pCommand)
        {
            double dRst = 0;
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            try
            {
                dRst = Convert.ToDouble(pCommand.ExecuteScalar());
                return dRst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }

        public static Int32 ExecIntScaler(SqlCommand pCommand)
        {
            Int32 dRst = 0;
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            try
            {
                dRst = Convert.ToInt32(pCommand.ExecuteScalar());
                return dRst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }

        public static string EexcStrScaler(string pSqlString)
        {
            SqlCommand oCmd = new SqlCommand();
            string vResult = "";
            try
            {
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = pSqlString;
                oCmd.Connection = GetConnection();
                oCmd.CommandTimeout = 3600;
                if (oCmd.ExecuteScalar() == DBNull.Value)
                    vResult = "";
                else
                    vResult = (string)oCmd.ExecuteScalar();
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Connection.Close();
            }
        }

        public static double EexcDblScaler(string pSqlString)
        {
            SqlCommand oCmd = new SqlCommand();
            double vResult = 0;
            try
            {
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = pSqlString;
                oCmd.Connection = GetConnection();
                oCmd.CommandTimeout = 3600;
                vResult = Convert.ToDouble(oCmd.ExecuteScalar());
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Connection.Close();
            }
        }

        public static void ExecuteForSelect(SqlCommand pCommand, DataTable pDataTable)
        {
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = pCommand;
            try
            {
                da.Fill(pDataTable);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }

        //Added on 29032019

        public static byte[] ExecuteByteScaler(SqlCommand pCommand)
        {
            byte[] retByte = null;
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            try
            {
                retByte = (byte[])pCommand.ExecuteScalar();
                return retByte;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }
        // end

        public static DataTable GetDataTable(SqlCommand pCommand)
        {
            DataTable dt = new DataTable();
            pCommand.Connection = GetConnection();
            pCommand.CommandTimeout = 3600;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = pCommand;
            try
            {
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }

        public static DataSet GetDataSet(SqlCommand pCommand)
        {
            DataSet ds = new DataSet();
            try
            {
                pCommand.Connection = GetConnection();
                pCommand.CommandTimeout = 36000;
                SqlDataAdapter da = new SqlDataAdapter(pCommand);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Connection.Close();
            }
        }
    }
}
