using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using logconfiguration;


namespace web
{

    public class MSC
    {
        public enum CallerType
        {
            LANDLINE = 1,
            MOBILE = 2
        }
        public enum CallerOrigin
        {
            DOMESTIC = 1,
            INTERNATIONAL = 2
        }

        public CallerType _caller_type;
        public CallerOrigin _caller_origin;
        public MSC_LandLine _mSC_LandLine_Info;
        public MSC_Mobile _mSC_Mobile_Info;
        public string _dst_number;

        public MSC()
        { }

        public MSC(int msc_code)
        {

        }

        public void getMSC_Information(string destination)
        {
            try
            {

                long destination_long;
                string destination_str;
                int destination_length;
                this._dst_number = destination;
                if (long.TryParse(destination, out destination_long))
                {
                    destination_str = destination_long.ToString();
                    destination_length = destination_str.Length;

                    if (destination.Length == 14 && destination.StartsWith("00") && destination.Substring(2, 2) != "91")
                    {
                        this._mSC_LandLine_Info = null;
                        this._mSC_Mobile_Info = new MSC_Mobile(0, "UNKNOWN", "UNKNOWN", "UNKNOWN", "UNKNOWN", 0, "UNKNOWN", 0, "UNKNOWN");
                        this._caller_origin = CallerOrigin.INTERNATIONAL;
                        this._caller_type = CallerType.MOBILE;

                    }
                    else if (destination.Length == 13 && destination.StartsWith("0") && destination.Substring(1, 2) != "91")
                    {
                        this._mSC_LandLine_Info = null;
                        this._mSC_Mobile_Info = new MSC_Mobile(0, "UNKNOWN", "UNKNOWN", "UNKNOWN", "UNKNOWN", 0, "UNKNOWN", 0, "UNKNOWN");
                        this._caller_origin = CallerOrigin.INTERNATIONAL;
                        this._caller_type = CallerType.MOBILE;

                    }
                    else if (destination.Length == 12 && destination.StartsWith("00"))
                    {
                        this._mSC_LandLine_Info = null;
                        this._mSC_Mobile_Info = new MSC_Mobile(0, "UNKNOWN", "UNKNOWN", "UNKNOWN", "UNKNOWN", 0, "UNKNOWN", 0, "UNKNOWN");
                        this._caller_origin = CallerOrigin.INTERNATIONAL;
                        this._caller_type = CallerType.MOBILE;
                    }
                    else if (destination_length <= 12)
                    {
                        if (destination_length == 10)
                        {
                            string msc_code = destination_str.Substring(0, 5);
                            get_msc_code_information(msc_code);
                        }
                        else if (destination_length == 12 && destination_str.StartsWith("91"))
                        {
                            string msc_code = destination_str.Substring(2, 5);
                            get_msc_code_information(msc_code);
                        }
                        else //for if (destination_length == 12 && destination_str.StartsWith("91"))
                        {
                            this._mSC_LandLine_Info = null;
                            this._mSC_Mobile_Info = new MSC_Mobile(0, "UNKNOWN", "UNKNOWN", "UNKNOWN", "UNKNOWN", 0, "UNKNOWN", 0, "UNKNOWN");
                            this._caller_origin = CallerOrigin.INTERNATIONAL;
                            this._caller_type = CallerType.MOBILE;
                        }
                    }
                    else
                    {
                        this._mSC_LandLine_Info = null;
                        this._mSC_Mobile_Info = new MSC_Mobile(0, "UNKNOWN", "UNKNOWN", "UNKNOWN", "UNKNOWN", 0, "UNKNOWN", 0, "UNKNOWN");
                        this._caller_origin = CallerOrigin.INTERNATIONAL;
                        this._caller_type = CallerType.MOBILE;
                    }

                }
                else
                {
                    throw new MSCException("error:number contains illeagal characters");
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, 0, "::MSC.cs::MSC::getMSC_Information():: --" + exc.ToString());
            }

        }



