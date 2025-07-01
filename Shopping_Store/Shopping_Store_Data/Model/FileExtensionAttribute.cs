using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Model
{
    public class FileExtensionAttribute : ValidationAttribute
    {

        // Các extensions cho phép, được truyền vào constructor (ví dụ: "jpg,png,jpeg")
        private readonly string[] _allowedExtensions;

        public FileExtensionAttribute(string allowedExtensions)
        {
            // Chia chuỗi extensions thành mảng
            _allowedExtensions = allowedExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(e => e.Trim().ToLower()) // Xóa khoảng trắng và chuyển sang chữ thường
                                                  .ToArray();
        }

        // Ghi đè phương thức IsValid để chứa logic validation
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Kiểm tra nếu giá trị là null (không có file được upload)
            if (value == null)
            {
                // Nếu không bắt buộc phải có file, return ValidationResult.Success.
                // Nếu bắt buộc phải có file, hãy dùng [Required] attribute kết hợp.
                return ValidationResult.Success; // Hoặc new ValidationResult("Vui lòng chọn một tập tin.") nếu bạn muốn bắt buộc file
            }

            // Kiểm tra nếu giá trị là IFormFile (kiểu dữ liệu của file upload)
            if (value is IFormFile file)
            {
                // Lấy phần mở rộng của file
                var fileExtension = Path.GetExtension(file.FileName); // Ví dụ: ".jpg"
                if (string.IsNullOrEmpty(fileExtension))
                {
                    return new ValidationResult("Tập tin không có phần mở rộng.");
                }

                // Loại bỏ dấu chấm và chuyển sang chữ thường để so sánh
                var actualExtension = fileExtension.TrimStart('.').ToLower();

                // Kiểm tra xem phần mở rộng có nằm trong danh sách cho phép không
                if (!_allowedExtensions.Any(ext => ext == actualExtension))
                {
                    // Trả về lỗi nếu không hợp lệ
                    return new ValidationResult($"Chỉ cho phép các định dạng: {string.Join(", ", _allowedExtensions.Select(e => $".{e}"))}.");
                }
            }
            else
            {
                // Nếu kiểu dữ liệu không phải IFormFile, có thể đây là một lỗi lập trình
                return new ValidationResult("Thuộc tính này không phải là một tập tin.");
            }

            // Nếu validation thành công
            return ValidationResult.Success;
        }
    }
}

