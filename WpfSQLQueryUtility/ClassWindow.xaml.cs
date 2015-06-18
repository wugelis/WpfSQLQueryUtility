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
using System.Data;

namespace WpfSQLQueryUtility
{
    /// <summary>
    /// ClassWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ClassWindow : Window
    {
        public ClassWindow(DataTable dt)
        {
            InitializeComponent();
            txtClassDef.Text = GetClassDef(dt);
        }

        private string GetClassDef(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("public class {0}", dt.TableName));
            sb.AppendLine("{");

            foreach (DataColumn col in dt.Columns)
            {
                switch (col.DataType.ToString())
                {
                    case "System.String":
                        sb.AppendLine(string.Format("\tpublic {0} {1} {{get; set;}}", "string", col.ColumnName));
                        break;
                    case "System.Int32":
                        sb.AppendLine(string.Format("\tpublic {0} {1} {{get; set;}}", "int", col.ColumnName));
                        break;
                    case "System.DateTime":
                        sb.AppendLine(string.Format("\tpublic {0} {1} {{get; set;}}", "DateTime", col.ColumnName));
                        break;
                    case "System.Byte[]":
                        sb.AppendLine(string.Format("\tpublic {0} {1} {{get; set;}}", "byte[]", col.ColumnName));
                        break;
                    case "System.Decimal":
                        sb.AppendLine(string.Format("\tpublic {0} {1} {{get; set;}}", "decimal", col.ColumnName));
                        break;
                    case "System.Guid":
                        sb.AppendLine(string.Format("\tpublic {0} {1} {{get; set;}}", "Guid", col.ColumnName));
                        break;
                    case "System.Boolean":
                        sb.AppendLine(string.Format("\tpublic {0} {1} {{get; set;}}", "bool", col.ColumnName));
                        break;
                    default:
                        sb.AppendLine(string.Format("\tpublic {0} {1} {{get; set;}}", "object", col.ColumnName));
                        break;
                }
            }
            sb.AppendLine("}");
            return sb.ToString();
        }    
    }


}
