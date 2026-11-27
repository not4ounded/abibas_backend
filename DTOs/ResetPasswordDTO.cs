using abibas_back.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace abibas_back.DTOs
{
    public class ResetPasswordDTO
    {
        [Required] public string ResetToken { get; set; } = string.Empty;
        [Required, MinLength(8)] public string NewPassword { get; set; } = null!;  
        [Required, Compare(nameof(NewPassword), ErrorMessage = "Пароли не совпадают")] 
        public string ConfirmPassword { get; set; } = string.Empty;  
    }
}