        public void get_msc_code_information(string msc_code)
        {
            try
            {
                string strQuery = "use dst;select dmc.dst_msc_id,dmc.dst_msc_circle_type,dmc.dst_msc_cdma_gsm,dmc.dst_msc_access_code,dmc.dst_msc_series,dmc.dst_operator_id,dod.dst_operator_name,dmc.dst_circle_id,dcd.dst_circle_name,dmc.dst_msc_code_yr_allotment " +
                                                     "  from dst_msc_codes dmc, " +
                                                     "  dst_circle_details dcd, " +
                                                     "  dst_operator_details dod" +
                                                     "  where " +
                                                     "  dcd.dst_circle_id = dmc.dst_circle_id and " +
                                                     "  dod.dst_operator_id = dmc.dst_operator_id and" +
                                                     " (dst_msc_code_0 = " + msc_code + " or dmc.dst_msc_code_1 = " + msc_code + " or dmc.dst_msc_code_2 = " + msc_code + " or dmc.dst_msc_code_3 = " + msc_code + " or dmc.dst_msc_code_4 = " + msc_code + " or dmc.dst_msc_code_5 = " + msc_code + " or dmc.dst_msc_code_6 = " + msc_code + " or dmc.dst_msc_code_7 = " + msc_code + " or dmc.dst_msc_code_8 = " + msc_code + " or dmc.dst_msc_code_9 = " + msc_code + ");";

                DataTable dt = DL.DL_ExecuteSimpleQuery(strQuery).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {

                    long dST_msc_id = Convert.ToInt64(dt.Rows[0]["dst_msc_id"]);
                    string dST_msc_circle_type = Convert.ToString(dt.Rows[0]["dst_msc_circle_type"]);
                    string dST_msc_cdma_gsm = Convert.ToString(dt.Rows[0]["dst_msc_cdma_gsm"]);
                    string dST_msc_access_code = Convert.ToString(dt.Rows[0]["dst_msc_access_code"]);
                    string dST_msc_series = Convert.ToString(dt.Rows[0]["dst_msc_series"]);
                    string dST_operator_name = Convert.ToString(dt.Rows[0]["dst_operator_name"]);
                    string dST_circle_name = Convert.ToString(dt.Rows[0]["dst_circle_name"]);
                    long dST_operator_id = Convert.ToInt64(dt.Rows[0]["dst_operator_id"]);
                    long dST_circle_id = Convert.ToInt64(dt.Rows[0]["dst_circle_id"]);

                    this._mSC_Mobile_Info = new MSC_Mobile(dST_msc_id, dST_msc_circle_type, dST_msc_cdma_gsm, dST_msc_access_code, dST_msc_series, dST_operator_id, dST_operator_name, dST_circle_id, dST_circle_name);
                    this._mSC_LandLine_Info = null;
                    this._caller_type = CallerType.MOBILE;
                    this._caller_origin = CallerOrigin.DOMESTIC;


                }
                else if (dt != null && dt.Rows.Count == 0)
                {
                    int std_start_length = 2;
                    string std_code = msc_code.Substring(0, std_start_length);
                    bool flag_found_landline = false;
                    while (!perform_landline_detection(std_code, out flag_found_landline) && std_start_length != 5)
                    {
                        std_code = msc_code.Substring(0, ++std_start_length);
                    }
                    if (!flag_found_landline)
                    {
                        this._mSC_LandLine_Info = null;
                        this._mSC_Mobile_Info = new MSC_Mobile(0, "UNKNOWN", "UNKNOWN", "UNKNOWN", "UNKNOWN", 0, "UNKNOWN", 0, "UNKNOWN");
                        this._caller_origin = CallerOrigin.INTERNATIONAL;
                        this._caller_type = CallerType.MOBILE;
                    }

                }
                else
                {
                    throw new MSCException("error:database returning null dataset");
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, 0, "::MSC.cs::MSC::get_msc_code_information():: --" + exc.ToString());
            }
        }


