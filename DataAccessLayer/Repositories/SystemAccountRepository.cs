using DataAccessLayer.DatabaseConfiguration;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
	{
		public SystemAccountRepository(NewsManagementDB context) : base(context)
		{
		}

		public Task<SystemAccount> GetByEmailAsync(string email)
		{
			return _context.SystemAccount.FirstOrDefaultAsync(x => x.AccountEmail == email);
		}
    }
}
