using APITools.CommonTools;
using APITools.Domain.Contracts;
using EnsimagCafet.Application.Contracts;
using EnsimagCafet.Application.Contracts.DTO;
using EnsimagCafet.Domain.Identity;

namespace EnsimagCafet.Application
{
    public sealed class UserBalanceService : IUserBalanceService
    {
        private readonly IRepository<User, Guid> _userRepository;

        public UserBalanceService(IRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserBalanceDTO>> GetUserBalance(string userName)
        {
            var userResult = await _userRepository.GetOneAsync(user => user.UserName == userName);
            if (userResult.IsSuccess)
            {
                var user = userResult.Value;
                return new UserBalanceDTO
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Balance = user.Balance
                };
            }
            return userResult.Exception;
        }

        public async Task<Result<IList<UserBalanceDTO>>> GetUserBalances()
        {
            var usersResult = await _userRepository.GetManyAsync();
            if (usersResult.IsSuccess)
            {
                return usersResult.Value.Select(user => new UserBalanceDTO
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Balance = user.Balance
                }).ToList();
            }
            return usersResult.Exception;
        }

        public async Task<Result> UpdateUserBalance(UpdateUserBalanceDTO input)
        {
            var userResult = await _userRepository.GetOneAsync(user => user.UserName == input.UserName);
            if (userResult)
            {
                var user = userResult.Value;
                var result = user.SetBalance(input.Balance);
                if (result)
                {
                    return await _userRepository.UpdateOneAsync(user, true);
                }
                return result;
            }
            return userResult.Exception;
        }
    }
}
