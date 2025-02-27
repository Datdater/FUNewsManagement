using BusinessLogicLayer.Enum;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.AccountViewModels
{
	public class AccountUpdateViewModel
	{
		public required int AccountID { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public required string AccountName { get; set; }
        [EmailAddress]
        public required string AccountEmail { get; set; }
        [Required]
        public Role AccountRole { get; set; }
	}
}
