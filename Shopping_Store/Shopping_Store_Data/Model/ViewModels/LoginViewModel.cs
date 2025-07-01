using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Model.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "User Name is bat buoc")]
		public string UserName { get; set; }

		[DataType(DataType.Password), Required(ErrorMessage = "password is phai nhap")]
		public string Password { get; set; }
		public string? returnUrl { get; set; }

	}
}
