﻿using System.Diagnostics.CodeAnalysis;

namespace BeautyGuide.Models
{
    public class UserViewModel
    {
      
            public List<UserDetail> UserDetailList { get; set; }
        }
        public class UserDetail
        {
            public int Id { get; set; }
            public int RoleId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }

            //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Chỉ chấp nhận các tập tin JPG và PNG.")]
            public IFormFile? AvatarFile { get; set; }
            
            [AllowNull]
            public string? Avatar { get; set; }
            [AllowNull]
         
            public DateTime? LastLogin { get; set; }
            [AllowNull]
            public DateTime? LastLogout { get; set; }
          
            [AllowNull]
            public DateTime? CreatedAt { get; set; }

            [AllowNull]
            public DateTime? UpdatedAt { get; set; }

            [AllowNull]
            public DateTime? DeletedAt { get; set; }
            [AllowNull]
            public string? NameRole { get; set; }
        }
    
}
