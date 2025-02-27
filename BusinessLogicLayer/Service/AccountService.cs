using AutoMapper;
using BusinessLogicLayer.Model.AccountModel;
using BusinessLogicLayer.Service.Contracts;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
	public class AccountService : IAccountService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;

		public AccountService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_configuration = configuration;
			_mapper = mapper;
		}
		public async Task<AccountDTO> AuthenticateAsync(LoginDTO loginDto)
		{
			var adminEmail = _configuration["AdminAccount:Email"];
			var adminPassword = _configuration["AdminAccount:Password"];
			if (loginDto.Email == adminEmail && loginDto.Password == adminPassword)
			{
				return new AccountDTO
				{
					AccountID = _configuration["AdminAccount:AccountId"],
					AccountEmail = adminEmail,
					AccountPassword = adminPassword,
					//AccountRole = 0,
					//AccountName = "Admin",
					AccountName = _configuration["AdminAccount:Name"],
					AccountRole = (Enum.Role)int.Parse(_configuration["AdminAccount:Role"])
			};
		}

			var account = await _unitOfWork.SystemAccounts.GetByEmailAsync(loginDto.Email);
			if (account == null)
			{
				return null;
			}

			if (account.AccountPassword != loginDto.Password)
			{
                return null;
			}

			return _mapper.Map<AccountDTO>(account);
		}

        public async Task CreateAccount(AccountCreateDTO accountDTO)
        {
			if(await IsEmailUniqueAsync(accountDTO.AccountEmail))
			{
				var account = _mapper.Map<SystemAccount>(accountDTO);
				await _unitOfWork.SystemAccounts.AddAsync(account);
				await _unitOfWork.SaveChangesAsync();
			}
        }

        public async Task DeleteAccount(int id)
        {
			var account = await _unitOfWork.SystemAccounts.GetByIdAsync(id);
			if (await _unitOfWork.NewsArticles.FindAsync(x => x.CreatedByID == id) != null)
			{
				throw new Exception("Can't delete because the staff created news");
			}
			await _unitOfWork.SystemAccounts.RemoveAsync(account);
			await _unitOfWork.SaveChangesAsync();
        }

		
		public async Task<List<AccountDTO>> GetAllAsync()
		{
			var accountListRaw = await _unitOfWork.SystemAccounts.GetAllAsync();
			var accountList = _mapper.Map<List<AccountDTO>>(accountListRaw);
			return accountList;
		}

		public async Task<AccountDTO> GetByIdAsync(int id)
        {
            var account = await _unitOfWork.SystemAccounts.GetByIdAsync(id);
            return _mapper.Map<AccountDTO>(account);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var existingAccount = await _unitOfWork.SystemAccounts.GetByEmailAsync(email);
			return existingAccount == null;
        }

        public async Task UpdateAccount(AccountUpdateDTO accountDto)
        {
            var existingAccount = await _unitOfWork.SystemAccounts.GetByIdAsync(accountDto.AccountID);
            if (existingAccount == null)
            {
                throw new KeyNotFoundException($"Account with ID {accountDto.AccountID} not found");
            }

            if (!await IsEmailUniqueAsync(accountDto.AccountEmail) && accountDto.AccountEmail != existingAccount.AccountEmail)
            {
                throw new InvalidOperationException("Email already exists");
            }
			accountDto.AccountPassword = existingAccount.AccountPassword;
            _mapper.Map(accountDto, existingAccount);
            await _unitOfWork.SystemAccounts.UpdateAsync(existingAccount);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePassword(string password, string id)
        {
            var existingAccount = await _unitOfWork.SystemAccounts.GetByIdAsync(int.Parse(id));
			existingAccount.AccountPassword = password;
            await _unitOfWork.SystemAccounts.UpdateAsync(existingAccount);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
