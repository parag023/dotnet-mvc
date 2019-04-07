//using carnival.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web;
using carnival.classes;
using logconfiguration;
using System.IO;
using System.Text;
using System.Configuration;
using System.Globalization;
using pullsms_reports.Models;
using Newtonsoft.Json;
using pullsms_reports.Fillter;

namespace pullsms_reports.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /SMSData/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult pullSmsReport()
        {
            try
            {
                userPullSms objuser = new userPullSms();
                if (Session["userObject"] != null)
                {
                    objuser = (userPullSms)Session["userObject"];
                    ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 96, "Inbox:GetPrimaryKeyword()::Function starts here.");
                    string query1 = "select distinct(shortcode) from shortcode where username = ?global.username; "; // distinct(PKey),
                    var shortcode_source = new HashSet<string> { };

                    string[] param = new string[] { objuser.pkeyUserName };
                    DataSet ds1 = DL.DL_ExecuteQuery(query1, param);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        ____logconfig.Log_Write(____logconfig.LogLevel.DB, 100, "Inbox:GetPrimaryKeyword()::Query to select from shortcode table:" + query1);
                        ____logconfig.Log_Write(____logconfig.LogLevel.DB, 101, "Inbox:GetPrimaryKeyword()::Result of query to select from shortcode table, no of rows:" + ds1.Tables[0].Rows.Count.ToString());

                        List<ddlshortcode> lstshortcode = utility.BindDataList<ddlshortcode>(ds1.Tables[0]);
                        List<SelectListItem> items = new List<SelectListItem>();

                        foreach (var t in lstshortcode)
                        {
                            SelectListItem s = new SelectListItem();

                            s.Text = t.shortcode;
                            s.Value = t.shortcode;
                            items.Add(s);
                        }
                        ViewBag.source = items;

                        //ViewBag.pkey = itempkey;
                        //ViewBag.skey = itemskey;
                    }
                    else
                    {
                        TempData["error"] = "No keyword found in your account.";
                    }
                }
                else
                {
                    TempData["error"] = "session expired! please login again";
                    return RedirectToAction("loginpullsms", "Account");
                }

            }
            catch (Exception exc)
            {
                TempData["error"] = "Problem While loading Data!!";
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "pullSmsReport");
                return View();
            }

            return View();

           
        }

        #region ajaxcode
        [HttpPost]
        [SessionTimeOut]
        public ActionResult pullSmsReport1(rptpull objrptpull)
        {
            resultjson1 objroot = new resultjson1();
            try
            {
                userPullSms objuser = new userPullSms();
                if (Session["userObject"] != null)
                {
                    objuser = (userPullSms)Session["userObject"];

                    string shortcode = objrptpull.source;
                    string pkey = objrptpull.pkey;
                    string skey = objrptpull.skey;
                    ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 249, "Inbox:getallreport()::Function starts here.");
                    DataSet dt;
                    string strQuery = "";
                    if (skey == "--------All--------") //DropDownList3.SelectedValue.ToString() 
                    {
                        strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                        string[] arr_values = new string[] { pkey.Trim(), objuser.pkeyUserName, shortcode };
                        dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                    }
                    else
                    {
                        strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and SKey=?global.skey and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                        string[] arr_values = new string[] { pkey.Trim(), skey.Trim(), objuser.pkeyUserName, shortcode };
                        dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                    }
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 264, "Inbox:getallreport()::Query to select from inbox table:" + strQuery);
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 265, "Inbox:getallreport()::Result of query to select from inbox table, no of rows:" + dt.Tables[0].Rows.Count.ToString());
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        

                        List<reportPullSms> lstrptpull = utility.BindDataList<reportPullSms>(dt.Tables[0]);
                        objroot.status = 200;
                        objroot.message = "Sucssefull!";
                        objroot.reportpullsms = lstrptpull;
                        //return View(lstrptpull);
                        return Json(objroot, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        List<reportPullSms> lstrptpull = new List<reportPullSms>();
                        objroot.status = 400;
                        objroot.message = "No Inbox Message Availble!!";
                        objroot.reportpullsms = lstrptpull;
                        return Json(objroot, JsonRequestBehavior.AllowGet);
                        ///return Json(lstshortcode, JsonRequestBehavior.AllowGet);  
                    }
                    //return Json(students, JsonRequestBehavior.AllowGet);  
                }
                else
                {
                    TempData["error"] = "session expired! please login again";
                    //return View("loginpullsms");
                    return RedirectToAction("loginpullsms", "Account");
                }

            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "pullSmsReport");
                //TempData["error"] = "Problem while Loading Data!!";
                //return RedirectToAction("logincarnival", "Account");

                List<reportPullSms> lstrptpull = new List<reportPullSms>();
                objroot.status = 400;
                objroot.message = "Problem while Loading Data!!";
                objroot.reportpullsms = lstrptpull;
                return Json(objroot, JsonRequestBehavior.AllowGet);
            }
            //return RedirectToAction("logincarnival", "Account");
        }
        #endregion

        [HttpPost]
        public ActionResult pullSmsReport(FormCollection formalue)
        {
            try
            {
                userPullSms objuser = new userPullSms();
                if (Session["userObject"] != null)
                {
                    objuser = (userPullSms)Session["userObject"];


                    ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 96, "Inbox:GetPrimaryKeyword()::Function starts here.");
                    string query1 = "select distinct(shortcode) from shortcode where username = ?global.username; "; // distinct(PKey),
                    var shortcode_source = new HashSet<string> { };

                    string[] param = new string[] { objuser.pkeyUserName };
                    DataSet ds1 = DL.DL_ExecuteQuery(query1, param);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        ____logconfig.Log_Write(____logconfig.LogLevel.DB, 100, "Inbox:GetPrimaryKeyword()::Query to select from shortcode table:" + query1);
                        ____logconfig.Log_Write(____logconfig.LogLevel.DB, 101, "Inbox:GetPrimaryKeyword()::Result of query to select from shortcode table, no of rows:" + ds1.Tables[0].Rows.Count.ToString());

                        List<ddlshortcode> lstshortcode = utility.BindDataList<ddlshortcode>(ds1.Tables[0]);
                        List<SelectListItem> items = new List<SelectListItem>();

                        foreach (var t in lstshortcode)
                        {
                            SelectListItem s = new SelectListItem();

                            s.Text = t.shortcode;
                            s.Value = t.shortcode;
                            items.Add(s);
                        }
                        ViewBag.source = items;
                    }


                    string shortcode = formalue["source"];
                    string pkey = formalue["pkey"];
                    string skey = formalue["skey"];
                    ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 249, "Inbox:getallreport()::Function starts here.");
                    DataSet dt;
                    string strQuery = "";
                    if (skey == "--------All--------") //DropDownList3.SelectedValue.ToString() 
                    {
                        strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                        string[] arr_values = new string[] { pkey.Trim(), objuser.pkeyUserName, shortcode };
                        dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                    }
                    else
                    {
                        strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and SKey=?global.skey and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                        string[] arr_values = new string[] { pkey.Trim(), skey.Trim(), objuser.pkeyUserName, shortcode };
                        dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                    }
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 264, "Inbox:getallreport()::Query to select from inbox table:" + strQuery);
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 265, "Inbox:getallreport()::Result of query to select from inbox table, no of rows:" + dt.Tables[0].Rows.Count.ToString());
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        List<reportPullSms> lstrptpull = utility.BindDataList<reportPullSms>(dt.Tables[0]);
                        return View(lstrptpull);
                    }
                    else
                    {
                        TempData["error"] = "No Messages In the Inbox.";
                        return RedirectToAction("pullSmsReport", "Report");
                    } 
                }
                else
                {
                    TempData["error"] = "session expired! please login again";
                    return RedirectToAction("loginpullsms", "Account");
                }

            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "pullSmsReport");
                TempData["error"] = "Problem while Loading Data!!";
                return RedirectToAction("pullSmsReport", "Report");
            }
            //return RedirectToAction("logincarnival", "Account");
        }

        #region old code
        //[HttpPost]
        //public ActionResult Carnival(FormCollection formalue)
        //{
        //    string query = "", fromdatefinal = "", todatefinal = ""; string[] paramdata = { "" };
        //    List<carrnival> lstcarrnival = new List<carrnival>();
        //    DataSet ds = new DataSet();

        //    try
        //    {
        //        if (Session["username"] != null)
        //        {
        //            ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 63, "Carnival:getcarnivaldata()::Function starts here.");
        //            string fromdate = formalue["Search-form-Fromdate"];
        //            string todate = formalue["Search-form-Todate"];

        //            if ((fromdate != "Select Date" && fromdate != "") && (todate != "Select Date" && todate != ""))
        //            {
        //                fromdatefinal = fromdate + " 00:00:00";
        //                todatefinal = todate + " 23:59:59";
        //            }
        //            else
        //            {
        //                fromdatefinal = "";
        //                todatefinal = "";
        //            }


        //            string username = formalue["username"];
        //            string mobileno = formalue["Search-form-mobile"];






        //            //if (username != "")
        //            //{
        //            #region binddropdown
        //            string query1 = "SELECT * FROM customer WHERE parentresellername=?global.username; ";
        //            string[] param = new string[] { Session["username"].ToString() };
        //            DataSet ds1 = DL.DL_ExecuteQuery(query1, param);

        //            List<usercarnival> lstcarrnival1 = utility.BindDataList<usercarnival>(ds1.Tables[0]);
        //            List<SelectListItem> items = new List<SelectListItem>();
        //            foreach (var t in lstcarrnival1)
        //            {
        //                SelectListItem s = new SelectListItem();
        //                s.Text = t.UserName;
        //                s.Value = t.UserName;
        //                //if (username == t.UserName)
        //                //{
        //                //    //s.Selected = true;
        //                //    ViewData["username"] = username;
        //                //}
        //                items.Add(s);
        //            }
        //            ViewBag.SelectedOption = username;
        //            ViewBag.username = items;

        //            #endregion


        //            if (fromdatefinal == "" && todatefinal == "" && mobileno != "" && username != "")
        //            {
        //                query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE  cr.`username`=?global.username and cr.`mobileno`=?global.destination;";
        //                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                paramdata = new string[] { username, mobileno };
        //                ds = DL.DL_ExecuteQuery(query, paramdata);
        //                Session["exportmobile"] = ds;
        //            }
        //            else if (fromdatefinal != "" && todatefinal != "" && mobileno == "" && username != "")
        //            {

        //                query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`username`=?global.username AND cr.`senddate`>='" + fromdatefinal + "' AND cr.`senddate`<='" + todatefinal + "' LIMIT 0,1000;";
        //                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                paramdata = new string[] { username };
        //                ds = DL.DL_ExecuteQuery(query, paramdata);


        //                if (ds != null)
        //                {
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        if (ds.Tables[0].Rows.Count < 1)
        //                        {

        //                            Session["exportdata"] = ds;
        //                        }
        //                    }
        //                }
        //                Session["exportmobile"] = null;
        //            }
        //            else if (fromdatefinal != "" && todatefinal != "" && mobileno != "" && username != "")
        //            {
        //                query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`username`=?global.username AND cr.`mobileno`=?global.destination and  cr.`senddate`>='" + fromdatefinal + "' AND cr.`senddate`<='" + todatefinal + "';";
        //                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                paramdata = new string[] { username, mobileno };
        //                ds = DL.DL_ExecuteQuery(query, paramdata);
        //                Session["exportmobile"] = ds;
        //            }
        //            else if (fromdate != "" && todatefinal != "" && mobileno == "" && username == "")
        //            {
        //                query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`senddate`>='" + fromdatefinal + "' AND cr.`senddate`<='" + todatefinal + "' LIMIT 0,1000;";
        //                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                ds = DL.DL_ExecuteSimpleQuery(query);
        //                if (ds != null)
        //                {
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        if (ds.Tables[0].Rows.Count < 1)
        //                        {
        //                            Session["exportdata"] = ds;
        //                        }
        //                    }
        //                }
        //                Session["exportmobile"] = null;
        //            }
        //            else if (fromdate == "" && todatefinal == "" && mobileno != "" && username == "")
        //            {
        //                query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`mobileno`=?global.destination and cr.`senddate`>='" + fromdatefinal + "' AND cr.`senddate`<='" + todatefinal + "';";
        //                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                paramdata = new string[] { mobileno };
        //                ds = DL.DL_ExecuteQuery(query, paramdata);
        //                Session["exportmobile"] = ds;
        //            }
        //            else if (fromdate != "" && todatefinal != "" && mobileno != "" && username == "")
        //            {
        //                query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`mobileno`=?global.destination and cr.`senddate`>='" + fromdatefinal + "' AND cr.`senddate`<='" + todatefinal + "';";
        //                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                paramdata = new string[] { mobileno };
        //                ds = DL.DL_ExecuteQuery(query, paramdata);
        //                Session["exportmobile"] = ds;
        //            }

        //            if (ds != null)
        //            {
        //                lstcarrnival = utility.BindDataList<carrnival>(ds.Tables[0]);
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    if (fromdatefinal != "" && todatefinal != "")
        //                    {
        //                        TempData["fromdate"] = fromdate;
        //                        TempData["todate"] = todate;
        //                        TempData["mobileno"] = mobileno;
        //                        if (ds.Tables[0].Rows.Count < 1000)
        //                        {
        //                            TempData["success"] = "Data Display count::" + ds.Tables[0].Rows.Count;
        //                        }
        //                        else
        //                        {
        //                            TempData["success"] = "Data is very large for selected date range so we are only displaying some rows. To check complete data, Please download report.";
        //                        }


        //                    }
        //                    else if (fromdatefinal == "" && todatefinal == "" && mobileno != "")
        //                    {
        //                        TempData["fromdate"] = fromdate;
        //                        TempData["todate"] = todate;
        //                        TempData["mobileno"] = mobileno;
        //                        TempData["success"] = "";
        //                    }
        //                }
        //                else
        //                {
        //                    TempData["success"] = "No Data To Display!!";
        //                }
        //            }
        //            else
        //            {
        //                TempData["error"] = "Problem while Loading Data!!";
        //            }
        //            return View(lstcarrnival);
        //            //}
        //            //else
        //            //{
        //            //    return RedirectToAction("Carnival", "Report");
        //            //}

        //        }
        //        else
        //        {
        //            TempData["error"] = "Session Expired!!";
        //            return RedirectToAction("logincarnival", "Account");
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "CarnivalReports");
        //        TempData["error"] = "Problem while Loading Data!!";
        //        return View(lstcarrnival);
        //    }
        //}

        //public ActionResult CarnivalSummary()
        //{


        //    try
        //    {
        //        if (Session["username"] != null)
        //        {
        //            ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 63, "Carnival:bind_reseller()::Function starts here.");
        //            string query1 = "SELECT * FROM customer WHERE parentresellername=?global.username; ";
        //            ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query1, "getreseller");
        //            string[] param = new string[] { Session["username"].ToString() };
        //            DataSet ds1 = DL.DL_ExecuteQuery(query1, param);
        //            List<usercarnival> lstcarrnival1 = utility.BindDataList<usercarnival>(ds1.Tables[0]);
        //            List<SelectListItem> items = new List<SelectListItem>();


        //            foreach (var t in lstcarrnival1)
        //            {
        //                SelectListItem s = new SelectListItem();

        //                s.Text = t.UserName;
        //                s.Value = t.UserName;
        //                items.Add(s);
        //            }
        //            ViewBag.username = items;
        //            //TempData["tempuser"] = items;
        //        }
        //        else
        //        {
        //            TempData["error"] = "session expired! please login again";
        //            return RedirectToAction("logincarnival", "Account");
        //        }

        //    }
        //    catch (Exception exc)
        //    {
        //        ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "CarnivalReports");
        //    }
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult CarnivalSummary(FormCollection formalue)
        //{
        //    string[] paramdata = { "" };
        //    List<carnivalSummary> lstCarrnivalSummary = new List<carnivalSummary>();
        //    CultureInfo provider = CultureInfo.InvariantCulture;
        //    try
        //    {
        //        if (Session["username"] != null)
        //        {
        //            ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 63, "Carnival:CarnivalSummary()::Function starts here.");
        //            string strfrom_time = formalue["SearchSummary-form-Fromdate"];
        //            string strto_time = formalue["SearchSummary-form-Todate"];

        //            if (strfrom_time == "--Select Date--" || strfrom_time == "")
        //            {
        //                strfrom_time = DateTime.Now.ToString("yyyy-MM-dd");
        //            }
        //            if (strto_time == "--Select Date--" || strto_time == "")
        //            {
        //                strto_time = DateTime.Now.ToString("yyyy-MM-dd");
        //            }

        //            DateTime From_time = DateTime.ParseExact(strfrom_time, @"yyyy-MM-dd", provider);
        //            string fromdate = From_time.ToString("yyyyMMdd",
        //                   provider);

        //            DateTime to_time = DateTime.ParseExact(strto_time, @"yyyy-MM-dd", provider);
        //            string todate = to_time.ToString("yyyyMMdd",
        //                   provider);

        //            string username = formalue["username"];

        //            #region binddropdown
        //            string query1 = "SELECT * FROM customer WHERE parentresellername=?global.username; ";
        //            string[] param = new string[] { Session["username"].ToString() };
        //            DataSet ds1 = DL.DL_ExecuteQuery(query1, param);

        //            List<usercarnival> lstcarrnival1 = utility.BindDataList<usercarnival>(ds1.Tables[0]);
        //            List<SelectListItem> items = new List<SelectListItem>();
        //            foreach (var t in lstcarrnival1)
        //            {
        //                SelectListItem s = new SelectListItem();
        //                s.Text = t.UserName;
        //                s.Value = t.UserName;
        //                if (username == t.UserName)
        //                {
        //                    s.Selected = true;
        //                    //ViewData["username"] = username;
        //                }
        //                items.Add(s);
        //            }
        //            //ViewBag.SelectedOption = username;
        //            ViewBag.username = items;

        //            #endregion


        //            if (int.Parse(fromdate) > int.Parse(todate))
        //            {
        //                TempData["error"] = "From date can not be greater than to date";
        //            }
        //            else
        //            {
        //                //dict_userdata.Add("TotalSent", sent_count);
        //                //dict_userdata.Add("DELIVRD", delivrdcount);
        //                //dict_userdata.Add("UNDELIVRD", undelivrdcount);
        //                //dict_userdata.Add("DNC", dnccount);
        //                //dict_userdata.Add("EXPIRD", expirdcount);
        //                //dict_userdata.Add("Other", othercount);
        //                //dict_userdata.Add("SMSC", long.Parse(smsccount.ToString()));
        //                //dict_userdata.Add("Invoicecount", invoicecount);
        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("User_Name");
        //                dt.Columns.Add("Date");
        //                dt.Columns.Add("Total_Sent");
        //                dt.Columns.Add("Delivered");
        //                dt.Columns.Add("Undelivered");
        //                dt.Columns.Add("Dnc_Reject");
        //                dt.Columns.Add("Other");
        //                dt.Columns.Add("SMSC");


        //                List<int> all_dates = utility.GetALLDate(fromdate, todate);

        //                int total = 0;
        //                int delivrd = 0;
        //                int undelivrd = 0;
        //                int dnc_reject = 0;
        //                int other = 0;
        //                int smsc = 0;
        //                foreach (int daywisedate in all_dates)
        //                {
        //                    Dictionary<string, long> dict_get_summery = Summary.getsmscount(daywisedate.ToString(), daywisedate.ToString(), username, ConfigurationManager.AppSettings["MysqlConnectionString"].ToString(), 1);
        //                    if (dict_get_summery != null)
        //                    {
        //                        if (dict_get_summery.ContainsKey("TotalSent") && dict_get_summery["TotalSent"] > 0)
        //                        {
        //                            DataRow dr = dt.NewRow();
        //                            dr["User_Name"] = Session["username"].ToString();
        //                            string dd = daywisedate.ToString() + "000000";
        //                            //CultureInfo provider = CultureInfo.InvariantCulture;
        //                            string dateString = daywisedate.ToString();
        //                            string format = "yyyyMMdd";
        //                            DateTime result = DateTime.ParseExact(dateString, format, provider);

        //                            dr["Date"] = result.ToString("yyyy-MM-dd");
        //                            dr["Total_Sent"] = dict_get_summery["TotalSent"];
        //                            dr["Delivered"] = dict_get_summery["DELIVRD"];
        //                            dr["Undelivered"] = dict_get_summery["UNDELIVRD"];
        //                            dr["Dnc_Reject"] = dict_get_summery["DNC"];
        //                            dr["Other"] = dict_get_summery["Other"] + dict_get_summery["EXPIRD"];
        //                            if (dict_get_summery["SMSC"] >= 0)
        //                            {
        //                                dr["SMSC"] = dict_get_summery["SMSC"];
        //                            }
        //                            else
        //                            {
        //                                dr["SMSC"] = 0;
        //                            }

        //                            dt.Rows.Add(dr);

        //                            total += int.Parse(dict_get_summery["TotalSent"].ToString());
        //                            delivrd += int.Parse(dict_get_summery["DELIVRD"].ToString());
        //                            undelivrd += int.Parse(dict_get_summery["UNDELIVRD"].ToString());
        //                            dnc_reject += int.Parse(dict_get_summery["DNC"].ToString());
        //                            other += int.Parse(dict_get_summery["Other"].ToString()) + int.Parse(dict_get_summery["EXPIRD"].ToString());
        //                            smsc += int.Parse(dict_get_summery["SMSC"].ToString());

        //                        }

        //                    }
        //                }

        //                if (dt != null)
        //                {
        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        DataView dv = dt.DefaultView;
        //                        dv.Sort = "Date ASC";
        //                        dt = dv.ToTable();

        //                        DataRow dr1 = dt.NewRow();
        //                        dr1["User_Name"] = "";

        //                        dr1["Date"] = "Total";
        //                        dr1["Total_Sent"] = "" + total.ToString() + "";
        //                        dr1["Delivered"] = "" + delivrd.ToString() + "";
        //                        dr1["Undelivered"] = "" + undelivrd.ToString() + "";
        //                        dr1["Dnc_Reject"] = "" + dnc_reject.ToString() + "";
        //                        dr1["Other"] = "" + other.ToString() + "";
        //                        dr1["SMSC"] = "" + smsc.ToString() + "";

        //                        dt.Rows.Add(dr1);



        //                        lstCarrnivalSummary = utility.BindDataList<carnivalSummary>(dt);
        //                        TempData["fromdate"] = strfrom_time;
        //                        TempData["todate"] = strto_time;
        //                    }
        //                    else
        //                    {
        //                        TempData["success"] = "No Data To Display!!";
        //                        TempData["fromdate"] = strfrom_time;
        //                        TempData["todate"] = strto_time;
        //                    }
        //                }
        //                else
        //                {
        //                    TempData["error"] = "Problem while Loading Data!!";
        //                    TempData["fromdate"] = strfrom_time;
        //                    TempData["todate"] = strto_time;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            TempData["error"] = "Session Expired!!";
        //            return RedirectToAction("logincarnival", "Account");
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        TempData["error"] = "Problem while Loading Data!!";
        //        ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "CarnivalSummary");
        //    }
        //    return View(lstCarrnivalSummary);
        //}

        //public string CreateFastCSVFile(DataTable table, string strFilePath, string downloadurl, string filename)
        //{
        //    const int capacity = 5000000;
        //    const int maxCapacity = 20000000;

        //    List<string> lst = new List<string>();
        //    string finalfilepath = strFilePath + filename;
        //    // ConvertToCSV conv = new ConvertToCSV(downloadpath, Session["username"].ToString(), downloadurl);
        //    //First we will write the headers.
        //    StringBuilder csvBuilder = new StringBuilder(capacity);

        //    csvBuilder.AppendLine(string.Join(",", table.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

        //    // Create the CSV file and write all from StringBuilder
        //    using (var sw = new StreamWriter(finalfilepath, false))
        //    {
        //        foreach (DataRow dr in table.Rows)
        //        {
        //            if (csvBuilder.Capacity >= maxCapacity)
        //            {
        //                sw.Write(csvBuilder.ToString());
        //                csvBuilder = new StringBuilder(capacity);
        //            }


        //            csvBuilder.Append(String.Join(",", dr.ItemArray));
        //            csvBuilder.Append("\n");

        //        }

        //        sw.Write(csvBuilder.ToString());
        //    }

        //    //conv.ZipFileDLR();
        //    string[] filepath = utility.ZipFileDLR(strFilePath, downloadurl, "rsv_carnival");

        //    return filepath[1];
        //}

        //public string export_to_csv(searchresult obj)
        //{
        //    string query = "", fromdatefinal = "", todatefinal = "", downloadpath = "", downloadurl, filename = ""; string[] paramdata = { "" };
        //    string ratval = "";
        //    //  string filePath = @"e:\temp\test.csv";
        //    downloadpath = ConfigurationManager.AppSettings["downloadpath"].ToString();
        //    downloadurl = ConfigurationManager.AppSettings["downloadurl"].ToString();
        //    //@"E:\downloadcarnival\";

        //    if (!Directory.Exists(downloadpath))
        //    {
        //        Directory.CreateDirectory(downloadpath);
        //    }
        //    try
        //    {
        //        if (Session["username"] != null)
        //        {
        //            filename = Session["username"].ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        //            if (Session["exportmobile"] != null)
        //            {

        //                DataSet ds = (DataSet)Session["exportmobile"];
        //                ratval = CreateFastCSVFile(ds.Tables[0], downloadpath, downloadurl, filename);
        //                Session["exportmobile"] = null;
        //                // ratval = "";
        //            }
        //            else
        //            {
        //                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 63, "Carnival:getcarnivaldata()::Function starts here.");
        //                string fromdate = obj.fromdate;//formalue["Search-form-Fromdate"];
        //                string todate = obj.todate;
        //                string mobileno = obj.mobileno;
        //                string username = obj.username;//formalue["Search-form-Todate"];
        //                if ((fromdate != null && fromdate != "") && (todate != null && todate != ""))
        //                {
        //                    fromdatefinal = fromdate + " 00:00:00";
        //                    todatefinal = todate + " 23:59:59";
        //                    if (obj.mobileno.ToString() != "black")
        //                    {
        //                        if (obj.username == null)
        //                        {
        //                            //query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE  cr.`username`=?global.username and cr.`mobileno`=?global.destination;";
        //                            query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`mobileno`=?global.destination and  cr.`senddate`>='" + fromdatefinal + "' AND cr.`senddate`<='" + todatefinal + "';";
        //                            ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                            paramdata = new string[] { obj.mobileno.ToString() };
        //                            DataSet ds = DL.DL_ExecuteQuery(query, paramdata);
        //                            if (ds.Tables[0].Rows.Count > 0)
        //                            {
        //                                ratval = CreateFastCSVFile(ds.Tables[0], downloadpath, downloadurl, filename);
        //                            }
        //                            else
        //                            {
        //                                ratval = "";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`username`=?global.username AND cr.`mobileno`=?global.destination and cr.`senddate`>='" + fromdatefinal + "' AND cr.`senddate`<='" + todatefinal + "';";
        //                            ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                            paramdata = new string[] { obj.username, obj.mobileno.ToString() };
        //                            DataSet ds = DL.DL_ExecuteQuery(query, paramdata);
        //                            if (ds.Tables[0].Rows.Count > 0)
        //                            {
        //                                ratval = CreateFastCSVFile(ds.Tables[0], downloadpath, downloadurl, filename);
        //                            }
        //                            else
        //                            {
        //                                ratval = "";
        //                            }
        //                        }
        //                        //query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE  cr.`username`=?global.username and cr.`mobileno`=?global.destination;";
        //                    }
        //                    else
        //                    {
        //                        if (Session["exportdata"] != null)
        //                        {
        //                            DataSet ds = (DataSet)Session["exportdata"];
        //                            //  CreateFastCSVFile(ds.Tables[0], @"E:\test.csv");
        //                            ratval = CreateFastCSVFile(ds.Tables[0], downloadpath, downloadurl, filename);
        //                        }
        //                        else
        //                        {
        //                            DataSet ds = new DataSet();
        //                            if (obj.username == null)
        //                            {
        //                                query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE  cr.`senddate`>='" + fromdatefinal + "' AND cr.`senddate`<='" + todatefinal + "';";
        //                                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                                ds = DL.DL_ExecuteSimpleQuery(query);
        //                                if (ds.Tables[0].Rows.Count > 0)
        //                                {
        //                                    ratval = CreateFastCSVFile(ds.Tables[0], downloadpath, downloadurl, filename);
        //                                }
        //                                else
        //                                {
        //                                    ratval = "";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`username`=?global.username AND cr.`senddate`>='" + fromdatefinal + "' AND cr.`senddate`<='" + todatefinal + "';";
        //                                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                                paramdata = new string[] { obj.username.ToString() };
        //                                ds = DL.DL_ExecuteQuery(query, paramdata);
        //                            }
        //                            if (ds.Tables[0].Rows.Count > 0)
        //                            {
        //                                ratval = CreateFastCSVFile(ds.Tables[0], downloadpath, downloadurl, filename);

        //                            }
        //                            else
        //                            {
        //                                ratval = "";
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (mobileno != "black")
        //                    {
        //                        if (username == null)
        //                        {
        //                            query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`mobileno`=?global.destination ;";
        //                            ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                            paramdata = new string[] { obj.mobileno.ToString() };

        //                        }
        //                        else
        //                        {
        //                            query = "SELECT cr.`username`,cr.`messageid`,cr.`mobileno`,cr.`message`,cr.`dlrstatus`,cr.`messagecount`,cr.`senddate`,cr.`dlrdate`,cr.`senderid` FROM client_delivery_reports  AS cr WHERE cr.`username`=?global.username and cr.`mobileno`=?global.destination;";
        //                            ____logconfig.Log_Write(____logconfig.LogLevel.DB, 0, "Query :: " + query, "getresellerdata");
        //                            paramdata = new string[] { obj.username, obj.mobileno.ToString() };

        //                        }
        //                        DataSet ds = DL.DL_ExecuteQuery(query, paramdata);
        //                        if (ds.Tables[0].Rows.Count > 0)
        //                        {
        //                            ratval = CreateFastCSVFile(ds.Tables[0], downloadpath, downloadurl, filename);
        //                        }
        //                        else
        //                        {
        //                            ratval = "";
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ratval = "timeout";
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "CarnivalReports");
        //        ratval = "";
        //    }
        //    Session["exportmobile"] = null;
        //    Session["exportdata"] = null;
        //    return ratval;
        //}
        #endregion

        [SessionTimeOut]
        public JsonResult getpkey(string source)
        {
            userPullSms objuser = new userPullSms();
            List<SelectListItem> itempkey = new List<SelectListItem>();

            try
            {
                if (Session["userObject"] != null)
                {
                    objuser = (userPullSms)Session["userObject"];
                    ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 96, "Inbox:GetPrimaryKeyword()::Function starts here.");
                    string query1 = "select PKey from shortcode where UserName=?global.username and shortcode=?global.shortcode";

                    string[] param = new string[] { objuser.pkeyUserName, source };
                    DataSet ds1 = DL.DL_ExecuteQuery(query1, param);
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 510, "Inbox:getpkey()::Query to select from shortcode:" + query1);
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 510, "Inbox:getpkey()::Result of query to select from shortcode, no of rows:" + ds1.Tables[0].Rows.Count.ToString());
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        List<ddlpkey> lstpkey = utility.BindDataList<ddlpkey>(ds1.Tables[0]);
                        itempkey.Add(new SelectListItem { Text = "Select Pkey", Value = "Select Pkey" });
                        foreach (var t in lstpkey)
                        {
                            SelectListItem s = new SelectListItem();

                            s.Text = t.PKey;
                            s.Value = t.PKey;
                            itempkey.Add(s);
                        }
                    }
                    else
                    {
                        itempkey.Add(new SelectListItem { Text = "default", Value = "default" });
                    }
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "getpkey");
            }
         
            
            return Json(new SelectList(itempkey, "Value", "Text"));
        }

        [SessionTimeOut]
        public JsonResult getskey(string pkey)
        {
            userPullSms objuser = new userPullSms();
            objuser = (userPullSms)Session["userObject"];
            List<SelectListItem> itemskey = new List<SelectListItem>();
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 429, "Inbox:getskey ()::Function starts here.");

                string strQuery = "select SKey from keywordmanager where UserName =?global.username and PKey=?global.pkey";
                string[] arr_values = new string[] { objuser.pkeyUserName, pkey };
                DataSet dt1 = DL.DL_ExecuteQuery(strQuery, arr_values);
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 434, "Inbox:getskey()::Query to select from keywordmanager:" + strQuery);
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 435, "Inbox:getskey()::Result of query to select from keywordmanager:" + dt1.Tables[0].Rows.Count.ToString());
                if (dt1.Tables[0].Rows.Count > 0)
                {
                    List<ddlskey> lstskey = utility.BindDataList<ddlskey>(dt1.Tables[0]);
                    itemskey.Add(new SelectListItem { Text = "--------All--------", Value = "--------All--------" });
                    foreach (var t in lstskey)
                    {
                        SelectListItem s = new SelectListItem();

                        s.Text = t.SKey;
                        s.Value = t.SKey; 
                        itemskey.Add(s);
                    }

                }
                else
                {
                    string[] AdminShortCodePKey = ConfigurationManager.AppSettings["AdminPKey-Source"].Split(',')[0].Split('-');
                    if (System.String.Compare(pkey, AdminShortCodePKey[0]) != 0)
                    {
                        itemskey.Add(new SelectListItem { Text = "--------All--------", Value = "--------All--------" });
                        itemskey.Add(new SelectListItem { Text = "default", Value = "default" });
                    }
                    else
                    {
                        itemskey.Add(new SelectListItem { Text = "--------All--------", Value = "--------All--------" });
                        itemskey.Add(new SelectListItem { Text = "default", Value = "default" });
                    }
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "getskey");
            }

            return Json(new SelectList(itemskey, "Value", "Text"));

        }

        public ActionResult changepwd()
        {
            if (Session["userObject"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "session expired! please login again";
                return RedirectToAction("loginpullsms", "Account");
            }
        }

        [HttpPost]
        [SessionTimeOut]
        public ActionResult changepwd(cngpwd objcngpwd)
        {
            userPullSms objuser = new userPullSms();
            resultjson objresult = new resultjson();
            try
            {
                if (Session["userObject"] != null)
                {
                    objuser = (userPullSms)Session["userObject"];

                    ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 249, "Inbox:getallreport()::Function starts here.");
                    string strQuery = @"SELECT pku.`password` FROM `pull_keyword_user` pku WHERE pku.`username`=?global.username and pku.`password`=?global.pwd";
                    string[] arr_values = new string[] { objuser.userName, objcngpwd.oldpwd };
                    
                    DataSet ds = DL.DL_ExecuteQuery(strQuery, arr_values);
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 346, "changepwd::Query to select from pull_keyword_user:" + strQuery);
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 346, "changepwd::result select from pull_keyword_user count:" + ds.Tables[0].Rows.Count);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            
                            if (ds.Tables[0].Rows[0]["password"].ToString().Trim() != objcngpwd.oldpwd)
                            {
                                objresult.status = 400;
                                objresult.message = "Password Incorrect!!";
                                return Json(objresult);
                            }
                            else
                            {
                                if (objcngpwd.newpwd != objcngpwd.confirtpwd)
                                {
                                    objresult.status = 400;
                                    objresult.message = "New Password Not Match With Confirm Password!!";
                                    return Json(objresult);
                                }
                                else
                                {
                                    string strupdate = @"UPDATE pull_keyword_user pku SET pku.`password`=?global.pwd WHERE pku.`username`=?global.username AND pku.`password`=?global.password";
                                    string[] arr_values1 = new string[] { objcngpwd.newpwd, objuser.userName, objcngpwd.oldpwd };
                                    int i = DL.DL_ExecuteNonQuery(strupdate, arr_values1, MSCon.ConnectionString);
                                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 346, "changepwd::Query to update password:" + strupdate);
                                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 346, "changepwd::result update password:" + i);
                                    if (i > 0)
                                    {
                                        objresult.status = 200;
                                        objresult.message = "Password Succsesfully Changed!!";
                                        return Json(objresult, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        objresult.message = "Error!! Please Try again Later";
                                        objresult.status = 400;
                                        return Json(objresult, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }
                        else
                        {
                            objresult.message = "Password Incorrect!!";
                            objresult.status = 400;
                            return Json(objresult, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        objresult.message = "Error!! Please Try again Later";
                        objresult.status = 400;
                        return Json(objresult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //objresult.status = 401;
                    //objresult.message = "session expired! please login again";
                    //return Json(objresult, JsonRequestBehavior.AllowGet);
                    TempData["error"] = "session expired! please login again";
                    return RedirectToAction("loginpullsms", "Account");
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "changepwd");
                objresult.message = "Error!! Please Try again Later";
                objresult.status = 400;
                return Json(objresult, JsonRequestBehavior.AllowGet);
            }
            //return View();
        }


        [HttpPost]
        public ActionResult forgotpwd(FormCollection formvalue)
        {
            if (Session["userObject"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "session expired! please login again";
                return RedirectToAction("loginpullsms", "Account");
            } 
        }


        public ActionResult forgotpwd()
        {
            return View();
        }


        [HttpPost]
        public ActionResult getdatadatewise(FormCollection formvalue)
        {
            try
            {
                string fromdatetime = string.Empty,todatetime=string.Empty;

                 userPullSms objuser = new userPullSms();
                 //rptpull objpull = new rptpull();
                 if (Session["userObject"] != null)
                 {
                     objuser=(userPullSms)Session["userObject"];

                     #region bind Dropdown pkey
                     ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 96, "Inbox:getdatadatewise()::Function starts here.");
                     string query1 = "select distinct(shortcode) from shortcode where username = ?global.username; "; // distinct(PKey),
                     var shortcode_source = new HashSet<string> { };

                     string[] param = new string[] { objuser.pkeyUserName };
                     DataSet ds1 = DL.DL_ExecuteQuery(query1, param);
                     if (ds1.Tables[0].Rows.Count > 0)
                     {
                         ____logconfig.Log_Write(____logconfig.LogLevel.DB, 100, "Inbox:GetPrimaryKeyword()::Query to select from shortcode table:" + query1);
                         ____logconfig.Log_Write(____logconfig.LogLevel.DB, 101, "Inbox:GetPrimaryKeyword()::Result of query to select from shortcode table, no of rows:" + ds1.Tables[0].Rows.Count.ToString());

                         List<ddlshortcode> lstshortcode = utility.BindDataList<ddlshortcode>(ds1.Tables[0]);
                         List<SelectListItem> items = new List<SelectListItem>();

                         foreach (var t in lstshortcode)
                         {
                             SelectListItem s = new SelectListItem();

                             s.Text = t.shortcode;
                             s.Value = t.shortcode;
                             items.Add(s);
                         }
                         ViewBag.source = items;
                         //ViewBag.pkey = itempkey;
                         //ViewBag.skey = itemskey;
                     }
                     #endregion
                     
                     
                     string reporttype = formvalue["rdn-getreport"];
                     string hfjsonvalue= formvalue["hfjson"];

                     
                     rptpull objpull = JsonConvert.DeserializeObject<rptpull>(hfjsonvalue);
                     string shortcode = objpull.source;// formvalue["source"];
                     string pkey = objpull.pkey;//formvalue["pkey"];
                     string skey = objpull.skey;// formvalue["skey"];

                     if (reporttype == "rpt-datewise")
                     {
                         ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 96, "Inbox:getdatadatewise()::DateWise Start Here.");
                         string fromdate = formvalue["SearchSummary-form-Fromdate"];
                         string todate = formvalue["SearchSummary-form-Todate"];

                         if (!string.IsNullOrEmpty(fromdate))
                         {
                             if(string.IsNullOrEmpty(todate))
                             {
                                 fromdatetime = fromdate + "00:00:00";
                                 todatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                 
                                 DataSet dt = new DataSet();
                                  string strQuery = "";
                                  if (skey == "--------All--------") //DropDownList3.SelectedValue.ToString() 
                                  {
                                      strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and (RecDateTime>='" + fromdatetime.ToString() + "') and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                                      string[] arr_values = new string[] { pkey.Trim(), objuser.pkeyUserName, shortcode };
                                      dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                                  }
                                  else
                                  {
                                      strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and SKey=?global.skey and (RecDateTime>='" + fromdatetime.ToString() + "') and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                                      string[] arr_values = new string[] { pkey.Trim(), skey.Trim(), objuser.userName, shortcode };
                                      dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                                  }
                                  ____logconfig.Log_Write(____logconfig.LogLevel.DB, 346, "Inbox:getreport()::Query to select from inbox:" + strQuery);
                                  ____logconfig.Log_Write(____logconfig.LogLevel.DB, 347, "Inbox:getreport()::Result of query to select from inbox, no of rows:" + dt.Tables[0].Rows.Count.ToString());

                                  if (dt.Tables[0].Rows.Count > 0)
                                  {
                                      List<reportPullSms> lstrptpull = utility.BindDataList<reportPullSms>(dt.Tables[0]);
                                      TempData["success"] = "Data Display FromDate::" + fromdatetime + " to ToDate::" + todatetime;
                                      return View("PullSmsReport", lstrptpull);

                                      
                                  }
                                  else
                                  {
                                      TempData["error"] = "No Messages In the Inbox.";
                                      return View("PullSmsReport");
                                  }
                             }
                             else
                             {
                                 fromdatetime = fromdate + "00:00:00";
                                 todatetime = todate + "23:59:59";
                                 DataSet dt = new DataSet();
                                 string strQuery = "";
                                 if (skey == "--------All--------") //DropDownList3.SelectedValue.ToString() 
                                 {
                                     strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and (RecDateTime>='" + fromdatetime.ToString() + "' and RecDateTime<='" + todatetime.ToString() + "') and UserName = binary ?global.username and Source=?global.shortcode";
                                     string[] arr_values = new string[] {pkey.Trim(), objuser.pkeyUserName, shortcode };
                                     dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                                 }
                                 else {
                                     strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?inbx.pkey and SKey=?global.skey and (RecDateTime>='" + fromdatetime.ToString() + "' and RecDateTime<='" + todatetime.ToString() + "') and UserName = ?global.username and Source=?global.shortcode";
                                     string[] arr_values = new string[] { pkey.Trim(), skey.Trim(), objuser.pkeyUserName, shortcode };
                                     dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                                 
                                 }
                                 ____logconfig.Log_Write(____logconfig.LogLevel.DB, 366, "Inbox:getreport()::Query to select from inbox:" + strQuery);
                                 ____logconfig.Log_Write(____logconfig.LogLevel.DB, 367, "Inbox:getreport()::Result of query to select from inbox, no of rows:" + dt.Tables[0].Rows.Count.ToString());

                                 if (dt.Tables[0].Rows.Count > 0)
                                 {
                                     List<reportPullSms> lstrptpull = utility.BindDataList<reportPullSms>(dt.Tables[0]);
                                     TempData["success"] = "Data Display From Date::" + fromdatetime + " to ToDate::" + todatetime;
                                     return View("PullSmsReport", lstrptpull);
                                 }
                                 else
                                 {
                                     TempData["error"] = "No Messages In the Inbox.";
                                     return View("PullSmsReport"); 
                                 }
                             }
                         }
                         else
                         {
                             TempData["error"] = "Please Enter Valid Date!!";
                             return View("PullSmsReport");
                         }
                     }
                     else if (reporttype == "rpt-all")
                     {
                         //____logconfig.Log_Write(____logconfig.LogLevel.INFO, 249, "Inbox:getallreport()::Function starts here.");
                         ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 96, "Inbox:getdatadatewise()::rpt-all Start Here.");
                         DataSet dt;
                         string strQuery = "";
                         if (skey == "--------All--------") //DropDownList3.SelectedValue.ToString() 
                         {
                             strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                             string[] arr_values = new string[] { pkey.Trim(), objuser.pkeyUserName, shortcode };
                             dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                         }
                         else
                         {
                             strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and SKey=?global.skey and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                             string[] arr_values = new string[] { pkey.Trim(), skey.Trim(), objuser.pkeyUserName, shortcode };
                             dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                         }
                         ____logconfig.Log_Write(____logconfig.LogLevel.DB, 264, "Inbox:getallreport()::Query to select from inbox table:" + strQuery);
                         ____logconfig.Log_Write(____logconfig.LogLevel.DB, 265, "Inbox:getallreport()::Result of query to select from inbox table, no of rows:" + dt.Tables[0].Rows.Count.ToString());
                         if (dt.Tables[0].Rows.Count > 0)
                         {
                             List<reportPullSms> lstrptpull = utility.BindDataList<reportPullSms>(dt.Tables[0]);
                             return View("PullSmsReport", lstrptpull);
                         }
                         else
                         {
                             TempData["error"] = "No Messages In the Inbox.";
                             return View("PullSmsReport");
                         }
                     }
                 
                 }
                 else
                 {
                     TempData["error"] = "No Messages In the Inbox.";
                     return View("PullSmsReport");
                 }
            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 868, exc.Message.ToString(), "getdatadatewise");
                TempData["error"] = "Problem while Loading Data!1";
                return View("PullSmsReport");
            }
            return View("PullSmsReport");
        }

        [HttpPost]
        [SessionTimeOut]
        public ActionResult ajaxgetdatadatewise(ajaxdatewisejson objdatewise)
        {
            resultjson1 objroot = new resultjson1();
            try
            {
                string fromdatetime = string.Empty, todatetime = string.Empty;
                if (Session["userObject"] != null)
                {
                    userPullSms objuser = (userPullSms)Session["userObject"];

                    if(objdatewise.reporttype == "rpt-datewise")
                    {
                        ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 96, "Inbox:getdatadatewise()::DateWise Start Here.");
                        string fromdate = objdatewise.fromdate;
                        string todate = objdatewise.todate;

                        if (!string.IsNullOrEmpty(fromdate))
                        {
                            if (string.IsNullOrEmpty(todate))
                            {
                                fromdatetime = fromdate + " 00:00:00";
                                todatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                DataSet dt = new DataSet();
                                string strQuery = "";
                                if (objdatewise.skey == "--------All--------") //DropDownList3.SelectedValue.ToString() 
                                {
                                    strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and (RecDateTime>='" + fromdatetime.ToString() + "') and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                                    string[] arr_values = new string[] { objdatewise.pkey.Trim(), objuser.pkeyUserName, objdatewise.source };
                                    dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                                }
                                else
                                {
                                    strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and SKey=?global.skey and (RecDateTime>='" + fromdatetime.ToString() + "') and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                                    string[] arr_values = new string[] { objdatewise.pkey.Trim(), objdatewise.skey.Trim(), objuser.pkeyUserName, objdatewise.source };
                                    dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                                }
                                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 346, "Inbox:getreport()::Query to select from inbox:" + strQuery);
                                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 347, "Inbox:getreport()::Result of query to select from inbox, no of rows:" + dt.Tables[0].Rows.Count.ToString());

                                if (dt.Tables[0].Rows.Count > 0)
                                {
                                    List<reportPullSms> lstrptpull = utility.BindDataList<reportPullSms>(dt.Tables[0]);
                                    //TempData["success"] = "Data Display FromDate::" + fromdatetime + " to ToDate::" + todatetime;
                                    //return View("PullSmsReport", lstrptpull);

                                    objroot.status = 200;
                                    objroot.message = "Data Display FromDate::" + fromdatetime + " to ToDate::" + todatetime; ;
                                    objroot.reportpullsms = lstrptpull;
                                    //return View(lstrptpull);
                                    return Json(objroot, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    //TempData["error"] = "No Messages In the Inbox.";
                                    //return View("PullSmsReport");
                                    List<reportPullSms> lstrptpull = new List<reportPullSms>();
                                    objroot.status = 400;
                                    objroot.message = "No Messages In the Inbox.";
                                    objroot.reportpullsms = lstrptpull;
                                    //return View(lstrptpull);
                                    return Json(objroot, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                fromdatetime = fromdate + "00:00:00";
                                todatetime = todate + "23:59:59";
                                DataSet dt = new DataSet();
                                string strQuery = "";
                                if (objdatewise.skey == "--------All--------") //DropDownList3.SelectedValue.ToString() 
                                {
                                    strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and (RecDateTime>='" + fromdatetime.ToString() + "' and RecDateTime<='" + todatetime.ToString() + "') and UserName = binary ?global.username and Source=?global.shortcode";
                                    string[] arr_values = new string[] { objdatewise.pkey.Trim(), objuser.pkeyUserName, objdatewise.source };
                                    dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                                }
                                else
                                {
                                    strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and SKey=?global.skey and (RecDateTime>='" + fromdatetime.ToString() + "' and RecDateTime<='" + todatetime.ToString() + "') and UserName = ?global.username and Source=?global.shortcode";
                                    string[] arr_values = new string[] { objdatewise.pkey.Trim(), objdatewise.skey.Trim(), objuser.pkeyUserName, objdatewise.source };
                                    dt = DL.DL_ExecuteQuery(strQuery, arr_values);

                                }
                                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 366, "Inbox:getreport()::Query to select from inbox:" + strQuery);
                                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 367, "Inbox:getreport()::Result of query to select from inbox, no of rows:" + dt.Tables[0].Rows.Count.ToString());

                                if (dt.Tables[0].Rows.Count > 0)
                                {
                                    List<reportPullSms> lstrptpull = utility.BindDataList<reportPullSms>(dt.Tables[0]);
                                   // TempData["success"] = "Data Display From Date::" + fromdatetime + " to ToDate::" + todatetime;
                                    //return View("PullSmsReport", lstrptpull);

                                    objroot.status = 200;
                                    objroot.message = "Data Display From Date::" + fromdatetime + " to ToDate::" + todatetime ;
                                    objroot.reportpullsms = lstrptpull;
                                    return Json(objroot, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    //TempData["error"] = "No Messages In the Inbox.";
                                    //return View("PullSmsReport");

                                    List<reportPullSms> lstrptpull = new List<reportPullSms>();
                                    objroot.status = 400;
                                    objroot.message = "No Messages In the Inbox.";
                                    objroot.reportpullsms = lstrptpull;
                                    return Json(objroot, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else
                        {
                            List<reportPullSms> lstrptpull = new List<reportPullSms>();
                            objroot.status = 400;
                            objroot.message = "Please Enter Valid Date!!";
                            objroot.reportpullsms = lstrptpull;
                            return Json(objroot, JsonRequestBehavior.AllowGet);
                            //TempData["error"] = "Please Enter Valid Date!!";
                            //return View("PullSmsReport");
                        }
                    }
                    else if (objdatewise.reporttype == "rpt-all")
                    {
                        //____logconfig.Log_Write(____logconfig.LogLevel.INFO, 249, "Inbox:getallreport()::Function starts here.");
                        ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 96, "Inbox:getdatadatewise()::rpt-all Start Here.");
                        DataSet dt;
                        string strQuery = "";
                        if (objdatewise.skey == "--------All--------") //DropDownList3.SelectedValue.ToString() 
                        {
                            strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                            string[] arr_values = new string[] { objdatewise.pkey.Trim(), objuser.pkeyUserName, objdatewise.source };
                            dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                        }
                        else
                        {
                            strQuery = "select MsgID,SKey,MobileNo,Message,RecDateTime,Source from inbox where PKey=?global.pkey and SKey=?global.skey and UserName =?global.username and Source=?global.shortcode order by RecDateTime desc";
                            string[] arr_values = new string[] { objdatewise.pkey.Trim(), objdatewise.skey.Trim(), objuser.pkeyUserName, objdatewise.source };
                            dt = DL.DL_ExecuteQuery(strQuery, arr_values);
                        }
                        ____logconfig.Log_Write(____logconfig.LogLevel.DB, 264, "Inbox:getallreport()::Query to select from inbox table:" + strQuery);
                        ____logconfig.Log_Write(____logconfig.LogLevel.DB, 265, "Inbox:getallreport()::Result of query to select from inbox table, no of rows:" + dt.Tables[0].Rows.Count.ToString());
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            List<reportPullSms> lstrptpull = utility.BindDataList<reportPullSms>(dt.Tables[0]);
                            objroot.status = 200;
                            objroot.message = "";
                            objroot.reportpullsms = lstrptpull;
                            return Json(objroot, JsonRequestBehavior.AllowGet);
                            //return View("PullSmsReport", lstrptpull);
                        }
                        else
                        {
                            //TempData["error"] = "No Messages In the Inbox.";
                            //return View("PullSmsReport");
                            List<reportPullSms> lstrptpull = new List<reportPullSms>();
                            objroot.status = 400;
                            objroot.message = "No Messages In the Inbox.";
                            objroot.reportpullsms = lstrptpull;
                            return Json(objroot, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                else
                {
                    TempData["error"] = "session expired! please login again";
                    return RedirectToAction("loginpullsms", "Account");
                }
            }
            catch (Exception exc)
            {
                List<reportPullSms> lstrptpull = new List<reportPullSms>();
                objroot.status = 400;
                objroot.message = "Problem while Loading Data!!";
                objroot.reportpullsms = lstrptpull;
                return Json(objroot, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

    }
}
