using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.CategoryViewModels
{
	public class CategoryCreateViewModel
	{
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters")]
        [MinLength(2, ErrorMessage = "Category Name must be at least 2 characters long")]
        public string CategoryName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [MinLength(5, ErrorMessage = "Description must be at least 5 characters long")]
        public string CategoryDescription { get; set; }
        public int? ParentCategoryID { get; set; }
		public bool IsActive { get; set; }
	}
}
