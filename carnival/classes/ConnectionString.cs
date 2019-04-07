using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using logconfiguration;

namespace web
{
    public class ConnectionString
    {

        public string GetConnection()
        {
            try
            {

                string strConDe = ConfigurationManager.AppSettings["MysqlConnectionString"];

                Byte[] b = Convert.FromBase64String(strConDe);

                string decryptedConnectionString = System.Text.ASCIIEncoding.ASCII.GetString(b);

                return decryptedConnectionString;

            }
            catch (Exception ex)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 28, ex.Message.ToString(), "ConnectionString");
                return (ex.Message.ToString());
            }
            finally
            {

            }
        }

        public string Decrypt(string str)
        {
            try
            {

                //string strConDe = ConfigurationManager.AppSettings["ConnectionString"];

                Byte[] b = Convert.FromBase64String(str);

                string decryptedConnectionString = System.Text.ASCIIEncoding.ASCII.GetString(b);

                return decryptedConnectionString;

            }
            catch (Exception ex)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 52, ex.Message.ToString(), "ConnectionString");
                return (ex.Message.ToString());
            }
            finally
            {

            }
        }

        public string Encrypt(string sString)
        {

            byte[] sString_bytes = System.Text.UnicodeEncoding.UTF8.GetBytes(sString);
            return Convert.ToBase64String(sString_bytes);
        }

        public string GetCRMConnection()
        {
            try
            {
                string constr = ConfigurationManager.AppSettings["28_ConnectionString"].ToString();
                Byte[] b = Convert.FromBase64String(constr);

                string decryptedConnectionString = System.Text.ASCIIEncoding.ASCII.GetString(b);

                return decryptedConnectionString;

            }
            catch (Exception ex)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 82, ex.Message.ToString(), "ConnectionString");
                return ex.Message.ToString();
            }
            finally
            {
            }
        }

        public string Get245Connection()
        {
            try
            {

                string strConDe = ConfigurationManager.AppSettings["userconstring"];

                Byte[] b = Convert.FromBase64String(strConDe);

                string decryptedConnectionString = System.Text.ASCIIEncoding.ASCII.GetString(b);

                return decryptedConnectionString;

            }
            catch (Exception ex)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 28, ex.Message.ToString(), "ConnectionString");
                return (ex.Message.ToString());
            }
            finally
            {

            }
        }

    }
}
