using Entities;
using Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqliteAccessLayer
{
    public class BookAccess : IDataAccess<Book>
    {
        string connStr;

        public BookAccess(string connStr)
        {
            this.connStr = connStr;
        }

        string selectQuery = @"SELECT b.id, b.name, a.id, a.firstName, a.lastName
FROM Authors a
    INNER JOIN AuthorsBooks AB on a.id = AB.authorId
    INNER JOIN Books b on b.id = AB.bookId
    WHERE a.firstName like $query
    ORDER BY b.id ASC
    LIMIT $pageSize OFFSET $startIndex";

        public IEnumerable<Book> GetList(int pageSize, int offset, string query = "")
        {
            List<Book> books = new List<Book>();

            SqliteConnection connection = new SqliteConnection(connStr);
            connection.Open();

            SqliteCommand cmd = new SqliteCommand(selectQuery, connection);
            cmd.Parameters.AddWithValue("$query", $"%{query}%");
            cmd.Parameters.AddWithValue("$pageSize", pageSize);
            cmd.Parameters.AddWithValue("$startIndex", offset);

            SqliteDataReader sdr = cmd.ExecuteReader();

            bool flag = false;
            while (sdr.Read())
            {
                Book book = books.Find(x => x.Name.Equals(sdr.GetString(1)));

                flag = true;
                if (book is null)
                {
                    flag = false;
                    book = new Book()
                    {
                        Id = sdr.GetInt32(0),
                        Name = sdr.GetString(1)
                    };
                }

                Author author = new Author()
                {
                    Id = sdr.GetInt32(2),
                    FirstName = sdr.GetString(3),
                    LastName = sdr.GetString(4)
                };

                book.Authors.Add(author);

                if (!flag)
                {
                    books.Add(book);
                }
            }

            connection.Close();
            return books;
        }

        public DataTable GetDataTable(int pageSize, int offset, string query = "")
        {
            return new DataTable();
        }

        //public DataTable GetDataByQuery(string query)
        //{
        //    // SELECT b.id, b.name, a.id, a.firstName, a.lastName
        //    IEnumerable<Book> list = GetList(10, 0);

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Id книги");
        //    dt.Columns.Add("Название");
        //    dt.Columns.Add("Id автора");
        //    dt.Columns.Add("Имя автора");
        //    dt.Columns.Add("Фамилия автора");

        //    foreach (Book item in list)
        //    {
        //        var a = item.Authors;

        //        string[] bookParams = new string[5]
        //            {
        //                item.Id.ToString(),
        //                item.Name,
        //                string.Join("; ", a.Select(x => x.Id)),
        //                string.Join("; ", a.Select(x => x.FirstName)),
        //                string.Join("; ", a.Select(x => x.LastName))
        //            };

        //        dt.Rows.Add(bookParams);
        //    }

        //    return dt;
        //}
    }
}
