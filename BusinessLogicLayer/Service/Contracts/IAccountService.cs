using BusinessLogicLayer.Model.AccountModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service.Contracts
{
	public interface IAccountService
    {
		Task<AccountDTO> AuthenticateAsync(LoginDTO loginDto);
		Task CreateAccount(AccountCreateDTO account);
        Task UpdatePassword(string password, string id);
		Task UpdateAccount(AccountUpdateDTO account);
		Task DeleteAccount(int id);
        Task<AccountDTO> GetByIdAsync(int id);
        Task<List<AccountDTO>> GetAllAsync();
        Task<bool> IsEmailUniqueAsync(string email);
    }
}
