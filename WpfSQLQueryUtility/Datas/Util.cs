using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Data;
using System.Web;
using System.Configuration;
using System.Reflection;

namespace WpfSQLQueryUtility.Datas
{
	/// <summary>
	/// Util 的摘要描述。
	/// </summary>
	public class Util
	{
		const string _strSource = "CDC_DOH ASP.NET Runtime";//事件來源
		const string _strLog = "CDC_DOH ASP.NET Application";//事件紀錄檔
		
		public Util()
		{
		}

        public static T Cast<T>(object obj, T type)
        {
            return (T)obj;
        }

        public static string GetExecutePath()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return Path.GetDirectoryName(assembly.Location);
        }
		/// <summary>
		/// get month from string yyymm
		/// </summary>
		/// <param name="yearMonth"></param>
		/// <returns></returns>
		public static int GetMonthFromTwYM(string yearMonth)
		{
			return Convert.ToInt16(GetStrMonthFromTwYm(yearMonth));
		}

		/// <summary>
		/// get year from string yyymm
		/// </summary>
		/// <param name="yearMonth"></param>
		/// <returns></returns>
		public static int GetYearFromTwYm(string yearMonth)
		{
			return Convert.ToInt16(GetStrYearFromTwYm(yearMonth))+1911;
		}

		public static String GetStrYearFromTwYm(string yearMonth)
		{
			return yearMonth.Substring(0,3);
		}

		public static String GetStrMonthFromTwYm(string yearMonth)
		{
			return yearMonth.Substring(3,2);
		}


		public static string ShowDateTimeNoOnly(System.DateTime dateTime)
		{
			return dateTime.ToString("yyyyMMdd");
		}


