using Entities;
using Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace SqliteAccessLayer
{
    public class AuthorAccess: IDataAccess<Author>
    {

        string connStr;

        public AuthorAccess(string connStr)
        {
            this.connStr = connStr;
        }

        public IEnumerable<Author> GetList(int pageSize=10, int offset=0, string query = "")
        {
            List<Author> authors = new List<Author>();

            SqliteConnection connection = new SqliteConnection(connStr);
            connection.Open();

            SqliteCommand cmd = new SqliteCommand("SELECT id, firstName, lastName FROM Authors", connection);
            SqliteDataReader sdr = cmd.ExecuteReader();
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
    }
}
