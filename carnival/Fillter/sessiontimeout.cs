using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pullsms_reports.Fillter
{
    public sealed class SessionTimeOut : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                HttpSessionStateBase objHttpSessionStateBase = filterContext.HttpContext.Session;
                var userSession = objHttpSessionStateBase["userObject"];
                if (((userSession == null) && (!objHttpSessionStateBase.IsNewSession)) || (objHttpSessionStateBase.IsNewSession))
                {
                    objHttpSessionStateBase.RemoveAll();
                    objHttpSessionStateBase.Clear();
                    objHttpSessionStateBase.Abandon();
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.StatusCode = 403;
                        filterContext.Result = new JsonResult { Data = "LogOut" };
                    }
                }
            }
        }
    }
}