namespace PresentationLayer.Models.CategoryViewModels
{
    public class CategoryViewModel
    {
		public int CategoryID { get; set; }
		public string? CategoryName { get; set; }
		public string? CategoryDescription { get; set; }
		public int? ParentCategoryID { get; set; }
		public bool IsActive { get; set; }
		public string? ParentCategoryName { get; set; }

	}
}
