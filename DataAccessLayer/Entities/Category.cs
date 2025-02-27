using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class Category
	{
		public required int CategoryID { get; set; }
		public required string CategoryName { get; set; }
		public required string CategoryDescription { get; set; }
		public int? ParentCategoryID { get; set; }
		public bool IsActive { get; set; }

		// Navigation Properties
		public virtual Category? ParentCategory { get; set; }
		public virtual ICollection<Category>? SubCategories { get; set; }
		public virtual ICollection<NewsArticle>? NewsArticles { get; set; }
	}
}
