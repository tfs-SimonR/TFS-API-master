using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class AuthDTO
    {

        public string Email { get; set; }

        public string Username { get; set; }
    }
}