        public bool perform_landline_detection(string std_code, out bool flag_found_landline)
        {
            try
            {
                string strQuery = "use dst;select dst_std_id,dst_circle_id, dst_std_service_area_name, dst_std_service_area, dst_std_lcda_name, dst_std_sdca_name, dst_std_sdca_code, dst_std_isactive " +
                                       "from dst.dst_std_code dsc where dst_std_sdca_code = " + std_code + ";";
                DataTable dt = DL.DL_ExecuteSimpleQuery(strQuery).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {


                    long dST_std_id = Convert.ToInt64(dt.Rows[0]["dst_std_id"]);
                    long dST_circle_id = Convert.ToInt64(dt.Rows[0]["dst_circle_id"]);

                    string dST_std_service_area_name = Convert.ToString(dt.Rows[0]["dst_std_service_area_name"]);
                    string dST_std_service_area = Convert.ToString(dt.Rows[0]["dst_std_service_area"]);
                    string dts_std_lcda_name = Convert.ToString(dt.Rows[0]["dst_std_lcda_name"]);
                    string dts_std_sdca_name = Convert.ToString(dt.Rows[0]["dst_std_sdca_name"]);
                    int dST_std_sdca_code = Convert.ToInt32(dt.Rows[0]["dst_std_sdca_code"]);
                    string dst_landline_number = "";
                    string dst_operator_name = "UNKNOWN";

                    if (std_code.Length == 2)
                    {
                        dst_landline_number = this._dst_number.Substring(2, this._dst_number.Length - 2);
                        dst_operator_name = get_landline_operator_name(dst_landline_number, dST_std_sdca_code);
                    }
                    else if (std_code.Length == 3)
                    {
                        dst_landline_number = this._dst_number.Substring(3, this._dst_number.Length - 3);
                        dst_operator_name = get_landline_operator_name(dst_landline_number, dST_std_sdca_code);
                    }
                    else if (std_code.Length == 4)
                    {
                        dst_landline_number = this._dst_number.Substring(4, this._dst_number.Length - 4);
                        dst_operator_name = get_landline_operator_name(dst_landline_number, dST_std_sdca_code);
                    }

                    this._mSC_Mobile_Info = null;
                    this._mSC_LandLine_Info = new MSC_LandLine(dST_std_id, dST_circle_id, dST_std_service_area, dST_std_service_area_name, dts_std_lcda_name, dts_std_sdca_name, dst_operator_name, dST_std_sdca_code);
                    this._caller_origin = CallerOrigin.DOMESTIC;
                    this._caller_type = CallerType.LANDLINE;

                    flag_found_landline = true;
                    return true;
                }
                else if (dt != null && dt.Rows.Count == 0)
                {
                    flag_found_landline = false;
                    return false;
                }
                else
                {
                    flag_found_landline = false;
                    throw new MSCException("error:database returning null dataset");
                }

            }
            catch (Exception exc)
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, 0, "::MSC.cs::MSC::get_msc_code_information():: --" + exc.ToString());
                flag_found_landline = false;
                return false;
            }

        }
        public string get_landline_operator_name(string dst_landline_number, int dST_std_sdca_code)
        {
            string dst_operator_name = "UNKNWON";

            if (dst_landline_number.StartsWith("793"))
                dst_operator_name = "MTS";
            else if (dst_landline_number.StartsWith("791") || dst_landline_number.StartsWith("792"))
                dst_operator_name = "Videocon";
            else if (dst_landline_number.StartsWith("71"))
                dst_operator_name = "VODAFONE";
            else if (dst_landline_number.StartsWith("6"))
                dst_operator_name = "TATA Indicom";
            else if (dst_landline_number.StartsWith("5"))
                dst_operator_name = "HFCL/MTS";
            else if (dst_landline_number.StartsWith("4"))
                dst_operator_name = "AIRTEL";
            else if (dst_landline_number.StartsWith("2") && (dST_std_sdca_code == 11 || dST_std_sdca_code == 22))
                dst_operator_name = "MTNL";
            else if (dst_landline_number.StartsWith("2") && (dST_std_sdca_code != 11 || dST_std_sdca_code != 22))
                dst_operator_name = "BSNL";
            else if (dst_landline_number.StartsWith("1"))
                dst_operator_name = "SPECIAL";

            return dst_operator_name;

        }
    }


    public class MSC_LandLine
    {
        public long _dST_std_id;
        public long _dST_circle_id;
        public string _dST_std_service_area;
        public string _dST_std_service_area_name;
        public string _dST_std_lcda_name;
        public string _dST_std_sdca_name;

        public string _dST_std_operator_name;
        public int _dST_std_sdca_code;

        public MSC_LandLine(long dST_std_id, long dST_circle_id, string dST_std_service_area, string dST_std_service_area_name, string dST_std_lcda_name, string dST_std_sdca_name, string dST_std_operator_name, int dST_std_sdca_code)
        {
            this._dST_std_id = dST_std_id;
            this._dST_std_sdca_code = dST_std_sdca_code;
            this._dST_std_service_area = dST_std_service_area;
            this._dST_std_lcda_name = dST_std_lcda_name;
            this._dST_std_sdca_name = dST_std_sdca_name;
            this._dST_std_operator_name = dST_std_operator_name;
            this._dST_circle_id = dST_circle_id;
            this._dST_std_service_area_name = dST_std_service_area_name;

        }

    }

    public class MSC_Mobile
    {

        public long _dST_msc_id;
        public string _dST_msc_circle_type;
        public string _dST_msc_cdma_gsm;
        public string _dST_msc_access_code;
        public string _dST_msc_series;
        public long _dST_operator_id;
        public string _dST_operator_name;
        public long _dST_circle_id;
        public string _dST_circle_name;

        public MSC_Mobile(long dST_msc_id, string dST_msc_circle_type, string dST_msc_cdma_gsm, string dST_msc_access_code, string dST_msc_series, long dST_operator_id, string dST_operator_name, long dST_circle_id, string dST_circle_name)
        {
            this._dST_msc_id = dST_msc_id;
            this._dST_msc_circle_type = dST_msc_circle_type;
            this._dST_msc_cdma_gsm = dST_msc_cdma_gsm;
            this._dST_msc_access_code = dST_msc_access_code;
            this._dST_msc_series = dST_msc_series;
            this._dST_operator_id = dST_operator_id;
            this._dST_operator_name = dST_operator_name;
            this._dST_circle_id = dST_circle_id;
            this._dST_circle_name = dST_circle_name;

        }
    }


    class MSCException : Exception
    {
        #region Constructors

        public MSCException()
            : base()
        {
        }

        public MSCException(string Message)
            : base(Message)
        {
        }

        public MSCException(string Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }

        #endregion
    }



}