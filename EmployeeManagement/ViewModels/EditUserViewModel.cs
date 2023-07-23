using System.ComponentModel.DataAnnotations;
using Core.Models;
using EmployeeManagement.Utilites;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.ViewModels
{

    public class EditUserViewModel
    {
        
      public string Id {get;set;}

          [Required]

      public string UserName {get;set;}
        [DataType(DataType.EmailAddress)]
      public string Email {get;set;}


        [Required]
        [DataType(DataType.Password)]
      public string Password {get;set;}
    
      public string City {get;set;}
      public List<string> Claims {get;set;}  =new List<string>();
      public IList<string> Roles {get;set;}= new List<string>();



    
    }
}