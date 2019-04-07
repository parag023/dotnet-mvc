using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pullsms_reports.Models
{
    public class userPullSms
    {
        public Int64 userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public Int16 isActive { get; set; }
        public string pkeyUserName { get; set; }
    }
}