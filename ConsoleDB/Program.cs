using Entities;
using Interfaces;
using System;
using System.Configuration;

namespace ConsoleDB
{
    class Program
    {
        static string connStrSqlite = @"Data Source=.\database.db";
        static string connStrSqlServer =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        static void Main(string[] args)
        {
            //Console.WriteLine(ConfigurationManager.AppSettings["logPath"]);


            IDataAccess<Book> bookAccess;
            IDataAccess<Author> authorAccess;

            SelectDatabase(out bookAccess, out authorAccess);

            int offset = 0, pageSize = 3;

            ConsoleKeyInfo key;
            do
            {
                //var bookList = bookAccess.GetList(offset, pageSize);
                //var bookList = bookAccess.GetList();
                var authorList = authorAccess.GetList(offset, pageSize);

                //Console.WriteLine(string.Join("\n", bookList));
                Console.WriteLine();
                Console.WriteLine("\n", string.Join("\n", authorList));

                key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        offset = offset > pageSize ? offset - pageSize : 0;
                        break;
                    case ConsoleKey.RightArrow:
                        offset += pageSize;
                        break;
                    default:
                        break;
                }
            }
            while (key.Key != ConsoleKey.Q);

            Console.ReadLine();
        }

        static void SelectDatabase(out IDataAccess<Book> bookAccess, out IDataAccess<Author> authorAccess)
        {
            Console.WriteLine("Выберите базы данных: 1 - Sqlite, 2 - Sql Server");
            int db = Convert.ToInt32(Console.ReadLine());

            switch (db)
            {
                case 1:
                    bookAccess = new SqliteAccessLayer.BookAccess(connStrSqlite);
                    authorAccess = new SqliteAccessLayer.AuthorAccess(connStrSqlite);
                    break;
                case 2:
                    bookAccess = new SqlServerAccessLayer.BookAccess(connStrSqlServer);
                    authorAccess = new SqlServerAccessLayer.AuthorAccess(connStrSqlServer);
                    break;
                default:
                    bookAccess = null;
                    authorAccess = null;
                    break;
            }
        }
    }
}