		/// <summary>
		/// check date is valid or not
		/// </summary>
		/// <param name="twDateTime"></param>
		/// <returns></returns>
		public static bool IsValidDate(string twDateTime)
		{
			try
			{
				TwDateString2Gregorian(twDateTime);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// transfer string of tw format(yyymmdd) to datetime
		/// </summary>
		/// <param name="twDateTime"></param>
		/// <returns></returns>
		public static DateTime TwDateString2Gregorian(string twDateTime)
		{
			// marked by wr for SPR-028 
			//string[] TwDate;
			//char[] Sep={'/'};
			//TwDate=twDateTime.Split(Sep,3);

			string year=twDateTime.Substring(0,3);
			string month=twDateTime.Substring(3,2);
			string day=twDateTime.Substring(5,2);

			///return new System.DateTime(int.Parse(TwDate[0])+1911,int.Parse(TwDate[1]),int.Parse(TwDate[2]),12,0,0);
			return new System.DateTime(int.Parse(year)+1911,int.Parse(month),int.Parse(day),12,0,0);
		}

		  

		/// <summary>
		/// transfer string of tw format(yyymmdd) to string
		/// </summary>
		/// <param name="twDateTime"></param>
		/// <returns></returns>
		public static string TwDateString2GregorianNoTime(string twDateTime)
		{
			// marked by wr for SPR-028 
			//string[] TwDate;
			//char[] Sep={'/'};
			//TwDate=twDateTime.Split(Sep,3);

			string year=twDateTime.Substring(0,3);
			string month=twDateTime.Substring(3,2);
			string day=twDateTime.Substring(5,2);

			int year2 = int.Parse(year)+1911;

			///return new System.DateTime(int.Parse(TwDate[0])+1911,int.Parse(TwDate[1]),int.Parse(TwDate[2]),12,0,0);
			return year2.ToString() + "/" + month + "/" + day;
		}

		public static string TwDateWithBrackets(string twDateTime)
		{
			// marked by wr for SPR-028 
			//string[] TwDate;
			//char[] Sep={'/'};
			//TwDate=twDateTime.Split(Sep,3);

			string year=twDateTime.Substring(0,3);
			string month=twDateTime.Substring(3,2);
			string day=twDateTime.Substring(5,2);

			///return new System.DateTime(int.Parse(TwDate[0])+1911,int.Parse(TwDate[1]),int.Parse(TwDate[2]),12,0,0);
			return year + "/" + month + "/" + day;
		}

		/// <summary>
		/// show yyymmdd format datetime, and yyy begin of 1911 
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ShowTWDateTime(System.DateTime dateTime)
		{
			//return dateTime.AddYears(-1911).ToString("yyy/MM/dd");
			return dateTime.AddYears(-1911).ToString("yyyMMdd");
		}

		/// <summary>
		/// show yyy/mm/dd format datetime, and yyy begin of 1911 
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ShowStdTWDateTime(System.DateTime dateTime)
		{
			return dateTime.AddYears(-1911).ToString("yyy/MM/dd");
		}
        
		/// <summary>
		/// show yyyy/mm/dd format datetime
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ShowStdDateTime(System.DateTime dateTime)
		{
			return dateTime.ToString("yyyy/MM/dd");
		}

		/// <summary>
		/// translate yyymmdd format to yyy/mm/dd format
		/// </summary>
		/// <param name="inDate"></param>
		/// <returns></returns>
		public static string Trans2StdTwDateTime(string inDate)
		{
			return ShowStdTWDateTime(TwDateString2Gregorian(inDate));
		}

		/// <summary>
		/// translate yyymmdd format to yyyy/mm/dd format
		/// </summary>
		/// <param name="inDate"></param>
		/// <returns></returns>
		public static string Trans2StdDateTime(string inDate)
		{
			return ShowStdDateTime(TwDateString2Gregorian(inDate));
		}

		/// <summary>
		/// format int to string
		/// </summary>
		/// <param name="number"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string ShowStringOfNo(int number,int length)
		{
			string StrNumber=number.ToString();
			int StrNumberLen=StrNumber.Length;

			if(StrNumberLen>length)
			{
				return StrNumber.Substring(StrNumberLen-length,length);
			}

			for(int i=0;i<length-StrNumberLen;i++)
			{
				StrNumber="0"+StrNumber;
			}

			return StrNumber;
		}

		public static string ShowCurrency(int amount)
		{
			string s= amount.ToString("C");
			return s.Substring(3,s.Length-6);
		}


		//checks to see if user-entered value is a valid number

		public static bool IsNumeric(string value2check)
		{
			try
			{
				if (Convert.ToInt32(value2check)>0)
					return true;
				else
					return false;
			}
			catch
			{
				return false;
			}
		}

		public static void WriteToEventViewer(string strMsg,EventLogEntryType enumType)
		{

			try
			{
				if(!EventLog.Exists(_strLog) || !EventLog.SourceExists(_strSource))
					EventLog.CreateEventSource(_strSource,_strLog);

				EventLog myLog = new EventLog();
				myLog.Source = _strSource;
				myLog.Log = _strLog;
				myLog.MachineName = ".";//本機
				myLog.WriteEntry(strMsg,enumType);
				myLog.Close();
			}
			catch
			{
	
			}

		}


		//亂數產生帳號 tp0712005 981026
		public static string GenAccount()
		{
			string strTemplate = "aaaaaa1111";
			string strAccount = string.Empty;

			strAccount =  Util.RandonStr(strTemplate);
			return strAccount;
		}

		public static string RandonStr(string Format)
		{
			int iCharCode=0;
			string sText, sChar;
			Random ran = new Random();

			sText = string.Empty;
			for(int i=0;i < Format.Length;i++)
			{
				sChar = Format.Substring(i, 1);
				if(sChar == "A")
					iCharCode = RandonInt(65, 90, ran);
				else if(sChar == "a")
					iCharCode = RandonInt(97, 122, ran);
				else if(sChar == "1")
					iCharCode = RandonInt(48, 57, ran);
				
				sText += Convert.ToChar(iCharCode);
			}

			return sText;
		}

		public static int RandonInt(int Min, int Max,Random objRan)
		{
			int iRnd;
			iRnd = objRan.Next(Min, Max);//產生特定範圍的亂數
			
			return iRnd;
		}

		public static uint parseIP(string IPNumber)
		{
			uint res = 0;
			if(IsNullOrEmpty(IPNumber)) return 0;
			string pattern = @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$";
			if(!Regex.IsMatch(IPNumber,pattern)) return 0;
			string[] elements = IPNumber.Split(new char[]{'.'});
			for(int i=0;i<elements.Length;i++)
			{
				if(Convert.ToInt32(elements[i])<0 || Convert.ToInt32(elements[i]) > 255)
					return 0;
			}

			if(elements.Length == 4)
			{
				res = (uint)Convert.ToInt32(elements[0]) << 24;
				res += (uint)Convert.ToInt32(elements[1]) << 16;
				res += (uint)Convert.ToInt32(elements[2]) << 8;
				res += (uint)Convert.ToInt32(elements[3]);
			}
			return res;
		}

		public static bool IsNullOrEmpty(string str)
		{
			if(str == null || str == string.Empty)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static string IPToString(uint ipnum)
		{
			string IpStr = (((int)ipnum & 0xFF000000) >> 24) + "." +
				(((int)ipnum & 0x00FF0000) >> 16) + "." +
				(((int)ipnum & 0x0000FF00) >> 8) + "." +
				(((int)ipnum & 0x000000FF));
			return IpStr;
		}	
    }
}
