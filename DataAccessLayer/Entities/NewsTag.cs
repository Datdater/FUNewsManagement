using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class NewsTag
	{
		public int NewsArticleID { get; set; }
		public int TagID { get; set; }

		// Navigation Properties
		public required virtual NewsArticle NewsArticle { get; set; }
		public required virtual Tag Tag { get; set; }
	}
}
