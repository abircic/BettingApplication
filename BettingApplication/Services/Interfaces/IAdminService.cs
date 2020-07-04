using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingApplication.Services.Interfaces
{
    public interface IAdminService
    {
        Task ExportDatabase();
        Task ImportTwoPlayerDatabase();
    }
}
