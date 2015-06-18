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
using System.Data;

namespace WpfSQLQueryUtility
{
    /// <summary>
    /// 資料庫連線視窗.
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        /// <summary>
        /// Sql Connection 連線資訊
        /// </summary>
        public class SqlConnectionInfo
        {
            public int WindowCount { get; set; }
            public bool IsConnect {get; set;}
            /// <summary>
            /// 取得連線狀態
            /// </summary>
            public ConnectionState ConnectionStatus 
            {
                get
                {
                    if (IsConnect)
                        return ConnectionState.Open;
                    else
                        return ConnectionState.Closed;
                }
            }
            public string DataSourceName { get; set; }
            public string UserId { get; set; }
            public string Password { get; set; }
            public string Initial_Catalog { get; set; }
            /// <summary>
            /// 取得連線字串.
            /// </summary>
            public string ConnectionString
            {
                get
                {
                    if (ConnectionStatus != ConnectionState.Open)
                    {
                        if (MainWindow.ConnectionInfo.WindowCount < 1)
                        {
                            ConnectionWindow cw = new ConnectionWindow();
                            IsConnect = cw.ShowDialog().Value; //將對話框的(Yes/No)作為IsConnect狀態值.
                        }
                    }
                    return string.Format(
                        "Data Source={0};Initial Catalog={1};User Id={2};Password={3}",
                        MainWindow.ConnectionInfo.DataSourceName,
                        MainWindow.ConnectionInfo.Initial_Catalog, 
                        MainWindow.ConnectionInfo.UserId, 
                        MainWindow.ConnectionInfo.Password);
                }
            }
        }
        public ConnectionWindow()
        {
            InitializeComponent();
            GetData();
            MainWindow.ConnectionInfo.WindowCount++;
        }

        ~ConnectionWindow()
        {
            MainWindow.ConnectionInfo.WindowCount--;
        }

        //取得一個 SqlConnectionTableDataTable 的 泛型 SqlStoreTable 物件.
        SQLStoreClass<SQLStoreDataSet.SqlConnectionTableDataTable> ConnStore 
                        = new SQLStoreClass<SQLStoreDataSet.SqlConnectionTableDataTable>();

        private void GetData()
        {
            SQLStoreDataSet.SqlConnectionTableDataTable tabConn = ConnStore.GetAllData();
            cbServer.DisplayMemberPath = "DataSourceName";
            cbServer.SelectedValuePath = "DataSourceName";
            cbServer.ItemsSource = tabConn;
            //帶入上一次的 Default 值.
            var result = tabConn.Rows.OfType<DataRow>().Where(r => (bool)r["IsDefault"]).FirstOrDefault();
            if (result != null)
            {
                cbServer.Text = result["DataSourceName"].ToString();
                txtPassword.Password = result["Password"].ToString();
                txtUserID.Text = result["UserId"].ToString();
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            ConnStore.Add(cbServer.Text, 
                cbInitialCatalog.Text, 
                txtUserID.Text, 
                txtPassword.Password);
            if (chkSetDefault.IsChecked.Value)
                ConnStore.SetIsDefaultByDataSourceName(cbServer.Text);
            SetConnectionInfo(true);
            Close();
        }

        private void SetConnectionInfo(bool isConnect)
        {
            MainWindow.ConnectionInfo = new SqlConnectionInfo() { 
                DataSourceName=cbServer.Text,
                Initial_Catalog = cbInitialCatalog.Text,
                UserId = txtUserID.Text,
                Password = txtPassword.Password,
                IsConnect = isConnect
            };
        }

        private void GetInitialCatalogData()
        {
            SetConnectionInfo(true);
            DAL dal = new DAL();
            var result = from schema in dal.GetSchemaDataTable("Databases").AsEnumerable()
                         select new
                         {
                             Id = schema["dbid"].ToString(),
                             DataBase_Name = (string)schema["database_name"]
                         };
            cbInitialCatalog.DisplayMemberPath = "DataBase_Name";
            cbInitialCatalog.SelectedValuePath = "Id";
            cbInitialCatalog.ItemsSource = result.ToList();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cbServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbServer.SelectedValue != null)
            {
                string SelectedText = cbServer.SelectedValue.ToString();
                DataRow dr = ConnStore.GetOne(SelectedText);
                if (dr != null)
                {
                    txtUserID.Text = dr["UserId"].ToString();
                    txtPassword.Password = dr["Password"].ToString();
                    cbInitialCatalog.Text = dr["InitialCatalogName"].ToString();
                }
            }
        }

        private void cbInitialCatalog_DropDownOpened(object sender, EventArgs e)
        {
            GetInitialCatalogData();
        }

    }
}
