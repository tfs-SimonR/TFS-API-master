//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HHTTaskDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_task_actions
    {
        public Nullable<int> taskid { get; set; }
        public int TA_id { get; set; }
        public Nullable<int> actiontypeid { get; set; }
        public Nullable<bool> isMandatory { get; set; }
        public string questions { get; set; }
        public string param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }
        public string param4 { get; set; }
    }
}
