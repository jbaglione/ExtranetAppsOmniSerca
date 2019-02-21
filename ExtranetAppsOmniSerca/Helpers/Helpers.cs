using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using NLog;
using System.Text;

namespace ExtranetAppsOmniSerca.Helpers
{
    
    public static class StrinfRowExtensions
	{
        public static string AnsiToFormatedDate(this string date)
		{
			if (date == "") return "";

			if (date.Contains('-'))
			{
				date = date.Replace("-", "");
			}

			string año = date.Substring(0, 4);
			string mes = date.Substring(4, 2);
            if (date.Length > 6)
            {
                string dia = date.Substring(6, 2);
                return dia + "/" + mes + "/" + año;
            }
            else
            {
                return mes + "/" + año;
            }

			
		}
		public static long FormatedDateToAnsi(this string date, int longitud = 6)
		{
            string año = "";
            string mes = "";
            string dia = "";
            try
            {
                if (date == "") return 0;

                date = date.Replace("-", "").Replace("/", "");

                if (date.Length > 6)
                {
                    año = date.Substring(4, 4);
                    mes = date.Substring(2, 2);
                    if (longitud == 6)
                        dia = date.Substring(0, 2);

                    return long.Parse(año + mes + dia);
                }
                else
                {
                    if (date.Length == 4)
                    {
                        int añoN;
                        añoN = Convert.ToInt32(date.Substring(2, 2));
                        año = (2000 + añoN).ToString();
                    }
                    else
                    {
                        año = date.Substring(2, 4);
                    }
                    mes = date.Substring(0, 2);
                    if (longitud == 6)
                        dia = "01";

                    return long.Parse(año + mes + dia);
                }
            }
            catch (Exception ex)
            {
                throw new Exception (
                    string.Format("FormatedDateToAnsi, date: {0}, longitud: {1}, año: {2}, mes: {3} , dia: {4}, Exeption: {5}",
                                        date, longitud, año, mes, dia, ex.Message));
            }
		}
	}
    public class Helpers
    {
        public string formatDate(string date)
        {
            if (date == "") return "";

            if (date.Contains('-'))
            {
                date = date.Replace("-", "");
            }
        
            string año = date.Substring(0, 4);
            string mes = date.Substring(4, 2);
            string dia = date.Substring(6, 2);

            return dia + "/" + mes + "/" + año;

        }

        public string StringEscape(string str)
        {
            return Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%_]",
                delegate(Match match)
                {
                    string v = match.Value;
                    switch (v)
                    {
                        case "\x00":            // ASCII NUL (0x00) character
                            return "\\0";
                        case "\b":              // BACKSPACE character
                            return "\\b";
                        case "\n":              // NEWLINE (linefeed) character
                            return "\\n";
                        case "\r":              // CARRIAGE RETURN character
                            return "\\r";
                        case "\t":              // TAB
                            return "\\t";
                        case "\u001A":          // Ctrl-Z
                            return "\\Z";
                        default:
                            return "\\" + v;
                    }
                });
        }

    }

    public static class Helper
    {
        public static string encodingBtoa(string toEncode)
        {
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            string toReturn = Convert.ToBase64String(bytes);
            return toReturn;
        }

        //public static string decodingAtob(string toDecode)
        //{
        //    var str = "eyJpc3MiOiJodHRwczovL2lkZW50aXR5LXN0YWdpbmcuYXNjZW5kLnh5eiIsImF1ZCI6Imh0dHBzOi8vaWRlbnRpdHktc3RhZ2luZy5hc2NlbmQueHl6L3Jlc291cmNlcyIsImNsaWVudF9pZCI6IjY5OTRBNEE4LTBFNjUtNEZFRC1BODJCLUM2ODRBMEREMTc1OCIsInNjb3BlIjpbIm9wZW5pZCIsInByb2ZpbGUiLCJzdWIucmVhZCIsImRhdGEud3JpdGUiLCJkYXRhLnJlYWQiLCJhbGcuZXhlY3V0ZSJdLCJzdWIiOiIzNzdjMDk1Yi03ODNiLTQ3ZTctOTdiMS01YWVkOThjMDM4ZmMiLCJhbXIiOiJleHRlcm5hbCIsImF1dGhfdGltZSI6MTQwNzYxNTUwNywiaWRwIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMDg0MGM3NjAtNmY3Yi00NTU2LWIzMzctOGMwOTBlMmQ0NThkLyIsIm5hbWUiOiJwa3NAYXNjZW5kLnh5eiIsImV4cCI6MTQwNzgzNjcxMSwibmJmIjoxNDA3ODMzMTExfQ";
        //    int mod4 = str.Length % 4;
        //    if (mod4 > 0)
        //    {
        //        str += new string('=', 4 - mod4);
        //    }
        //    return str;
        //}

        public static string decodingAtob(string toDecode)
        {
            //string base64Encoded = "YmFzZTY0IGVuY29kZWQgc3RyaW5n";
            string base64Decoded;
            byte[] data = Convert.FromBase64String(toDecode);
            base64Decoded = Encoding.Default.GetString(data);
            return base64Decoded;
        }


        public enum ImageFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.unknown;
        }

    }


    public class Data
    {
        public static DataSet GetDataSetByPrefixes(DataSet ds, List<string> listPrefixes)
        {
            DataSet dsSplit = new DataSet();

            foreach (string prefix in listPrefixes)
            {
                DataTable dt = GetTableByPrefix(ds.Tables[0], prefix);
                if (dt != null)
                    dsSplit.Tables.Add(dt);
            }
            return dsSplit;
        }

        public static DataTable GetTableByPrefix(DataTable table, string prefix)
        {
            string[] cols = (from dc in table.Columns.Cast<DataColumn>()
                             where dc.ColumnName.Contains(prefix)
                             select dc.ColumnName)
                .ToArray();

            if (cols.Count() != 0)
            {
                DataView view = new DataView(table);
                DataTable selected = view.ToTable(prefix, false, cols);

                foreach (DataColumn column in selected.Columns)
                    column.ColumnName = column.ColumnName.Replace(prefix, "");

                return selected;
            }
            else
                return null;
        }
    }
    public static class DataRowExtensions
	{
		public static string GetNulleableDBString(this DataRow row, string field)
		{
			if (row[field] is DBNull)
				return "";

			return row[field].ToString();

		}
	}
	public static class Config
	{
		public static string MailServer
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings["MailServer"];
			}
		}
		public static string MailAddress
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings["MailAddress"];
			}
		}
		public static string MailAccount
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings["MailAccount"];
			}
		}
		public static string MailPassword
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings["MailPassword"];
			}
		}
        public static string ServerFileSystem
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ServerFileSystem"];
            }
        }
    }

    public static class NumbersHelpers
    {
        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
}