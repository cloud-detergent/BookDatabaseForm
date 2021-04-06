using System;
using System.Collections.Generic;
using System.Data;

namespace Interfaces
{
    public interface IDataAccess<T>
    {
        public IEnumerable<T> GetList(int pageSize, int offset, string query = "");

        public DataTable GetDataTable(int pageSize, int offset, string query = "");
    }
}
