using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.NewsArticleModel
{
    public class NewsArticleCreateViewModel
    {
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
        public string NewsTitle { get; set; }
        [Required(ErrorMessage = "Headline is required.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Headline must be between 10 and 200 characters.")]
        public string Headline { get; set; }

        [Required(ErrorMessage = "News content is required.")]
        [MinLength(50, ErrorMessage = "News content must be at least 50 characters long.")]
        public string NewsContent { get; set; }

        [Required(ErrorMessage = "News source is required.")]
        [StringLength(150, ErrorMessage = "News source must not exceed 150 characters.")]
        public string NewsSource { get; set; }
        public int CategoryID { get; set; }
        public bool NewsStatus { get; set; }
        public int CreatedByID { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();

    }
}
