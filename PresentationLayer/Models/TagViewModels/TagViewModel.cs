using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.TagViewModels
{
    public class TagViewModel
    {
        public int TagID { get; set; }
        [Required]
        public string TagName { get; set; }
        public string Note { get; set; }
    }
}
