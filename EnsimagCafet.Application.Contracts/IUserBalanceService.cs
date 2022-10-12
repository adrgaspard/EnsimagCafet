using APITools.CommonTools;
using EnsimagCafet.Application.Contracts.DTO;

namespace EnsimagCafet.Application.Contracts
{
    public interface IUserBalanceService
    {
        Task<Result<UserBalanceDTO>> GetUserBalance(string userName);

        Task<Result<IList<UserBalanceDTO>>> GetUserBalances();

        Task<Result> UpdateUserBalance(UpdateUserBalanceDTO input);
    }
}