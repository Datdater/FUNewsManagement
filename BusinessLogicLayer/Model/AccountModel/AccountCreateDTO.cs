using BusinessLogicLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model.AccountModel
{
    public class AccountCreateDTO
    {
		public required string AccountName { get; set; }
		public required string AccountEmail { get; set; }
		public required string AccountPassword { get; set; }
		public Role AccountRole { get; set; }
	}
}
