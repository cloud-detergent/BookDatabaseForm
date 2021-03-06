using Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlServerAccessLayer
{
    public class AuthorAccess : IDataAccess<Author>
    {

        string connStr;

        public AuthorAccess(string connStr)
        {
            this.connStr = connStr;
        }

        public IEnumerable<Author> GetList(int pageSize=10, int offset=0, string query = "")
        {
            List<Author> authors = new List<Author>();

            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();

            SqlCommand cmd = new SqlCommand("SELECT Id, FirstName, LastName FROM Authors", connection);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                var author = new Author()
                {
                    Id = sdr.GetInt32(0),
                    FirstName = sdr.GetString(1),
                    LastName = sdr.GetString(2)
                };

                authors.Add(author);
            }

            connection.Close();
            return authors;
        }

        public DataTable GetDataTable(int pageSize, int offset, string query = "")
        {
            return new DataTable();
        }

        public void CreateEntity(Author entity)
        {
            throw new NotImplementedException();
        }
    }
}
