using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Models;
using BettingApplication.ViewModels;

namespace BettingApplication.Services.Interfaces
{
    public interface IAccountService
    {
        Task Register(AppUser user);
        Task<bool> IsUsernameExist(string username);
        Task<AppUser> GetUser(string username);
        Task<List<UsersViewModel>> GetUsersForActivate();
        Task<List<UsersViewModel>> GetUsersList();
        Task DeleteUser(string id);
        Task ActivateUser(AppUser applicationUser);
    }
}
