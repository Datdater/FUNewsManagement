using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class NewsArticle
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
		public int? UpdatedByID { get; set; }
		public DateTime? ModifiedDate { get; set; }

		// Navigation Properties
		public required virtual Category Category { get; set; }
		public required virtual SystemAccount CreatedBy { get; set; }
		public virtual SystemAccount? UpdatedBy { get; set; }
		public required virtual ICollection<NewsTag> NewsTags { get; set; }
	}
}
