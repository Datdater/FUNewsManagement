using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class Tag
	{
		[Key]
		public int TagID { get; set; }
		public required string TagName { get; set; }
		public required string Note { get; set; }

		// Navigation Properties
		public virtual ICollection<NewsTag>? NewsTags { get; set; }
	}
}
