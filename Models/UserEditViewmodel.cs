using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class UserEditViewmodel
    {
        public long? Id {get;set;} 
        [Required(ErrorMessage = "Username can not be empty!")]
        [StringLength(50,MinimumLength = 3,ErrorMessage = "Username must be between 3 and 50 characters")]
        public string? FullName {get;set;} 

        [EmailAddress]
        public string? Email {get;set;}

        public string? Image {get;set;}

        [DataType(DataType.Password)]
        public string? Password {get;set;} 
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Passwords do not match!")]
        public string? ConfirmPassword {get;set;}
    }
}