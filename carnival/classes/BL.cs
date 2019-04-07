using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using logconfiguration;

namespace web
{
    public class BL
    {

        public DataSet GetGroupList(string unm)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 18, "BL:GetGroupList()::Function starts here.");
                //string sqlstmt = "select g.GroupName, g.GroupId,count(c.contacts) as No.of Contacts from comgt c,groups g where c.GroupID=g.GroupID and g.UserName=?global.username group by g.Groupname";

                string str_getgroupid = "select GroupID from groups where UserName=?global.username;";
                string[] arr_groupid = new string[] { unm };
                DataSet ds_groupid = DL.DL_ExecuteQuery(str_getgroupid, arr_groupid);
                List<string> lstgroupid = new List<string>();
                if (ds_groupid != null && ds_groupid.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr_groupid in ds_groupid.Tables[0].Rows)
                    {
                        lstgroupid.Add(dr_groupid["GroupID"].ToString());
                    }

                }
                DataSet dr;
                if (lstgroupid.Count > 0)
                {
                    //string sqlstmt = "select g.GroupName, g.GroupId,count(c.contacts) as Contacts from comgt c,groups g where c.GroupID=g.GroupID and g.UserName=?global.username group by g.Groupname";
                    string sqlstmt = "select g.GroupName, g.GroupId,count(c.cid) as Contacts from comgt c  inner join groups g on c.GroupID=g.GroupID where g.GroupId in (" + string.Join(",", lstgroupid) + ") group by g.GroupId;";
                    string[] arr = new string[] { unm };
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 22, "BL:GetGroupList()::Query to select group details:" + sqlstmt);
                    dr = DL.DL_ExecuteSimpleQuery(sqlstmt);
                    ____logconfig.Log_Write(____logconfig.LogLevel.DB, 24, "BL:GetGroupList()::Result of query to select group details, no of rows:" + dr.Tables[0].Rows.Count.ToString());
                }
                else
                { dr = null; }


                return dr;
            }
            catch (Exception ex)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 29, ex.Message.ToString(), "BL");
                return null;
            }
        }



        public DataSet GetFilterValue(string cellval)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 40, "BL:GetFilterValue()::Function starts here.");
                string sqlstmt = "select * from comgt where GroupId=?grps.groupid";
                string[] arr = new string[] { cellval };
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 43, "BL:GetFilterValue()::Query to select from comgt:" + sqlstmt);
                DataSet dr = DL.DL_ExecuteQuery(sqlstmt, arr);
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 45, "BL:GetFilterValue()::Result of query to select from comgt, no of rows:" + dr.Tables[0].Rows.Count.ToString());
                return dr;
            }
            catch (Exception ex)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 50, ex.Message.ToString(), "BL");
                return null;
            }

        }



        public DataSet GetAllGroup(System.Web.UI.WebControls.ListItem listItem)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 62, "BL:GetAllGroup()::Function starts here.");
                string sqlstmt = "select * from comgt where GroupName=?global.username";
                string[] arr = new string[] { listItem.ToString() };
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 65, "BL:GetAllGroup()::Query to select from comgt:" + sqlstmt);
                DataSet dr = DL.DL_ExecuteQuery(sqlstmt, arr);
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 67, "BL:GetAllGroup()::Result of query to select from comgt, no of rows:" + dr.Tables[0].Rows.Count.ToString());
                return dr;
            }
            catch (Exception ex)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 72, ex.Message.ToString(), "BL");
                return null;
            }
        }

        public DataSet GetSubCont(int grval)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 82, "BL:GetSubCont()::Function starts here.");
                string sqlstmt = "select contacts from comgt where GroupId=?comgt.groupid";
                string[] arr_val = { grval.ToString() };
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 85, "BL:GetSubCont()::query to select contacts from comgt:" + sqlstmt);
                DataSet ds = DL.DL_ExecuteQuery(sqlstmt, arr_val);
                ____logconfig.Log_Write(____logconfig.LogLevel.DB, 87, "BL:GetSubCont()::Result of query to select contacts from comgt, no of rows:" + ds.Tables[0].Rows.Count.ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 99, ex.Message.ToString(), "BL");
                return null;
            }

        }


    }

}

