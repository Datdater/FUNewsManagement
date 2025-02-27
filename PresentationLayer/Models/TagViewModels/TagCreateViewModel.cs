using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.TagViewModels
{
	public class TagCreateViewModel
	{
		[Required(ErrorMessage = "Tag name is required.")]
		[StringLength(100, ErrorMessage = "Tag name must be between 3 and 100 characters.", MinimumLength = 3)]
		public string TagName { get; set; }

		[StringLength(255, ErrorMessage = "Note cannot exceed 255 characters.")]
		public string Note { get; set; }
	}
}
