//using carnival.Models;
using pullsms_reports.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web;

namespace pullsms_reports.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Login/

        public ActionResult loginpullsms()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult loginpullsms(FormCollection fromvalue)
        {
            try
            {
                userPullSms objuser = new userPullSms();
                //usercarnival objuser = new usercarnival();
                objuser.userName = fromvalue["login-form-username"];
                objuser.password = fromvalue["login-form-password"];
                if ((objuser.userName !=null && objuser.userName!="") &&(objuser.password != null && objuser.password!=""))
                {
                    //string getReseller = "select * from user_masterhelo where UserName =?global.username and Passkey=?global.pwd"; //and AccountType='Reseller'
                    //string getReseller = "select * from user_master where UserName =?global.username and Passkey=?global.pwd and AccountType='Reseller'"; //and AccountType='Reseller'
                    string getpullsms_user = @"SELECT * FROM `swiftsmsdb`.`pull_keyword_user` pku WHERE pku.`username`=?global.username AND pku.`password`=?global.pwd AND pku.`isactive`=1;";
                    string[] param = new string[] { objuser.userName, objuser.password };
                    DataSet dspullsms_user = DL.DL_ExecuteQuery(getpullsms_user, param);
                    if (dspullsms_user != null)
                    {
                        if (dspullsms_user.Tables[0].Rows.Count > 0)
                        {
                            //Session["userObject"] = dspullsms_user.Tables[0].Rows[0]["pkey_username"];

                            objuser.pkeyUserName = dspullsms_user.Tables[0].Rows[0]["pkey_username"].ToString();
                            objuser.userId = Convert.ToInt64(dspullsms_user.Tables[0].Rows[0]["id"]);
                            objuser.isActive = Convert.ToInt16(dspullsms_user.Tables[0].Rows[0]["isactive"]);
                            Session["username"] = objuser.userName;
                            Session["userObject"] = objuser;
                            Session.Timeout = 10;

                            return RedirectToAction("pullSmsReport", "Report");
                            //return RedirectToAction("Carnival", "Report");
                        }
                        else
                        {
                            TempData["error"] = "Username or password Not Valid";
                            return RedirectToAction("loginpullsms", "Account");
                        }
                    }
                    else
                    {
                        TempData["error"] = "Error Loading Data!!";
                        return RedirectToAction("loginpullsms", "Account");
                    }
                }
                else
                {
                    TempData["error"] = "Please Enter Username and Password!!";
                    return RedirectToAction("loginpullsms", "Account");
                }
            }
            catch (Exception exc)
            {
                TempData["error"] = "Error Loading Data!!";
                return RedirectToAction("loginpullsms", "Account");
            }
        }

        public ActionResult logout()
        {
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("loginpullsms", "Account");
        }
    }
}
