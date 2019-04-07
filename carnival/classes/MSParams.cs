using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Collections;
using logconfiguration;

namespace web
{
    /// <summary>
    /// Represents column name with its DBType <para>and size defined in swiftsmsdb Database.</para>
    /// </summary>
    public class MSParams
    {
        private  string _ParamName;
        private  MySqlDbType _dbtype;
        private  int _paramsize;
        //static WriteErrLog logWrt = new WriteErrLog("DataLayer.txt");
        public string ParamName
        {
            get { return this._ParamName; }
        }
        public MySqlDbType DbType
        {
            get { return this._dbtype; }
        }
        public int ParamSize
        {
            get { return this._paramsize; }
        }

        public MSParams(string _ParamName, MySqlDbType _dbtype, int _paramsize)
        {
            this._ParamName = _ParamName;
            this._dbtype = _dbtype;
            this._paramsize = _paramsize;
        }
        public MSParams(string _ParamName, MySqlDbType _dbtype)
        {
            this._ParamName = _ParamName;
            this._dbtype = _dbtype;
        }


        public static Dictionary<string, MSParams> allparams = new Dictionary<string, MSParams>() { 

            { "global.username" , new MSParams( "global.username",MySqlDbType.VarChar , 50)},
            { "global.pwd" , new MSParams( "global.pwd",MySqlDbType.VarChar , 50)},
            { "global.shortcode" , new MSParams( "global.shortcode",MySqlDbType.VarChar , 50)},
            { "global.pkey" , new MSParams( "global.pkey",MySqlDbType.VarChar , 50)},
            { "global.skey" , new MSParams( "global.skey",MySqlDbType.VarChar , 50)},
            { "global.password" , new MSParams( "global.password",MySqlDbType.VarChar , 50)},
            { "global.addvalue" ,new MSParams("global.addvalue" , MySqlDbType.Int32)},
            { "global.default4fest" , new MSParams( "global.default4fest",MySqlDbType.VarChar , 15)},
            { "global.date" , new MSParams( "global.date",MySqlDbType.DateTime)},
            { "global.text" , new MSParams( "global.text",MySqlDbType.Text)},
            { "global.sendsmsautoid" , new MSParams( "global.sendsmsautoid",MySqlDbType.VarChar,50)},
           
            {"global.senderid", new MSParams( "global.senderid",MySqlDbType.VarChar,50)},
            {"global.cdmaheader", new MSParams( "global.cdmaheader",MySqlDbType.VarChar,50)},

            {"global.tagname",new MSParams("global.tagname",MySqlDbType.VarChar,50)},
            { "global.destination" , new MSParams( "global.destination",MySqlDbType.Int64 , 11)},
            // campaign 

            {"cmp.user_id",new MSParams("cmp.user_id",MySqlDbType.Int64,50) },
            {"cmp.id",new MSParams("cmp.id",MySqlDbType.VarChar,50) },
            {"cmp.username",new MSParams("cmp.username",MySqlDbType.VarChar,50) },
            {"cmp.senderid",new MSParams("cmp.senderid",MySqlDbType.VarChar,50) },
            {"cmp.cdmaheader",new MSParams("cmp.cdmaheader",MySqlDbType.VarChar,50)},
            {"cmp.message",new MSParams("cmp.message",MySqlDbType.VarChar,5000)},
            {"cmp.filename",new MSParams("cmp.filename",MySqlDbType.VarChar,100)},
            {"cmp.starttime",new MSParams("cmp.starttime",MySqlDbType.VarChar,100)},
            {"cmp.passkey",new MSParams("cmp.passkey",MySqlDbType.VarChar,100)},

            {"cmp.camp_name",new MSParams("cmp.camp_name",MySqlDbType.VarChar,50) },
            {"cmp.destination",new MSParams("cmp.destination",MySqlDbType.VarChar,1000)}


        };

        /// <summary>
        /// Returns object of MSParams
        /// </summary>
        /// <param name="paramname"></param>
        /// <returns></returns>
        public static MSParams getMSParams(string paramname)
        {
            try
            {
                MSParams msOut;
                allparams.TryGetValue(paramname, out msOut);
                return msOut;
            }
            catch (Exception exc)
            {
                //logWrt.WriteError_File("-----------  " + DateTime.Now.GetDateTimeFormats()[39] + "  -----------\n" + exc.ToString() + "\n  -------------------END--------------  \n\n");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 315, exc.Message.ToString(), "MSParams");
                return null;
            }
        }
    }
}
