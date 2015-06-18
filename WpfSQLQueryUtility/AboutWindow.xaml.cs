using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using WpfSQLQueryUtility.Datas;
using System.Data;

namespace WpfSQLQueryUtility
{
	/// <summary>
	/// ExecuteSQL Statement 的互動邏輯.
	///  Create by Gelis at 2012/2/7.
	/// </summary>
	[DebuggerVisualizer(typeof(AboutWindow))]
	[Serializable]
	public partial class AboutWindow : Window
	{
		#region Constructor
		public AboutWindow()
		{
			InitializeComponent();
		}

		public AboutWindow(string Id):this()
		{
		}
		#endregion

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			
		}
	}
}
