using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.NewsArticleModel
{
    public class NewsArticleViewModel
    {
        public int NewsArticleID { get; set; }
        public required string NewsTitle { get; set; }
        public required string Headline { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string NewsContent { get; set; }
        public required string NewsSource { get; set; }
        public int CategoryID { get; set; }
        public required bool NewsStatus { get; set; }
        public int CreatedByID { get; set; }
        public int UpdatedByID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<string> Tags { get; set; }
        public string CreatedByName { get; set; }
        public string CategoryName { get; set; }
        public string UpdatedByName { get; set; }
    }
}
