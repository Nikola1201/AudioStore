using AudioStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task CreateUser(ApplicationUser user);
        Task DeleteUser(int? id);
        Task<int> GetApplicationUserId();
    }
}
