using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Model
{
	public class UserModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "User Name is required")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }

		[DataType(DataType.Password),Required(ErrorMessage ="password is required")]
		public string Password { get; set; }

	}
}
