﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Shared
{
    public class GetPersonalUserDto
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImgPath { get; set; }
        public int WemadePoint { get; set; }
        public int? ManagedStoreId { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }

    public class UpdateUsernameDto
    {
        [Required]
        public string Username { get; set; }
    }
    public class UpdatePhoneNumberDto
    {
        [Required]
        public string PhoneNumber { get; set; }
    }

    public class UpdateAvatarDto
    {
        [Required]
        public IFormFile Imgpath { get; set; }
    }

}
