using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.IO;
using logconfiguration;

namespace web
{
    public class MSCon
    {


        public static string ConnectionString
        {
            get { return DecryptConnectionString(ConfigurationManager.AppSettings["MysqlConnectionString"]); }
        }

        public static string ConnectionStringlocaluser
        {
            get { return DecryptConnectionString(ConfigurationManager.AppSettings["userconstringlocal"]); }
        }
        public static string ConnectionStringPersonalisedsms240
        {
            get { return DecryptConnectionString(ConfigurationManager.AppSettings["Personalisedsms240"]); }
        }
        public static string ConnectionString114
        {
            get { return DecryptConnectionString(ConfigurationManager.AppSettings["114constring"]); }
        }


        public static string ConnectionString1
        {
            get { return DecryptConnectionString(ConfigurationManager.AppSettings["userconstring"]); }
        }

        public static string ConnectionStringlocalhost
        {
            get { return DecryptConnectionString(ConfigurationManager.AppSettings["userconstringlocalhost"]); }
        }
        public static string ConnectionStringpsmBackendDBConStr
        {
            get { return DecryptConnectionString(ConfigurationManager.AppSettings["BackendRegularConnectionString"]); }
        }
        public static string ConnectionString230
        {
            get { return DecryptConnectionString(ConfigurationManager.AppSettings["userconstring"]); }
        }
        public static string DecryptConnectionString(string strConn)
        {
            try
            {
                //string strConDe = ConfigurationManager.AppSettings["ConnectionString"];
                //"Server=localhost;Port=3306;Database=swiftsmsdb;Uid=root;Pwd=Airtel_169%;";
                //string strConDe = ConfigurationManager.AppSettings["MysqlConnectionString"];
                Byte[] b = Convert.FromBase64String(strConn);
                return System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 36, exc.Message.ToString(), "MSCon");
                return "";
            }
        }

        public static MySqlDbType getMySqlDbType(string DbColumnName, out int size_ret)
        {
            try
            {
                string ds = HttpContext.Current.Application["un"].ToString();
                string path = HttpContext.Current.ApplicationInstance.Server.MapPath("//App_Data//Properties.xml");
                //string path = Path.("//App_Data//Properties.xml");
                XElement xe = XElement.Load(path);
                MySqlDbType msdt = MySqlDbType.VarChar;
                size_ret = 0;
                foreach (XElement xit in xe.Elements())
                {
                    string param = xit.Attribute("param").Value;
                    string msdbtype = xit.Attribute("msdbtype").Value;
                    string size = xit.Attribute("size").Value;
                    if (param == DbColumnName)
                    {
                        size_ret = Convert.ToInt32(size);
                        switch (msdbtype)
                        {
                            case "Varchar":
                                {
                                    msdt = MySqlDbType.VarChar;
                                    break;
                                }
                            case "int":
                                {
                                    msdt = MySqlDbType.Int32;
                                    break;
                                }
                        }
                        break;
                    }//end of if condition
                }//end of for loop

                return msdt;
            }

            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 80, exc.Message.ToString(), "MSCon");
                size_ret = 1;
                return MySqlDbType.Binary;
            }
        }

    }
}
