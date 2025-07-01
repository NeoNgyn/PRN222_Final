using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shopping_Store_Data.Model;

namespace Shopping_Store_Data
{
    public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if (!_context.Products.Any())
			{
				Category macbook = new Category { Name= "macbook", Slug= "macbook", Description= "macbook is the large brand in the world", status="1"};

				Category pc = new Category { Name= "pc", Slug= "pc", Description= "pc is the second brand in the world", status="1"};

				Brand apple = new Brand { Name= "apple", Slug= "apple", Description= "apple is the second brand in the world", Status="1"};

				Brand samsung = new Brand { Name="Samsung", Slug= "samsung", Description= "samsung is the second brand in the world", Status="1"};

				_context.Products.AddRange(
					
					new Product
					{
						Name = "Macbook",					
						Description = "Macbook is the best computer in the world",
						Price = 1000,
						Slug = "Macbook",
						Category = macbook,
						Brand = apple,
						ImageUrl = "1.jpg"
					},
					new Product
					{
						Name = "pc",					
						Description = "pc is the best computer in the world",
						Price = 1234,
						Slug = "Macbook",
						Category = pc,
						Brand = samsung,
						ImageUrl = "1.jpg"
					}
				);
				_context.SaveChanges();
			}

			}
		}
	}

