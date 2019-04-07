using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace pullsms_reports.Models
{
    public class reportPullSms
    {
        public Int64 MsgID { get; set; }
        // public string UserName { get; set; }
        // public string PKey { get; set; }
        public string SKey { get; set; }
        public string MobileNo { get; set; }
        public string Message { get; set; }
        public DateTime RecDateTime { get; set; }
        public string Circle { get; set; }
        public string Source { get; set; }
    }

    public class ddlshortcode
    {
        public string PKey { get; set; }
        public string shortcode { get; set; }
    }


    public class ddlskey
    {
        public string SKey { get; set; }
        public string SKeyvalue { get; set; }
    }

    public class ddlpkey
    {
        public string PKey { get; set; }
        public string Pkeyvalue { get; set; }
    }

    public class rptpull
    {
        public string source { get; set; }
        public string pkey { get; set; }
        public string skey { get; set; }
    }

    public class resultjson
    {
        public int status { get; set; }
        public string message { get; set; }

       // public List<reportPullSms> bindrptpull { get; set; }
    }

    public class cngpwd
    {
        public string oldpwd { get; set; }
        public string newpwd { get; set; }
        public string confirtpwd { get; set; }
    }

    public class datejson
    {
        public string fromdate { get; set; }
        public string todate { get; set; }

    }
    public class ajaxdatewisejson
    {
        public string source { get; set; }
        public string pkey { get; set; }
        public string skey { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string reporttype { get; set; }
    }
    public class resultjson1
    {
        public int status { get; set; }
        public string message { get; set; }
        public List<reportPullSms> reportpullsms { get; set; }
    }
}