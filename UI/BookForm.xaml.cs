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
    public partial class BookForm : Window
    {
        public string query;

        IDataAccess<Book> bookAccess;
        IDataAccess<Author> authorAccess;

        List<Author> authors;

        public BookForm(IDataAccess<Author> authorAccess, IDataAccess<Book> bookAccess)
        {
            this.authorAccess = authorAccess;
            this.bookAccess = bookAccess;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            authors = authorAccess.GetList(10, 0).ToList();

            foreach (var author in authors)
            {
                lbAuthors.Items.Add($"{author.FirstName} {author.LastName}");
            }
        }

        private void ButtonAddBook_Click(object sender, RoutedEventArgs e)
        {
            string bookName = tbName.Text;
            if (lbAuthors.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо указать автора для книги", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var selectedAuthor = authors[lbAuthors.SelectedIndex];
                var book = new Book()
                {
                    Name = bookName,
                    Authors = { selectedAuthor }
                };

                bookAccess.CreateEntity(book);

                this.Close();
            }
        }
    }
}
