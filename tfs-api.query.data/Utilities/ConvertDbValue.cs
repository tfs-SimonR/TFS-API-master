using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfs_api.query.data.Utilities
{
    public static class ConvertDbValue
    {
        /// <summary>
        /// Converts a database object value the given type, catering for DBNull values
        /// </summary>
        /// <typeparam name="TResult">type to convert too</typeparam>
        /// <param name="dbValue">database object value to be convert</param>
        /// <returns></returns>
        public static TResult ToType<TResult>(object dbValue)
        {
            return dbValue == DBNull.Value ? default(TResult) : (TResult)dbValue;
        }
    }
}
