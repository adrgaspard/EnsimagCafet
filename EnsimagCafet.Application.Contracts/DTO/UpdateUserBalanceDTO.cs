using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsimagCafet.Application.Contracts.DTO
{
    public record UpdateUserBalanceDTO
    {
        public string UserName { get; init; } = "";

        public float Balance { get; init; }
    }
}
