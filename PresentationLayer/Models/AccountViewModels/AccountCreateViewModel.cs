using BusinessLogicLayer.Enum;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.AccountViewModels
{
	public class AccountCreateViewModel
	{
		[Required]
		[MinLength(3)]
		[MaxLength(20)]
		public string AccountName { get; set; }
		[Required]
        [EmailAddress]
        public string AccountEmail { get; set; }
		[Required]
		[MinLength(6)]
		[DataType(DataType.Password)]
        public string AccountPassword { get; set; }
		[Required]
		public Role AccountRole { get; set; }
	}
}
