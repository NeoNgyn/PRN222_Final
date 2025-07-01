using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Model
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4,ErrorMessage ="Please Fill Your Name Category")]
		public string Name { get; set; }

		[Required, MinLength(4, ErrorMessage = "Please Fill Your Description Category")]
		public string Description { get; set; }

	
		public string? Slug { get; set; }

		public string status { get; set; }
	}
}
