using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfs_api.query.data.Models
{
    public class ProductDto
    {
        public string variantcode { get; set; }
        public int? varint { get; set; }
        public string proddesc { get; set; }
        
    }
}
