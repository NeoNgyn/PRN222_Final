using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Shopping_Store_Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shopping_Store_Data.Model
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Please Fill your Product Name")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Please Fill your Product Description0")]
		public string Description { get; set; }
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")] // Kiểm tra giá trị > 0
        public decimal Price { get; set; }
		public string? Slug { get; set; }
		public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }

        public int BrandId { get; set; }

        [ValidateNever]
        public Brand Brand { get; set; }

        public string? ImageUrl { get; set; }

		[NotMapped]      
        [FileExtension("jpg,png,jpeg", ErrorMessage = "Chỉ cho phép các định dạng ảnh .jpg, .png, .jpeg.")] // <-- Áp dụng Attribute     
        public IFormFile? ImgUpload { get; set; }
    }
}
