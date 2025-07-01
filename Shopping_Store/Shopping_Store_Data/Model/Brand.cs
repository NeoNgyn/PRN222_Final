using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Model
{
	public class Brand
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage ="Please Fill your Brand Name")]
		public string Name { get; set; }
		public string? Slug { get; set; }
		[Required, MinLength(4, ErrorMessage = "Please Fill your Brand Description")]
		public string Description { get; set; }
		public string Status { get; set; }

		//public virtual ICollection<Product> Products { get; set; } = new List<Product>();
	}
}
