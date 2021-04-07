using Entities;
using Interfaces;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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

        IDataAccess<Author> authorAccess = new SqlServerAccessLayer.AuthorAccess(connStrSqlServer);
        IDataAccess<Book> bookAccess = new SqlServerAccessLayer.BookAccess(connStrSqlServer);

        public MainWindow()
        {
            InitializeComponent();
            
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
                authorAccess = new SqlServerAccessLayer.AuthorAccess(connStrSqlServer);
            }

            if (rbSqlite.IsChecked ?? false)
            {
                bookAccess = new SqliteAccessLayer.BookAccess(connStrSqlite);
                authorAccess = new SqliteAccessLayer.AuthorAccess(connStrSqlite);
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

            DataTable dt = bookAccess.GetDataTable(20, 0, this.tbSearchQuery.Text);
            dataGridView.ItemsSource = dt.DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = bookAccess.GetDataTable(20, 0);
            dataGridView.ItemsSource = dt.DefaultView;
        }

        BookForm creationForm;

        private void ButtonAddBook_Click(object sender, RoutedEventArgs e)
        {
            creationForm = new BookForm(authorAccess, bookAccess);
            creationForm.ShowDialog();
            SearchData();
        }
    }
}
