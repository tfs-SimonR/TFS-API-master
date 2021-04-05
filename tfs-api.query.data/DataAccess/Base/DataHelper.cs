using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tfs_api.query.data.DataAccess.Base
{
    public class DataHelper
    {
        private readonly string _connectionString;

        public DataHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        //
        public DataTable GetDataTable(string sqlText)
        {
            SqlCommand command = new SqlCommand(sqlText);
            return GetDataTable(command);
        }

        public DataTable GetDataTable(SqlCommand command)
        {
            var results = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                command.Connection = connection;
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(results);
                }
            }
            return results;
        }

        public List<TResult> GetResults<TResult>(SqlCommand command)
        {
            DataTable resultsDt = GetDataTable(command);
            List<TResult> results = new List<TResult>();
            foreach (DataRow row in resultsDt.Rows)
            {
                TResult result = CreateItem<TResult>(row);
                results.Add(result);
            }
            return results;
        }

        private TResult CreateItem<TResult>(DataRow row)
        {
            TResult result = Activator.CreateInstance<TResult>();
            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo property = result.GetType().GetProperty(column.ColumnName);
                if (property != null)
                {
                    object databaseValue = row[column.ColumnName];
                    object defaultValue = property.PropertyType.IsValueType
                        ? Activator.CreateInstance(property.PropertyType)
                        : null;
                    property.SetValue(result,
                        databaseValue == DBNull.Value ? defaultValue : databaseValue);
                }
            }
            return result;
        }

        public TResult ExecuteScalar<TResult>(SqlCommand command)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                command.Connection = connection;
                command.Connection.Open();
                return (TResult)command.ExecuteScalar();
            }
        }

        public TResult ExecuteScalar<TResult>(string sqlText)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlText);
                command.Connection = connection;
                command.Connection.Open();
                return (TResult)command.ExecuteScalar();
            }
        }

        public void ExecuteNonQuery(SqlCommand command)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                command.Connection = connection;
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteNonQuery(string sqlText)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlText);
                command.Connection = connection;
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
