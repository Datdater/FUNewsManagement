using BusinessLogicLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model.AccountModel
{
    public class AccountDTO
    {
		public string AccountID { get; set; }
		public string AccountName { get; set; }
		public string AccountEmail { get; set; }
		public string AccountPassword { get; set; }
		public Role AccountRole { get; set; }
	}
}
