using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GearStore.Models
{
    public class SignInViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class SignUpViewModel
    {
        public int CustomerID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string FullName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public bool Gender { get; set; }
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
    public class AcccountDetailsViewModel
    {
        public int CustomerID { get; set; }
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string FullName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }
        public bool Gender { get; set; }
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}