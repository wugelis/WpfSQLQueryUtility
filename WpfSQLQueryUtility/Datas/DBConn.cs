using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WpfSQLQueryUtility.Datas
{
	/// <summary>
	/// DBConn ���K�n�y�z�C
	/// </summary>
	public class DBConn
	{
		public DBConn()
		{
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
		}

		public string Connect()
		{
            string cn = MainWindow.ConnectionInfo.ConnectionString; //ConfigurationManager.AppSettings["CDCDDbConfig"];
			return cn;
		}
	}
}
