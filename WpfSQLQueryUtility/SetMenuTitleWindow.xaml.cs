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
using WpfSQLQueryUtility.Datas;

namespace WpfSQLQueryUtility
{
    /// <summary>
    /// SetMenuTitleWindow.xaml 的互動邏輯
    /// </summary>
    public partial class SetMenuTitleWindow : Window
    {
        public SetMenuTitleWindow()
        {
            InitializeComponent();
        }

        public SetMenuTitleWindow(string SQL):this()
        {
            this.Sql = SQL;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtMenuTitle.Text.Trim() == "")
            {
                MessageBox.Show("請輸入一個易記的名稱。");
                return;
            }

            try
            {
                SQLStoreClass<SQLStoreDataSet.SQLTableDataTable> SqlStore = 
                    new SQLStoreClass<SQLStoreDataSet.SQLTableDataTable>();
                SqlStore.Add(this.txtMenuTitle.Text, this.Sql, string.Empty, string.Empty);
            }
            finally
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private string Sql = string.Empty;
    }
}
