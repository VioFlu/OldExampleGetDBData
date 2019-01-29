using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Create a class which handels the connection

namespace GetDBData
{
    public class DBDataHandler
    {
        private static ILog log = LogManager.GetCurrentClassLogger();


        public void RunSqlTransaction(SqlDataAdapter da, string myStringConnection, DataSet ds)
        {
            SqlConnection connection = new SqlConnection(myStringConnection);
            connection.Open();
            SqlTransaction myTrans = connection.BeginTransaction();
            SqlCommand myCommand = new SqlCommand();

            myCommand.Transaction = myTrans;

            try
            {
                da.Update(ds);
                myCommand.Transaction.Commit();
                Console.WriteLine("Update successful.");
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                }
                catch (SqlException ex)
                {
                    if (myTrans.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                                          " was encountered while attempting to roll back the transaction.");
                    }
                }
                
                // Should be added a log file to get the error 
                log.Trace(e.Message);
            }
            connection.Close();
        }

        public void RunSQLGetData(string myStringConnection,out DataSet dsGeneric, string cmdString)
        {
            SqlConnection connection = new SqlConnection(myStringConnection);
            dsGeneric = null;
            try{
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = connection.CreateCommand();
                cmd.CommandText = cmdString;
                dsGeneric = GetDataSet(cmd);
                // don't forget to close the connection, usually it consumes a lot an oppened connection
                connection.Close();
            }
            catch(Exception e){
                connection.Close();
                log.Debug(e.Message);
            }
        }

        public static void RunSQLCommand(string myStringConnection, string cmdString){
            SqlConnection connection = new SqlConnection(myStringConnection);
            try{
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = connection.CreateCommand();
                cmd.CommandText = cmdString;
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex){
                connection.Close();
                log.Debug(ex.Message);
            }

        }
        public static void RunSQLCommandTest(string myStringConnection, string cmdString, List<SqlParameter> paramsCol)
        {
            SqlConnection connection = new SqlConnection(myStringConnection);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = connection.CreateCommand();
                cmd.CommandText = cmdString;
                cmd.Parameters.AddRange(paramsCol.ToArray());
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                log.Debug(ex.Message);
            }

        }

        DataSet GetDataSet(SqlCommand sqlCMD)
        {
            DataSet ds = new DataSet();
            ds = null;
            try{
               // DataSet ds = new DataSet();
                DataTable table = new DataTable();
                table.Load(sqlCMD.ExecuteReader());
                DataSet dsTest = new DataSet();
                dsTest.Tables.Add(table);
                ds = dsTest;
                return ds;
            }
            catch (Exception ex){
                log.Debug(ex.Message);
                return ds;
            }
        }

    }
}
