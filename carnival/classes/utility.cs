﻿using Ionic.Zip;
using logconfiguration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;

namespace carnival.classes
{
    public class utility
    {

        public static T BindData<T>(DataTable dt)
        {
            var ob = Activator.CreateInstance<T>();
            try
            {
                DataRow dr = dt.Rows[0];

                // Get all columns' name
                List<string> columns = new List<string>();
                foreach (DataColumn dc in dt.Columns)
                {
                    columns.Add(dc.ColumnName);
                }

                // Create object


                // Get all fields
                var fields = typeof(T).GetFields();
                foreach (var fieldInfo in fields)
                {
                    if (columns.Contains(fieldInfo.Name))
                    {
                        // Fill the data into the field
                        //fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                        if (fieldInfo.FieldType == typeof(int))
                        {
                            int i = ExtractInt(dr[fieldInfo.Name]);
                            fieldInfo.SetValue(ob, i);
                        }
                        else
                        {
                            fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                        }
                    }
                }

                // Get all properties
                var properties = typeof(T).GetProperties();
                foreach (var propertyInfo in properties)
                {
                    if (columns.Contains(propertyInfo.Name))
                    {
                        // Fill the data into the property
                        propertyInfo.SetValue(ob, dr[propertyInfo.Name],null);
                    }
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 176, exc.Message.ToString(), "BindData");
            }

            return ob;
        }

        public static int ExtractInt(object data)
        {
            if (data.GetType() == typeof(int))
            {
                return (int)data;
            }
            else
            {
                int i = 0;
                int.TryParse(data + "", out i);
                return i;
            }
        }

        public static List<T> BindDataList<T>(DataTable dt)
        {
            List<T> lst = new List<T>();


            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn dc in dt.Columns)
                {
                    columns.Add(dc.ColumnName);
                }

                var fields = typeof(T).GetFields();
                var properties = typeof(T).GetProperties();



                foreach (DataRow dr in dt.Rows)
                {
                    var ob = Activator.CreateInstance<T>();

                    foreach (var fieldInfo in fields)
                    {
                        if (columns.Contains(fieldInfo.Name))
                        {   
                            fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                        }
                    }

                    foreach (var propertyInfo in properties)
                    {
                        if (columns.Contains(propertyInfo.Name))
                        {
                            //  object a =ob == DBNull.Value ? "" : ob;
                            object a=DBNull.Value.ToString() ;
                          
                          propertyInfo.SetValue(ob == null? a : ob, dr[propertyInfo.Name], null);
                           
                            
                        }
                    }

                    lst.Add(ob);
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 176, exc.Message.ToString(), "BindDataList");
            }
            return lst;
        }

        public static void converzip()
        {
            FileStream sourceFile = File.OpenRead(@"e:\test.csv");
            FileStream destFile = File.Create(@"e:\test.zip");
            GZipStream compStream = new GZipStream(destFile, CompressionMode.Compress);
            try
            {
                int theByte = sourceFile.ReadByte();
                while (theByte != -1)
                {
                    compStream.WriteByte((byte)theByte);
                    theByte = sourceFile.ReadByte();
                }
            }
            finally
            {
                compStream.Dispose();
            }
        }

        public static List<int> GetALLDate(string fromdate, string todate)
        {
            int num_startdate = int.Parse(fromdate);
            int num_enddate = int.Parse(todate);
            CultureInfo provider = CultureInfo.InvariantCulture;
            string format = "yyyyMMdd";
            DateTime start_date = DateTime.ParseExact(fromdate.ToString(), format, provider);
            DateTime ebd_date = DateTime.ParseExact(todate.ToString(), format, provider);

            int days = ebd_date.Subtract(start_date).Days;
            List<int> all_datees = new List<int>();
            for (int i = 0; i <= days; i++)
            {
                all_datees.Add(int.Parse(ebd_date.AddDays(-i).ToString("yyyyMMdd")));
            }
            return all_datees;
        }

        //public static string[] ZipFileDLR(string downloadpath,string downloadurl,string _username)
        //{
        //    ZipFile zip=new ZipFile(); 
        //    string[] file = null;
        //    DirectoryInfo dir = new DirectoryInfo(downloadpath);
        //    FileInfo[] fileinf = dir.GetFiles();
        //    List<string> list = new List<string>();
        //    if ((fileinf != null) && (fileinf.Length > 0))
        //    {
        //        foreach (FileInfo files in fileinf)
        //        {
        //            if (files.Name.EndsWith(".xls"))
        //            {
        //                list.Add(files.FullName);
        //            }
        //        }
        //        zip.AddFiles(list, _username);
        //        zip.Save(downloadpath + _username + ".zip");
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            File.Delete(list[i]);
        //        }
        //        FileInfo f = new FileInfo(downloadpath + _username + ".zip");
        //        file = new string[3];
        //        file[0] = f.Name;
        //        file[1] = downloadurl + f.Name;
        //        if (f.Length > (1024 * 1024))
        //        {
        //            float fl = ((float)(f.Length) / (float)(1024 * 1024));
        //            file[2] = Math.Round(fl, MidpointRounding.ToEven).ToString() + "MB";
        //        }
        //        else
        //        {
        //            float fl = ((float)(f.Length) / (float)(1024));
        //            file[2] = Math.Round(fl, MidpointRounding.ToEven).ToString() + "KB";
        //        }
        //        return file;

        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public static string[] ZipFileDLR(string downloadpath, string downloadurl, string _username)
        {
            ZipFile zip = new ZipFile(); 
            string[] file = null;
            DirectoryInfo dir = new DirectoryInfo(downloadpath);
            FileInfo[] fileinf = dir.GetFiles();
            List<string> list = new List<string>();
            if ((fileinf != null) && (fileinf.Length > 0))
            {
                foreach (FileInfo files in fileinf)
                {
                    if (files.Name.EndsWith(".csv"))
                    {
                        list.Add(files.FullName);
                    }
                }
                zip.AddFiles(list, _username);
                zip.Save(downloadpath + _username + ".zip");
                for (int i = 0; i < list.Count; i++)
                {
                    File.Delete(list[i]);
                }
                FileInfo f = new FileInfo(downloadpath + _username + ".zip");
                file = new string[3];
                file[0] = f.Name;
                file[1] = downloadurl + f.Name;
                if (f.Length > (1024 * 1024))
                {
                    float fl = ((float)(f.Length) / (float)(1024 * 1024));
                    file[2] = Math.Round(fl, MidpointRounding.ToEven).ToString() + "MB";
                }
                else
                {
                    float fl = ((float)(f.Length) / (float)(1024));
                    file[2] = Math.Round(fl, MidpointRounding.ToEven).ToString() + "KB";
                }
                return file;

            }
            else
            {
                return null;
            }
        }

    }
}