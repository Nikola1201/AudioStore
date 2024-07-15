using AudioStore.DataAccess;
using AudioStore.Models;
using AudioStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private ApplicationDbContext _context;
        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task CreateUser(ApplicationUser user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int? id)
        {
            var appuser = await _context.ApplicationUsers.FindAsync(id);
            if (appuser != null)
            {
                _context.Remove(appuser);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Application user with ID {id} doesn't exist!");

            }
        }

        public async Task<int> GetApplicationUserId()
        {
            int id = await _context.ApplicationUsers.OrderByDescending(x => x.ApplicationUserID).Select(x => x.ApplicationUserID).FirstOrDefaultAsync();
            return id;
        }



    }
}
