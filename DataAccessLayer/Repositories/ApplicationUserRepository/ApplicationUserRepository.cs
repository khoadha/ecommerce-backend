﻿using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories {
    public class ApplicationUserRepository : IApplicationUserRepository {

        private readonly EXEContext _context;

        public ApplicationUserRepository(EXEContext context) {
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetUsers() {
            var listUsers = await _context.Users.ToListAsync();
            return listUsers;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email) {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.NormalizedEmail == email.ToUpper());
            return user;
        }
        public async Task<ApplicationUser> GetUserById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
            return user;
        }

        public async Task<bool> IsUserExist(string id) {
            var result = await _context.Users.AnyAsync(a => a.Id == id);
            return result;
        }

        public async Task<int> GetWemadePoint(string userId) {
            int point = 0;
            var user = await _context.Users.FirstOrDefaultAsync(a=>a.Id == userId);
            if(user is not null) {
                point = user.WemadePoint;
            }
            return point;
        }

        public bool Save() {
            int save = _context.SaveChanges();
            return save > 0;
        }
    }
}
