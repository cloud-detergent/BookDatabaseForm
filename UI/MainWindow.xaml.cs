using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string connStrSqlite = @"Data Source=.\database.db";
        static string connStrSqlServer =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public string query;

        SqlServerAccessLayer.BookAccess bookAccess = new SqlServerAccessLayer.BookAccess(connStrSqlServer);

        public MainWindow()
        {
            InitializeComponent();
            this.tbSearchQuery.DataContext = query;
            this.tbSearchQuery.DataContextChanged += TbSearchQuery_DataContextChanged;
            this.tbSearchQuery.TextChanged += TbSearchQuery_TextChanged;
        }

        private void TbSearchQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchData();
        }

        private void TbSearchQuery_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SearchData();
        }

        private void SearchData()
        {
            Debug.WriteLine(this.tbSearchQuery.Text);
            Debug.WriteLine(this.query);

            DataTable dt = bookAccess.GetTableByFirstName(this.tbSearchQuery.Text);
            dataGridView.ItemsSource = dt.DefaultView;
            /*
             DataTable dt = new DataTable();
		    dt.Columns.Add("One");
		    dt.Columns.Add("Two");
		
		    dt.Rows.Add(new string[]{"1", "2"});
		    dt.Rows.Add(new string[]{"3", "4"});
             */
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = bookAccess.GetList();
            dataGridView.ItemsSource = dt.DefaultView;
        }
    }
}
