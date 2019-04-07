using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using Ionic.Zip;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using logconfiguration;
using System.IO.Compression;

namespace web
{
    public class ConvertToCSV
    {
        private string _savepath;
        private ZipFile zip;
        private DownloadPathDetails dpd;
        private string _username;
        private int _filenamepostfix;
        private string _filename;
        private string _zippath;
        public ConvertToCSV(string savepath, string username,string downloadpathurl)
        {
            _filename = username;
            _filenamepostfix = 1;
            _username = username;
            _savepath = savepath;
            zip = new ZipFile();
            dpd = new DownloadPathDetails(_savepath, downloadpathurl);
            _zippath = _savepath + username + ".zip";
        }
        public void WriteToFile(string content)
        {
            try
            {
                if (File.Exists(_savepath + _filename + ".csv"))
                {
                    string[] arr_file = null;

                    if (_filename.Contains("_"))
                        arr_file = _filename.Split('_');
                    else
                    {
                        arr_file = new string[1];
                        arr_file[0] = _filename;
                    }

                    _filename = arr_file[0] + "_" + _filenamepostfix;
                    _filenamepostfix += 1;
                }

                using (FileStream fs = new FileStream(_savepath + _filename + ".csv", FileMode.Append, FileAccess.Write, FileShare.Read, 8, true))
                {
                    byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(content);
                    fs.BeginWrite(bt, 0, bt.Length, OnwriteCompleted, fs);
                }
            }
            catch
            {
            }
        }

        public void SyncWriteToFile(string content)
        {
            try
            {
                if (!Directory.Exists(dpd.Path))
                    Directory.CreateDirectory(dpd.Path);

                if (File.Exists(_savepath + _filename + ".csv"))
                {
                    string[] arr_file = null;

                    if (_filename.Contains("_"))
                        arr_file = _filename.Split('_');
                    else
                    {
                        arr_file = new string[1];
                        arr_file[0] = _filename;
                    }

                    _filename = arr_file[0] + "_" + _filenamepostfix;
                    _filenamepostfix += 1;
                }

                using (FileStream fs = new FileStream(_savepath + _filename + ".csv", FileMode.Append, FileAccess.Write, FileShare.Read, 8, true))
                {
                    byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(content);
                    fs.Write(bt, 0, bt.Length);
                }
            }
            catch
            {
            }
        }
        

