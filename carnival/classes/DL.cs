using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;
using logconfiguration;
using System.Text;

namespace web
{
    public class DL
    {
        //static WriteErrLog logWrt = new WriteErrLog("DataLayer.txt");

        //-----------------sikandar -------

        ////public static DataSet GroupList(string sqlstmt)
        ////{

        ////}




        public static DataSet DL_ExecuteQuery(string strQuery,string [] arr_values)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 30, "DL:DL_ExecuteQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 31, "DL:DL_ExecuteQuery::Query received:"+strQuery);
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams;
                
                mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arr_values[i];
                }
               return MySqlHelper.ExecuteDataset(MSCon.ConnectionString, strQuery, mysqlparams);
               // return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocaluser, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
               // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 48, exc.Message.ToString(), "DL");
                return null;
            }
        }

        public static DataSet DL_ExecuteQuery1(string strQuery, string[] arr_values)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 30, "DL:DL_ExecuteQuery1::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 31, "DL:DL_ExecuteQuery1::Query received:" + strQuery);
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams;

                mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arr_values[i];
                }
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionString, strQuery, mysqlparams);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionString1, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 48, exc.Message.ToString(), "DL");
                return null;
            }
        }

        public static DataSet DL_ExecuteQuerylocal(string strQuery, string[] arr_values)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 30, "DL:DL_ExecuteQuery1::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 31, "DL:DL_ExecuteQuery1::Query received:" + strQuery);
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams;

                mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arr_values[i];
                }
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionString, strQuery, mysqlparams);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocalhost, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 48, exc.Message.ToString(), "DL");
                return null;
            }
        }



        public static DataSet DL_ExecuteQuery(string strQuery, string[] arr_values,string MSConParam)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 56, "DL:DL_ExecuteQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 57, "DL:DL_ExecuteQuery::Query received:" + strQuery);
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams;

                mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arr_values[i];
                }
                return MySqlHelper.ExecuteDataset(MSConParam, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
               // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 74, exc.Message.ToString(), "DL");
                return null;
            }
        }


        public static int DL_ExecuteSimpleNonQuery114(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 112, "DL:DL_ExecuteSimpleNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 113, "DL:DL_ExecuteSimpleNonQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionString114, strQuery);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 119, exc.Message.ToString(), "DL");
                return 0;
            }
        }


        public static DataSet DL_ExecuteSimpleQuery(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 82, "DL:DL_ExecuteSimpleQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 83, "DL:DL_ExecuteSimpleQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionString, strQuery);
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocaluser, strQuery);
                
            }
            catch (Exception exc)
            {
               // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 89, exc.Message.ToString(), "DL");
                return null;
            }
        }




        public static DataSet DL_ExecuteSimpleQueryhelo(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 82, "DL:DL_ExecuteSimpleQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 83, "DL:DL_ExecuteSimpleQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionString1, strQuery);
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocaluser, strQuery);

            }
            catch (Exception exc)
            {
                // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 89, exc.Message.ToString(), "DL");
                return null;
            }
        }
        public static DataSet DL_ExecuteSimpleQueryPersonalisedsms240(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 82, "DL:DL_ExecuteSimpleQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 83, "DL:DL_ExecuteSimpleQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringPersonalisedsms240, strQuery);
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocaluser, strQuery);

            }
            catch (Exception exc)
            {
                // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 89, exc.Message.ToString(), "DL");
                return null;
            }
        }

        public static DataSet DL_ExecuteSimpleQueryBackendDBConStr(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 82, "DL:DL_ExecuteSimpleQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 83, "DL:DL_ExecuteSimpleQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringpsmBackendDBConStr, strQuery);
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocaluser, strQuery);

            }
            catch (Exception exc)
            {
                // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 89, exc.Message.ToString(), "DL");
                return null;
            }
        }

        public static DataSet DL_ExecuteSimpleQuery245(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 82, "DL:DL_ExecuteSimpleQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 83, "DL:DL_ExecuteSimpleQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionString1, strQuery);
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocaluser, strQuery);

            }
            catch (Exception exc)
            {
                // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 89, exc.Message.ToString(), "DL");
                return null;
            }
        }

        public static DataSet DL_ExecuteSimpleQuerylocal(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 82, "DL:DL_ExecuteSimpleQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 83, "DL:DL_ExecuteSimpleQuery::Query received:" + strQuery);
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionString, strQuery);
               //return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocaluser, strQuery);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocalhost, strQuery);

            }
            catch (Exception exc)
            {
                // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 89, exc.Message.ToString(), "DL");
                return null;
            }
        }

        public static DataSet DL_ExecuteSimpleQuery230(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 82, "DL:DL_ExecuteSimpleQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 83, "DL:DL_ExecuteSimpleQuery::Query received:" + strQuery);
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionString, strQuery);
                //return MySqlHelper.ExecuteDataset(MSCon.ConnectionStringlocaluser, strQuery);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionString230, strQuery);

            }
            catch (Exception exc)
            {
                // logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 89, exc.Message.ToString(), "DL");
                return null;
            }
        }
        public static DataSet DL_ExecuteSimpleQuery(string strQuery, string MSConParam)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 97, "DL:DL_ExecuteSimpleQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 98, "DL:DL_ExecuteSimpleQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteDataset(MSConParam, strQuery);
            }
            catch (Exception exc)
            {
              //  logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 104, exc.Message.ToString(), "DL");
                return null;
            }
        }
        public static int DL_ExecuteSimpleNonQuery(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 112, "DL:DL_ExecuteSimpleNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 113, "DL:DL_ExecuteSimpleNonQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionString, strQuery);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 119, exc.Message.ToString(),"DL");
                return 0;
            }
        }
        public static DataSet DL_ExecuteSimpleNonQuery2(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 112, "DL:DL_ExecuteSimpleNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 113, "DL:DL_ExecuteSimpleNonQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionString1, strQuery);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 119, exc.Message.ToString(), "DL");
                return null;
            }
        }

        public static int DL_ExecuteSimpleNonQuery1(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 112, "DL:DL_ExecuteSimpleNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 113, "DL:DL_ExecuteSimpleNonQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionString1, strQuery);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 119, exc.Message.ToString(), "DL");
                return 0;
            }
        }

        public static int DL_ExecuteSimpleNonQuerylocal(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 112, "DL:DL_ExecuteSimpleNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 113, "DL:DL_ExecuteSimpleNonQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionStringlocalhost, strQuery);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 119, exc.Message.ToString(), "DL");
                return 0;
            }
        }

        public static int DL_ExecuteSimpleNonQuery240(string strQuery)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 112, "DL:DL_ExecuteSimpleNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 113, "DL:DL_ExecuteSimpleNonQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionStringPersonalisedsms240, strQuery);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 119, exc.Message.ToString(), "DL");
                return 0;
            }
        }

        public static long DL_ExecuteQueryid(string strQuery)
        {


            using (var connection = new MySqlConnection(MSCon.ConnectionString1)) //ConnectionString1
            {
                using (var command = connection.CreateCommand())
                {
                    byte [] UTF8encodes = UTF8Encoding .UTF8.GetBytes(strQuery);
                    command.CommandText = strQuery;
                    command.Parameters.Add(strQuery,UTF8encodes);
                    connection.Open();
                    command.ExecuteNonQuery();
                    long id = command.LastInsertedId;       // Get the ID of the inserted item

                    return id;
                }
            }

        }


        public static long DL_ExecuteQueryidlocal(string strQuery)
        {
            long id = 0;
            try {
            
            using (var connection = new MySqlConnection(MSCon.ConnectionStringlocalhost)) //ConnectionString1
            {
                using (var command = connection.CreateCommand())
                {
                    byte[] UTF8encodes = UTF8Encoding.UTF8.GetBytes(strQuery);
                    command.CommandText = strQuery;
                    command.Parameters.Add(strQuery, UTF8encodes);
                    connection.Open();
                    command.ExecuteNonQuery();
                    id =1;       // Get the ID of the inserted item

                    return id;
                    }
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 134, exc.Message.ToString(), "DL");
                return id;
            }

        }


        public static long DL_ExecuteQueryidlocalhost(string strQuery)
        {


            using (var connection = new MySqlConnection(MSCon.ConnectionStringlocalhost)) //ConnectionString1
            {
                using (var command = connection.CreateCommand())
                {
                    byte[] UTF8encodes = UTF8Encoding.UTF8.GetBytes(strQuery);
                    command.CommandText = strQuery;
                    command.Parameters.Add(strQuery, UTF8encodes);
                    connection.Open();
                    command.ExecuteNonQuery();
                    long id = command.LastInsertedId;       // Get the ID of the inserted item

                    return id;
                }
            }

        }



        public static long DL_ExecuteQueryid230(string strQuery)
        {
            long id = 0;
            try
            {

                using (var connection = new MySqlConnection(MSCon.ConnectionString230)) //ConnectionString1
                {
                    using (var command = connection.CreateCommand())
                    {
                        byte[] UTF8encodes = UTF8Encoding.UTF8.GetBytes(strQuery);
                        command.CommandText = strQuery;
                        command.Parameters.Add(strQuery, UTF8encodes);
                        connection.Open();
                        command.ExecuteNonQuery();
                        id = 1;       // Get the ID of the inserted item

                        return id;
                    }
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 134, exc.Message.ToString(), "DL");
                return id;
            }

        }


        public static int DL_ExecuteSimpleNonQuery(string strQuery, string MSConParam)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 127, "DL:DL_ExecuteSimpleNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 128, "DL:DL_ExecuteSimpleNonQuery::Query received:" + strQuery);
                return MySqlHelper.ExecuteNonQuery(MSConParam, strQuery);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 134, exc.Message.ToString(), "DL");
                return 0;
            }
        }
        public static int DL_ExecuteNonQuery(string strQuery, string[] arr_values)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 142, "DL:DL_ExecuteNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 143, "DL:DL_ExecuteNonQuery::Query received:" + strQuery);
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arr_values[i];
                }
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionString, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 158, exc.Message.ToString(), "DL");
                return 0;
            }
        }

        public static int DL_ExecuteNonQueryhelo(string strQuery, string[] arr_values)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 142, "DL:DL_ExecuteNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 143, "DL:DL_ExecuteNonQuery::Query received:" + strQuery);
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arr_values[i];
                }
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionString1, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 158, exc.Message.ToString(), "DL");
                return 0;
            }
        }
        public static int DL_ExecuteNonQuerylocal(string strQuery, string[] arr_values)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 142, "DL:DL_ExecuteNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 143, "DL:DL_ExecuteNonQuery::Query received:" + strQuery);
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arr_values[i];
                }
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionStringlocalhost, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 158, exc.Message.ToString(), "DL");
                return 0;
            }
        }


        public static int DL_ExecuteNonQuery(string strQuery, string[] arr_values, string MSConParam)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 166, "DL:DL_ExecuteNonQuery::function starts here.");
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 167, "DL:DL_ExecuteNonQuery::Query received:" + strQuery);
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arr_values[i];
                }
                return MySqlHelper.ExecuteNonQuery(MSConParam, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 182, exc.Message.ToString(), "DL");
                return 0;
            }
        }
        protected static string[] getQueryParameters(string strQuery)
        {
            try
            {
                //Regex reg = new Regex("\\?[a-zA-Z]*.[a-zA-Z_0-9]*");
                Regex reg = new Regex("\\?[a-zA-Z0-9_]*.[a-zA-Z0-9_@]*");
                string param = "";
                foreach (Match m in reg.Matches(strQuery))
                    param += m.Value.TrimStart('?') + ",";
                string[] arr = param.TrimEnd(',').Split(',');
                return arr;
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("----Regular Expression Error " + DateTime.Now.GetDateTimeFormats()[39] + "  ----\n" + strQuery + "\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 200, exc.Message.ToString(), "DL");
                return null;
            }
        }
    }
}
