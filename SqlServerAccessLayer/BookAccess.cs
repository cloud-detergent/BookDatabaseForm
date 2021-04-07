using Entities;
using Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlServerAccessLayer
{
    public class BookAccess : IDataAccess<Book>
    {
        string connStr;

        public BookAccess(string connStr)
        {
            this.connStr = connStr;
        }

        string selectQuery = @"SELECT b.Id, b.Name, a.Id, a.FirstName, a.LastName
FROM [Authors] a
    INNER JOIN AuthorsBooks AB on a.Id = AB.AuthorId
    INNER JOIN Books b on b.Id = AB.BookId
    WHERE a.[FirstName] LIKE @query
    ORDER BY b.Id ASC
    OFFSET @startIndex ROWS 
    FETCH NEXT @pageSize ROWS ONLY";


        public IEnumerable<Book> GetList(int pageSize, int offset, string query = "")
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = selectQuery;

                cmd.Parameters.AddWithValue("@query", $"%{query}%");
                cmd.Parameters.AddWithValue("@startIndex", offset);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                SqlDataReader sdr = cmd.ExecuteReader();

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

            }
            return books;
        }

        public DataTable GetDataTable(int pageSize, int offset, string query = "")
        {
            DataTable dt;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(selectQuery, connection);
                cmd.Parameters.AddWithValue("@query", $"%{query}%");
                cmd.Parameters.AddWithValue("@startIndex", offset);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dt = new DataTable();
                dataAdapter.Fill(dt);
            }

            return dt;
        }

        public void CreateEntity(Book entity)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = "dbo.CreateBook";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookName", entity.Name);
                cmd.Parameters.AddWithValue("@AuthorId", entity.Authors[0].Id);
                cmd.ExecuteNonQuery();

            }
        }
    }
}