        public void WriteToFile(StringBuilder sb)
        {
            try
            {
                if (Directory.Exists(_savepath))
                    Directory.Delete(_savepath);

                if (File.Exists(_savepath + _filename + ".csv"))
                {
                    string[] arr_file = null;

                    if (_filename.Contains("_"))
                        arr_file = _filename.Split('_');
                    else
                        arr_file[0] = _filename;

                    _filename = arr_file[0] + "_" + _filenamepostfix;
                    _filenamepostfix += 1;
                }

                using (FileStream fs = new FileStream(_savepath + _filename + ".csv", FileMode.Append, FileAccess.Write, FileShare.Read, 8, true))
                {
                    byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(sb.ToString());
                    fs.BeginWrite(bt, 0, bt.Length, OnwriteCompleted, fs);
                }
            }
            catch
            {
            }
        }
        public void OnwriteCompleted(IAsyncResult ar)
        {
            FileStream fs = null;
            try
            {
                fs = (FileStream)ar.AsyncState;
                fs.EndWrite(ar);
            }
            catch (Exception ex)
            {
                //WriteErrLog logs = new WriteErrLog("DownloadWrite.txt");
                //logs.WriteError_File(ex.ToString());
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 140, ex.Message.ToString(), "ExcelHelper");
            }
        }
        public string[] ZipFileDLR()
        {
            string[] file = null;
            DirectoryInfo dir = new DirectoryInfo(@dpd.Path);
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
                zip.Save(dpd.Path + _username + ".zip");
                for (int i = 0; i < list.Count; i++)
                {
                    File.Delete(list[i]);
                }
                FileInfo f = new FileInfo(dpd.Path + _username + ".zip");
                file = new string[3];
                file[0] = f.Name;
                file[1] = dpd.Url + f.Name;
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
    public class DownloadPathDetails
    {
        private string _path;
        private string _url;

        public string Path
        {
            get { return this._path; }
            set { this._path = value; }
        }
        public string Url
        {
            get { return this._url; }
            set { this._url = value; }
        }

        public DownloadPathDetails(string path, string url)
        {
            this.Path = path;
            this.Url = url;
        }
            
    }
    public class ExcelHelperForDLR
    {
        //Row limits older excel verion per sheet, the row limit for excel 2003 is 65536
        const int rowLimit = 65000;
        public static DownloadPathDetails dpd;
        
        private string getWorkbookTemplate()
        {
            var sb = new StringBuilder(818);
            sb.AppendFormat(@"<?xml version=""1.0""?>{0}", Environment.NewLine);
            sb.AppendFormat(@"<?mso-application progid=""Excel.Sheet""?>{0}", Environment.NewLine);
            sb.AppendFormat(@"<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);
            sb.AppendFormat(@" xmlns:o=""urn:schemas-microsoft-com:office:office""{0}", Environment.NewLine);
            sb.AppendFormat(@" xmlns:x=""urn:schemas-microsoft-com:office:excel""{0}", Environment.NewLine);
            sb.AppendFormat(@" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);
            sb.AppendFormat(@" xmlns:html=""http://www.w3.org/TR/REC-html40"">{0}", Environment.NewLine);
            sb.AppendFormat(@" <Styles>{0}", Environment.NewLine);
            sb.AppendFormat(@"  <Style ss:ID=""Default"" ss:Name=""Normal"">{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Alignment ss:Vertical=""Bottom""/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Borders/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Interior/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <NumberFormat/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Protection/>{0}", Environment.NewLine);
            sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
            sb.AppendFormat(@"  <Style ss:ID=""s62"">{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""{0}", Environment.NewLine);
            sb.AppendFormat(@"    ss:Bold=""1""/>{0}", Environment.NewLine);
            sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
            sb.AppendFormat(@"  <Style ss:ID=""s63"">{0}", Environment.NewLine);
            sb.AppendFormat(@"   <NumberFormat ss:Format=""Short Date""/>{0}", Environment.NewLine);
            sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
            sb.AppendFormat(@" </Styles>{0}", Environment.NewLine);
            return sb.ToString();
        }

        private string replaceXmlChar(string input)
        {
            input = input.Replace("&", "&amp");
            input = input.Replace("<", "&lt;");
            input = input.Replace(">", "&gt;");
            input = input.Replace("\"", "&quot;");
            input = input.Replace("'", "&apos;");
            return input;
        }

        public string[] ToExcel(DataSet dsInput, string filename, string downloadpath, string downloadurl)
        {
            try
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.INFO, 188, "ExcelHelperForDLR:ToExcel()::Function starts here.");
                dpd = new DownloadPathDetails(downloadpath, downloadurl);
                //sample path: "D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\"
                //sample url : "http://downloads.vivaconnect.in/download/DLR/"
                System.IO.Directory.CreateDirectory(@dpd.Path);
                if (File.Exists(dpd.Path + filename))
                    File.Delete(dpd.Path + filename);

                GetExcelXml(dsInput, filename);
                string fullpath = dpd.Path + filename;

                string zipPath = fullpath.Substring(0, fullpath.Length - 4);
                using (ZipFile zip = new ZipFile())
                {
                    ZipEntry zip_entry = zip.AddFile(zipPath + ".xls");
                    zip_entry.FileName = filename;
                    zip.Save(zipPath + ".zip");
                }
                if (File.Exists(dpd.Path + filename))
                    File.Delete(dpd.Path + filename);

                FileInfo f = new FileInfo(zipPath + ".zip");
                string[] file = new string[3];
                file[0] = f.Name;
                file[1] = dpd.Url + f.Name;
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
            catch (Exception exc)
            {
                //new Logs("exportexcel.txt").asyncWrite(exc.Message.ToString());
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 229, exc.Message.ToString(), "ExcelHelperForDLR");
                return null;
            }

        }

        private string getCell(Type type, object cellData)
        {
            var data = (cellData is DBNull) ? "" : cellData;
            if (type.Name.Contains("Int") || type.Name.Contains("Double") || type.Name.Contains("Decimal")) return string.Format("<Cell><Data ss:Type=\"Number\">{0}</Data></Cell>", data);
            if (type.Name.Contains("Date") && data.ToString() != string.Empty)
            {
                return string.Format("<Cell ss:StyleID=\"s63\"><Data ss:Type=\"DateTime\">{0}</Data></Cell>", Convert.ToDateTime(data).ToString("yyyy-MM-dd"));
            }
            return string.Format("<Cell><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(data.ToString()));
        }
        private string getWorksheets(DataSet source,string str_filename)
        {
            StringBuilder sw = new StringBuilder();
            if (source == null || source.Tables.Count == 0)
            {
                sw.Append("<Worksheet ss:Name=\"Sheet1\">\r\n<Table>\r\n<Row><Cell><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");
                File.AppendAllText(dpd.Path + str_filename, sw.ToString());
                return sw.ToString();
            }
            foreach (DataTable dt in source.Tables)
            {
                if (dt.Rows.Count == 0)
                {
                    sw.Append("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + "\">\r\n<Table>\r\n<Row><Cell  ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");
                    File.AppendAllText(dpd.Path + str_filename, sw.ToString());
                }
                else
                {
                    //write each row data                
                    var sheetCount = 0;
                    int k=0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        if ((i % rowLimit) == 0)
                        {
                            //add close tags for previous sheet of the same data table
                            if ((i / rowLimit) > sheetCount)
                            {
                                sw.Append("\r\n</Table>\r\n</Worksheet>");
                                sheetCount = (i / rowLimit);
                            }
                            sw.Append("\r\n<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) +
                                     (((i / rowLimit) == 0) ? "" : Convert.ToString(i / rowLimit)) + "\">\r\n<Table>");
                            //write column name row
                            sw.Append("\r\n<Row>");
                            foreach (DataColumn dc in dt.Columns)
                                sw.Append(string.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dc.ColumnName)));
                            sw.Append("</Row>");
                        }
                        sw.Append("\r\n<Row>");
                        foreach (DataColumn dc in dt.Columns)
                            sw.Append(getCell(dc.DataType, dt.Rows[i][dc.ColumnName]));
                        sw.Append("</Row>");
                        if (i == k)
                        {
                            File.AppendAllText(dpd.Path + str_filename, sw.ToString());
                            sw.Remove(0, sw.Length);
                            k = k + 10000;
                        }
                        
                    }
                    sw.Append("\r\n</Table>\r\n</Worksheet>");
                    File.AppendAllText(dpd.Path + str_filename, sw.ToString());
                    
                }
            }

            return sw.ToString();
        }
        public string GetExcelXml(DataTable dtInput, string filename)
        {
            var excelTemplate = getWorkbookTemplate();
            var ds = new DataSet();
            ds.Tables.Add(dtInput.Copy());
            var worksheets = getWorksheets(ds, filename);
            var excelXml = string.Format(excelTemplate, worksheets);
            return excelXml;
        }

