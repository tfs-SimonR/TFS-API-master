//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPOSDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuestionAnswer
    {
        public int Id { get; set; }
        public int StoredId { get; set; }
        public int CustomerId { get; set; }
        public string TransId { get; set; }
        public bool isGood { get; set; }
        public bool isBad { get; set; }
        public string Question { get; set; }
        public int TillId { get; set; }
        public Nullable<System.DateTime> AnswerDateTime { get; set; }
    }
}