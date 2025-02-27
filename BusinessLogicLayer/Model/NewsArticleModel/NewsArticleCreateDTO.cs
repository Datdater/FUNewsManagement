using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model.NewsArticleModel
{
    public class NewsArticleCreateDTO
    {
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public string NewsContent { get; set; }
        public string NewsSource { get; set; }
        public int CategoryID { get; set; }
        public bool NewsStatus { get; set; }
        public int CreatedByID { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
    }
}
