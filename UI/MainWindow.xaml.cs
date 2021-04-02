using Entities;
using Interfaces;
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

        IDataAccess<Book> bookAccess = new SqlServerAccessLayer.BookAccess(connStrSqlServer);

        public MainWindow()
        {
            InitializeComponent();
            this.tbSearchQuery.DataContext = query;
            this.tbSearchQuery.DataContextChanged += TbSearchQuery_DataContextChanged;
            this.tbSearchQuery.TextChanged += TbSearchQuery_TextChanged;

            rbSqlite.Checked += DatabaseSourceChanged;
            rbSqlServer.Checked += DatabaseSourceChanged;
        }

        private void DatabaseSourceChanged(object sender, RoutedEventArgs e)
        {
            if (rbSqlServer.IsChecked ?? false)
            {
                bookAccess = new SqlServerAccessLayer.BookAccess(connStrSqlServer);
            }

            if (rbSqlite.IsChecked ?? false)
            {
                bookAccess = new SqliteAccessLayer.BookAccess(connStrSqlite);
            }
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

            DataTable dt = bookAccess.GetDataByFirstName(this.tbSearchQuery.Text);
            dataGridView.ItemsSource = dt.DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = bookAccess.GetList();
            dataGridView.ItemsSource = dt.DefaultView;
        }
    }
}