        public void GetExcelXml(DataSet dsInput, string filename)
        {
            var excelTemplate = getWorkbookTemplate();
            File.AppendAllText(dpd.Path + filename, excelTemplate);
            var worksheets = getWorksheets(dsInput, filename);
            File.AppendAllText(dpd.Path + filename, "\r\n</Workbook>");
        }

        public string[] ToExcel(DataSet dsInput, string filename, HttpResponse response,string downloadpath,string downloadurl)
        {
            try
            {
                dpd = new DownloadPathDetails(downloadpath, downloadurl);
                //sample path: "D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\"
                //sample url : "http://downloads.vivaconnect.in/download/DLR/"
                System.IO.Directory.CreateDirectory(@dpd.Path);
                if (File.Exists(dpd.Path + filename))
                    File.Delete(dpd.Path + filename);

                GetExcelXml(dsInput, filename);
                string fullpath = dpd.Path + filename;

                string zipPath = fullpath.Substring(0, fullpath.Length - 4);
                using (ZipFile zip = new ZipFile())
                {
                    ZipEntry zip_entry = zip.AddFile(zipPath + ".xls");
                    zip_entry.FileName = filename;
                    zip.Save(zipPath + ".zip");
                }
                if (File.Exists(dpd.Path + filename))
                    File.Delete(dpd.Path + filename);

                FileInfo f = new FileInfo(zipPath + ".zip");
                string[] file = new string[3];
                file[0] = f.Name;
                file[1] = dpd.Url + f.Name;
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
            catch (Exception exc)
            {
                //new WriteErrLog().WriteError_File(exc.Message.ToString(), "exportDLR", "ToExcel()");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 388, exc.Message.ToString(), "ExcelHelper");
                return null;
            }

        }
        //*********************Added by yogi on 24-03-2010*******************
        //Do not delete the below commented code. 
        //*******************************************************************
        //public string[] ToExcel(DataSet dsInput, string filename, HttpResponse response)
        //{
        //    try
        //    {

        //        System.IO.Directory.CreateDirectory(@"D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\");
        //        if (File.Exists("D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename))
        //            File.Delete("D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename);

        //        GetExcelXml(dsInput, filename);
        //        string fullpath = "D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename;
                
        //        string zipPath = fullpath.Substring(0, fullpath.Length - 4);
        //        using (ZipFile zip = new ZipFile())
        //        {
        //            ZipEntry zip_entry = zip.AddFile(zipPath + ".xls");
        //            zip_entry.FileName = filename;
        //            zip.Save(zipPath + ".zip");
        //        }
        //        if (File.Exists("D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename))
        //            File.Delete("D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename);

        //        FileInfo f = new FileInfo(zipPath + ".zip");
        //        string[] file = new string[3];
        //        file[0] = f.Name;
        //        file[1] = "http://downloads.vivaconnect.in/download/DLR/" + f.Name;
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
        //    catch (Exception exc)
        //    {
        //        new WriteErrLog().WriteError_File(exc.Message.ToString(), "exportDLR", "ToExcel()");
        //        return null;
        //    }

        //}
        
        
    }

