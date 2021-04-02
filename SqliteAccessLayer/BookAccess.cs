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
    ORDER BY b.id ASC
    LIMIT $pageSize OFFSET $startIndex";

        public IEnumerable<Book> GetList(int pageSize, int offset)
        {
            List<Book> books = new List<Book>();

            SqliteConnection connection = new SqliteConnection(connStr);
            connection.Open();

            SqliteCommand cmd = new SqliteCommand(selectQuery, connection);
            cmd.Parameters.AddWithValue("$pageSize", pageSize);
            cmd.Parameters.AddWithValue("$startIndex", offset);
            SqliteDataReader sdr = cmd.ExecuteReader();

            bool flag = false;
            while (sdr.Read())
            {
                //var book = new Book()
                //{
                //    Id = Convert.ToInt32(sdr["id"]),
                //    Name = sdr["name"].ToString()
                //};

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

        public DataTable GetList()
        {
            return new DataTable();
        }

        public DataTable GetTableByFirstName(string query)
        {
            return new DataTable();
        }
    }
}
