﻿using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Dtos.RequestModels
{
    public class MReq_UserLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }        
    }
}