    public class ExcelHelperForDLRReports
    {
        //Row limits older excel verion per sheet, the row limit for excel 2003 is 65536
        const int rowLimit = 65000;
        public static DownloadPathDetails dpd;
        private ZipFile zip;        
        private int i;
        private string _username;
        private string _filename;
        public ExcelHelperForDLRReports(string username,string downloadpath,string downloadurl)
        {
            zip = new ZipFile();
            i = 0;
            dpd = new DownloadPathDetails(downloadpath, downloadurl);
            _username = username;
            _filename = username;
        }

        private string getWorkbookTemplateDLR()
        {
            var sb = new StringBuilder(818);
            sb.AppendFormat(@"<?xml version=""1.0""?>{0}", Environment.NewLine);
            sb.AppendFormat(@"<?mso-application progid=""Excel.Sheet""?>{0}", Environment.NewLine);
            sb.AppendFormat(@"<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);
            sb.AppendFormat(@" xmlns:o=""urn:schemas-microsoft-com:office:office""{0}", Environment.NewLine);
            sb.AppendFormat(@" xmlns:x=""urn:schemas-microsoft-com:office:excel""{0}", Environment.NewLine);
            sb.AppendFormat(@" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);
            sb.AppendFormat(@" xmlns:html=""http://www.w3.org/TR/REC-html40"">{0}", Environment.NewLine);
            sb.AppendFormat(@" <Styles>{0}", Environment.NewLine);
            sb.AppendFormat(@"  <Style ss:ID=""Default"" ss:Name=""Normal"">{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Alignment ss:Vertical=""Bottom""/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Borders/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Interior/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <NumberFormat/>{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Protection/>{0}", Environment.NewLine);
            sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
            sb.AppendFormat(@"  <Style ss:ID=""s62"">{0}", Environment.NewLine);
            sb.AppendFormat(@"   <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""{0}", Environment.NewLine);
            sb.AppendFormat(@"    ss:Bold=""1""/>{0}", Environment.NewLine);
            sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
            sb.AppendFormat(@"  <Style ss:ID=""s63"">{0}", Environment.NewLine);
            sb.AppendFormat(@"   <NumberFormat ss:Format=""Short Date""/>{0}", Environment.NewLine);
            sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
            sb.AppendFormat(@" </Styles>{0}", Environment.NewLine);
            return sb.ToString();
        }

        private string replaceXmlCharDLR(string input)
        {
            input = input.Replace("&", "&amp");
            input = input.Replace("<", "&lt;");
            input = input.Replace(">", "&gt;");
            input = input.Replace("\"", "&quot;");
            input = input.Replace("'", "&apos;");
            return input;
        }

        private string getCellDLR(Type type, object cellData)
        {
            var data = (cellData is DBNull) ? "" : cellData;
            if (type.Name.Contains("Int") || type.Name.Contains("Double") || type.Name.Contains("Decimal")) return string.Format("<Cell><Data ss:Type=\"Number\">{0}</Data></Cell>", data);
            if (type.Name.Contains("Date") && data.ToString() != string.Empty)
            {
                return string.Format("<Cell ss:StyleID=\"s63\"><Data ss:Type=\"DateTime\">{0}</Data></Cell>", Convert.ToDateTime(data).ToString("yyyy-MM-dd"));
            }
            return string.Format("<Cell><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlCharDLR(data.ToString()));
        }
        private string getWorksheetsDLR(DataSet source, string str_filename)
        {
            StringBuilder sw = new StringBuilder();
            if (source == null || source.Tables.Count == 0)
            {
                sw.Append("<Worksheet ss:Name=\"Sheet1\">\r\n<Table>\r\n<Row><Cell><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");
                File.AppendAllText(dpd.Path + str_filename, sw.ToString());
                return sw.ToString();
            } 
            foreach (DataTable dt in source.Tables)
            {
                if (dt.Rows.Count == 0)
                {
                    sw.Append("<Worksheet ss:Name=\"" + replaceXmlCharDLR(dt.TableName) + "\">\r\n<Table>\r\n<Row><Cell  ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");
                    File.AppendAllText(dpd.Path + str_filename, sw.ToString());
                }
                else
                {
                    //write each row data                
                    var sheetCount = 0;
                    int k = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if ((i % rowLimit) == 0)
                        {
                            //add close tags for previous sheet of the same data table
                            if ((i / rowLimit) > sheetCount)
                            {
                                sw.Append("\r\n</Table>\r\n</Worksheet>");
                                sheetCount = (i / rowLimit);
                            }
                            sw.Append("\r\n<Worksheet ss:Name=\"" + replaceXmlCharDLR(dt.TableName) +
                                     (((i / rowLimit) == 0) ? "" : Convert.ToString(i / rowLimit)) + "\">\r\n<Table>");
                            //write column name row
                            sw.Append("\r\n<Row>");
                            foreach (DataColumn dc in dt.Columns)
                                sw.Append(string.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlCharDLR(dc.ColumnName)));
                            sw.Append("</Row>");
                        }
                        sw.Append("\r\n<Row>");
                        foreach (DataColumn dc in dt.Columns)
                            sw.Append(getCellDLR(dc.DataType, dt.Rows[i][dc.ColumnName]));
                        sw.Append("</Row>");
                        if (i == k)
                        {
                            File.AppendAllText(dpd.Path + str_filename, sw.ToString());
                            sw.Remove(0, sw.Length);
                            k = k + 10000;
                        }

                    }
                    sw.Append("\r\n</Table>\r\n</Worksheet>");
                    File.AppendAllText(dpd.Path + str_filename, sw.ToString());

                }
            }

            return sw.ToString();
        }
        public string GetExcelXmlDLR(DataTable dtInput, string filename)
        {
            var excelTemplate = getWorkbookTemplateDLR();
            var ds = new DataSet();
            ds.Tables.Add(dtInput.Copy());
            var worksheets = getWorksheetsDLR(ds, filename);
            var excelXml = string.Format(excelTemplate, worksheets);
            return excelXml;
        }

        public void GetExcelXmlDLR(DataSet dsInput, string filename)
        {
            var excelTemplate = getWorkbookTemplateDLR();
            File.AppendAllText(dpd.Path + filename, excelTemplate);
            var worksheets = getWorksheetsDLR(dsInput, filename);
            File.AppendAllText(dpd.Path + filename, "\r\n</Workbook>");
        }

        public string ToExcelDLR(DataSet dsInput)
        {
            try
            {
                //string oldzippedfile = (filename.Substring(0, filename.Length - 4)) + ".zip";                
                //if (File.Exists(dpd.Path + oldzippedfile))
                //    File.Delete(dpd.Path + oldzippedfile);

                //sample path: "D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\"
                //sample url : "http://downloads.vivaconnect.in/download/DLR/"
                if (!Directory.Exists(dpd.Path))
                    Directory.CreateDirectory(dpd.Path);

                if (File.Exists(dpd.Path + _username + ".zip"))
                    File.Delete(dpd.Path + _username + ".zip");

                if (File.Exists(dpd.Path + _filename + ".xls"))
                {
                    string[] arr_names = _filename.Split('_');
                    _filename = arr_names[0] + "_" + i.ToString();
                    i++;
                }

                GetExcelXmlDLR(dsInput, _filename + ".xls");
                string fullpath = dpd.Path + _filename + ".xls";

                string zipPath = fullpath.Substring(0, fullpath.Length - 4);
                return zipPath;


            }
            catch (Exception exc)
            {
                //new WriteErrLog().WriteError_File(exc.Message.ToString(), "exportDLR", "ToExcel()");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 626, exc.Message.ToString(), "ExcelHelper");
                return null;
            }

        }


        public void converzip()
        {
            FileStream sourceFile = File.OpenRead(@"e:\test.csv");
            FileStream destFile = File.Create(@"C:\sample.zip");
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
        public string[] ZipFileDLR()
        {
            string[] file = null;
            DirectoryInfo dir = new DirectoryInfo(@dpd.Path);
            FileInfo[] fileinf = dir.GetFiles();
            List<string> list = new List<string>();
            if ((fileinf != null) && (fileinf.Length > 0))
            {
                foreach (FileInfo files in fileinf)
                {
                    if (files.Name.EndsWith(".xls"))
                    {                        
                        list.Add(files.FullName);
                    }
                }
                zip.AddFiles(list, _username);
                zip.Save(dpd.Path + _username + ".zip");
                for (int i = 0; i < list.Count; i++)
                {
                    File.Delete(list[i]);
                }
                FileInfo f = new FileInfo(dpd.Path + _username + ".zip");
                file = new string[3];
                file[0] = f.Name;
                file[1] = dpd.Url + f.Name;
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

        //*********************Added by yogi on 24-03-2010*******************
        //Do not delete the below commented code. 
        //*******************************************************************
        //public string[] ToExcel(DataSet dsInput, string filename, HttpResponse response)
        //{
        //    try
        //    {

        //        System.IO.Directory.CreateDirectory(@"D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\");
        //        if (File.Exists("D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename))
        //            File.Delete("D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename);

        //        GetExcelXml(dsInput, filename);
        //        string fullpath = "D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename;

        //        string zipPath = fullpath.Substring(0, fullpath.Length - 4);
        //        using (ZipFile zip = new ZipFile())
        //        {
        //            ZipEntry zip_entry = zip.AddFile(zipPath + ".xls");
        //            zip_entry.FileName = filename;
        //            zip.Save(zipPath + ".zip");
        //        }
        //        if (File.Exists("D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename))
        //            File.Delete("D:\\Websites\\http\\dbsmsapiorg\\downloads.vivaconnect.in\\download\\DLR\\" + filename);

        //        FileInfo f = new FileInfo(zipPath + ".zip");
        //        string[] file = new string[3];
        //        file[0] = f.Name;
        //        file[1] = "http://downloads.vivaconnect.in/download/DLR/" + f.Name;
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
        //    catch (Exception exc)
        //    {
        //        new WriteErrLog().WriteError_File(exc.Message.ToString(), "exportDLR", "ToExcel()");
        //        return null;
        //    }

        //}

      
       

    }     //Class Added by Gautam. Customized ExcelHelperForDLR.
}
