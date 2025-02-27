using BusinessLogicLayer.Enum;

namespace PresentationLayer.Models.AccountViewModels
{
	public class AccountViewModel
	{
		public string AccountID { get; set; }
		public string AccountName { get; set; }
		public string AccountEmail { get; set; }
		public string AccountPassword { get; set; }
		public Role AccountRole { get; set; }
	}
}
