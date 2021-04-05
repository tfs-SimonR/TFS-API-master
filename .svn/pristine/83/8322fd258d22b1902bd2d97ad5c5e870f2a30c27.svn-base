using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using TFS_API.Services.Interfaces;

namespace TFS_API.Services
{
    public class StoredProcedureService :IStoredProcedureService
    {
        public IEnumerable<dynamic> DataEnumerable(string cmdText)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=tfs-vmmdb01-prod;Initial Catalog=mi9_mms_fs_live;Integrated Security=True"))
            {
                conn.Open();

                using (var command = new SqlCommand(cmdText, conn))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        var fields = new List<string>();

                        for (var i = 0; i < dataReader.FieldCount; i++)
                        {
                            fields.Add(dataReader.GetName(i));
                        }

                        while (dataReader.Read())
                        {
                            var item = new ExpandoObject() as IDictionary<string, object>;

                            for (var i = 0; i < fields.Count; i++)
                            {
                                item.Add(fields[i], dataReader[fields[i]]);
                            }

                            yield return item;
                        }
                    }
                }
                conn.Close();

            }
           
            
        }
    }
}