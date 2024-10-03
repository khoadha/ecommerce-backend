﻿using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace EXE_API.Services.ApplicationUserService
{
    public class ApplicationUserService : IApplicationUserService {

        private readonly IApplicationUserRepository _userRepo;

        public ApplicationUserService(IApplicationUserRepository userRepo) {
            _userRepo = userRepo;
        }

        public async Task<ServiceResponse<List<ApplicationUser>>> GetUsers(){
            var serviceResponse = new ServiceResponse<List<ApplicationUser>>();
            var listUsers = await _userRepo.GetUsers();
            serviceResponse.Data = listUsers;
            return serviceResponse;
        }

        public async Task<ServiceResponse<ApplicationUser>> GetUserByEmail(string email) {
            var serviceResponse = new ServiceResponse<ApplicationUser>();
            var user = await _userRepo.GetUserByEmail(email);
            serviceResponse.Data = user;
            return serviceResponse;
        }
        public async Task<ServiceResponse<ApplicationUser>> GetUserById(string id)
        {
            var serviceResponse = new ServiceResponse<ApplicationUser>();
            try
            {
                var user = await _userRepo.GetUserById(id);
                serviceResponse.Data = user;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> IsUserExist(string id) {
            var serviceResponse = new ServiceResponse<bool>();
            var result = await _userRepo.IsUserExist(id);
            serviceResponse.Data = result;
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> GetWemadePoint(string id) {
            var serviceResponse = new ServiceResponse<int>();
            var result = await _userRepo.GetWemadePoint(id);
            serviceResponse.Data = result;
            return serviceResponse;
        }

        public bool Save() {
            return _userRepo.Save();
        }
    }
}
