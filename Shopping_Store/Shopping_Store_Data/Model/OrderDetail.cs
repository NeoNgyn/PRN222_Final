using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Model
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public string UserName { get; set; }
        public int OrderCode { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
