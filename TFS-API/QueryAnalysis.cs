using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API
{
    public static class QueryAnalysis {

        public static void Reset()
        {
            QueryStartTime = DateTime.Now;
            QueryEndTime = DateTime.Now;
            QueryExecutionTime = 0;
        }
        public static void Start()
        {
            QueryStartTime = DateTime.Now;
        }
        public static void Stop()
        {
            try
            {
                QueryEndTime = DateTime.Now;
                QueryExecutionTime = (QueryEndTime - QueryStartTime).TotalSeconds;
            }
            catch
            {
                QueryExecutionTime = 0;
            }
        }
        public static DateTime QueryStartTime { get; set; }
        public static DateTime QueryEndTime { get; set; }
        public static double QueryExecutionTime { get; set; }
    }
}