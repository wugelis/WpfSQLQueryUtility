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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsForm = System.Windows.Forms;
using System.Resources;
using System.Reflection;
using WpfSQLQueryUtility.Datas;
using System.Data;

namespace WpfSQLQueryUtility
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowsForm.NotifyIcon m_notifyIcon;
        private WindowsForm.ContextMenu cm_Menu;

        #region Contructor
        public MainWindow()
        {
            InitializeComponent();

            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = this.Title;
            m_notifyIcon.BalloonTipTitle = this.Title;
            m_notifyIcon.Text = this.Title;
            m_notifyIcon.Icon = (System.Drawing.Icon)(new ResourceManager("WpfSQLQueryUtility.Properties.Resources", Assembly.GetExecutingAssembly()).GetObject("dbs"));
            m_notifyIcon.Visible = true;
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);
            m_notifyIcon.DoubleClick += new EventHandler(m_notifyIcon_DoubleClick);

            InitializeMenu();
        }

        void m_notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }
        #endregion

        /// <summary>
        /// 連線資訊, 用以判斷是否需要重新連線.
        /// </summary>
        public static ConnectionWindow.SqlConnectionInfo ConnectionInfo = new ConnectionWindow.SqlConnectionInfo();

        #region InitializeMenu
        private void InitializeMenu()
        {
            SQLStoreClass<SQLStoreDataSet.SQLTableDataTable> SqlStore =
                    new SQLStoreClass<SQLStoreDataSet.SQLTableDataTable>();
            int iCount = SqlStore.getSQLCount+5, i = 0;
            SQLStoreDataSet.SQLTableDataTable SqlTable = SqlStore.GetAllData();
            WindowsForm.MenuItem[] menuItem = new WindowsForm.MenuItem[iCount];
            foreach (DataRow dr in SqlTable.Rows)
            {
                menuItem[i] = new WindowsForm.MenuItem(dr["SQLCommandName"].ToString(), (a, c) => { ClearScreen(); ExecuteSQL(a); });
                i++;
            }
            menuItem[iCount-5] =    new WindowsForm.MenuItem("-");
            menuItem[iCount - 4] = new WindowsForm.MenuItem("連線設定(C&)", (a, c) => { ConnectionWindow cw = new ConnectionWindow(); cw.ShowDialog(); });
            menuItem[iCount-3] =    new WindowsForm.MenuItem("關於此程式(&B)", (a, c) => { AboutWindow win = new AboutWindow(); win.ShowDialog();});
            menuItem[iCount-2] =    new WindowsForm.MenuItem("-");
            menuItem[iCount-1] =    new WindowsForm.MenuItem("結束(&X)", (a, c) => { this.Close(); });

            this.cm_Menu = new System.Windows.Forms.ContextMenu(menuItem);
            this.m_notifyIcon.ContextMenu = cm_Menu;
        }
        #endregion

        #region OnClose
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            m_notifyIcon.Visible = false;
        }
        #endregion

        #region Dispose
        ~MainWindow()
        {
            m_notifyIcon.Dispose();
        }
        #endregion

        #region WindowStateChange
        private WindowState m_storedWindowState = WindowState.Normal;
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (m_notifyIcon != null)
                    m_notifyIcon.ShowBalloonTip(2000);
            }
            else
                m_storedWindowState = WindowState;
        }

        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }

        void ShowTrayIcon(bool show)
        {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = show;
        }

        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }
        #endregion

        #region NotifyIcon Click
        void m_notifyIcon_Click(object sender, EventArgs e)
        {
            //AboutWindow w = new AboutWindow();
            //w.Show();
            //WindowState = m_storedWindowState;
        }
        #endregion

        #region 清除畫面資料
        void ClearScreen()
        {
            dataGrid1.ItemsSource = null;
            txtSQL.Text = " ";
            this.Title = "簡易 SQL 執行工具 v1.0 (#)";
        }
        #endregion

        #region 執行 SQL 
        /// <summary>
        /// 執行 SQL 
        /// </summary>
        /// <param name="sender"></param>
        void ExecuteSQL(object sender)
        {
            this.Visibility = Visibility.Visible;

            SQLStoreClass<SQLStoreDataSet.SQLTableDataTable> SqlStore =
                    new SQLStoreClass<SQLStoreDataSet.SQLTableDataTable>();
            var SqlObj = SqlStore.GetOneDataByMenuTitle(
                (sender as WindowsForm.MenuItem).Text, 
                new { SQL="", SQLCommandName=""});

            string SqlStatement = SqlObj.SQL;
            txtSQL.Text = SqlStatement;

            this.Title = this.Title.Replace("(#)", string.Format("({0})", SqlObj.SQLCommandName));

            dataGrid1.ItemsSource = GetData(SqlStatement);
        }
        /// <summary>
        /// 執行 SQL 
        /// </summary>
        void ExecuteSQL()
        {
            DataView dv = QuerySQL();
            dataGrid1.ItemsSource = dv;
            txtSQL.Focus();
        }
        /// <summary>
        /// 執行 SQL (取回 DataView)
        /// </summary>
        /// <returns></returns>
        private DataView QuerySQL()
        {
            DataView dv = null;
            if (txtSQL.SelectedText != string.Empty && txtSQL.SelectedText != null)
            {
                dv = GetData(txtSQL.SelectedText);
            }
            else
            {
                dv = GetData(txtSQL.Text);
            }
            return dv;
        }
        #endregion

        #region 取得資料
        DataView GetData(string SqlStatement)
        {
            DataView dv = null;
            try
            {
                DAL dal = new DAL();
                DataTable dt = new DataTable();
                //dt.TableNewRow += (a, c) => { labDataCount.Content = string.Format("共 {0} 筆", 100); };
                if (ConnectionInfo.ConnectionStatus == ConnectionState.Open)
                {
                    dt = dal.Query(SqlStatement).Tables[0];
                    dv = dt.DefaultView;
                    labDataCount.Content = string.Format("共 {0} 筆", dv.Count);
                }
                else
                {
                    MessageBox.Show("未連線到 SQL Server！", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return dv;
       }
        #endregion

        #region 相關按鈕事件
        private void btnExecuteSQL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Show();
                ExecuteSQL();
            }
            catch (Exception ex)
            {
                ConnectionInfo.IsConnect = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateSQL_Click(object sender, RoutedEventArgs e)
        {
            SQLStoreClass<SQLStoreDataSet.SQLTableDataTable> SqlStore =
                    new SQLStoreClass<SQLStoreDataSet.SQLTableDataTable>();
            int start = this.Title.IndexOf('(');
            int end = this.Title.IndexOf(')');
            SqlStore.Edit(
                Title.Substring(start+1, (end-start)-1),
                txtSQL.Text,
                string.Empty,
                string.Empty);
            MessageBox.Show("儲存成功！", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnAdd2Favorites_Click(object sender, RoutedEventArgs e)
        {
            SetMenuTitleWindow w = new SetMenuTitleWindow(txtSQL.Text);
            bool? result = w.ShowDialog();
            if (result.Value)
            {
                InitializeMenu();
            }
        }

        private void btnCreateClass_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = QuerySQL().Table;
            ClassWindow cw = new ClassWindow(dt);
            cw.ShowDialog();
        }
        #endregion
    }
}
