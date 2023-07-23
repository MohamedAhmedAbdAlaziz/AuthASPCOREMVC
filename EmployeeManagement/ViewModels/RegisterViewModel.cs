using System.ComponentModel.DataAnnotations;
using Core.Models;
using EmployeeManagement.Utilites;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.ViewModels
{

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse", controller:"Account")]
        [ValidEmailDomian(allowedDomain:"pragim.com",ErrorMessage ="Email domain must be pragim.com")]
      public string Email {get;set;}


        [Required]
        [DataType(DataType.Password)]
      public string Password {get;set;}


        [DataType(DataType.Password)]
        [Display(Name ="confirm password")]
        [Compare("Password",ErrorMessage ="Password and confirm password do to not match")]
      public string ConfirmPassword {get;set;}

     
      public string City {get;set;}


    
    }
}