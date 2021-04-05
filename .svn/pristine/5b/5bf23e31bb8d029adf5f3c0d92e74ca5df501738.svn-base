using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class HHTActionDTO
    {
        public int task_id { get; set; }

        public string store_id { get; set; }

        public string description { get; set; }

        public string instructions { get; set; }

        public List<TaskActions> task_actions { get; set; }

        public class TaskActions
        {
            public int action_type_id { get; set; }

            public bool? isMandatory { get; set; }

            public string question { get; set; }

            public string param_1 { get; set; }

            public string param_2 { get; set; }

            public string param_3 { get; set; }

            public string param_4 { get; set; }

        }
    }
}