using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class SystemAccount
	{
		[Key]
		public int AccountID { get; set; }
		public required string AccountName { get; set; }
		public required string AccountEmail { get; set; }
		public required string AccountRole { get; set; }
		public required string AccountPassword { get; set; }

		// Navigation Properties
		public virtual ICollection<NewsArticle>? CreatedNewsArticles { get; set; }
		public virtual ICollection<NewsArticle>? UpdatedNewsArticles { get; set; }
	}
}
