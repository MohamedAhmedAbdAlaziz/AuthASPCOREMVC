using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Core.Models;
using EmployeeManagement.ViewModels;

namespace EmployeeManagement.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager,
     SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    { 
        return View(); 
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
      if(ModelState.IsValid){
        var user= new ApplicationUser
        {
          UserName=model.Email,
          Email=model.Email,
          City= model.City
        };
        var result= await _userManager.CreateAsync(user,model.Password);
        if(result.Succeeded){
          await _signInManager.SignInAsync(user,isPersistent:false);
          return RedirectToAction("index","Home");

        }
       foreach (var item in result.Errors)
      {
        ModelState.AddModelError(string.Empty, item.Description);
      }
     
      }
      
      
        return View(model);
   
    }
[AcceptVerbs("Get","Post")]
[AllowAnonymous]
public async Task<IActionResult> IsEmailInUse(string Email){
  var user = await _userManager.FindByEmailAsync(Email);
  if(user ==null){
    return Json(true);
  }
  else{
    return Json($"Email {Email } is already in use");
  }

}


    [HttpGet]
    [AllowAnonymous]
     public IActionResult Login()
    { 
        return View();
          
        
    }
      [HttpPost]
     public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
    { 
      Console.Write("Mohamed");
      if(ModelState.IsValid){
         var result= await _signInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,false);
         
         if(result.Succeeded){

     if(!string.IsNullOrEmpty(returnUrl)&& Url.IsLocalUrl(returnUrl)){
      return Redirect(returnUrl);
     } 
     return RedirectToAction("index","Home");
         }
            
      
      ModelState.AddModelError(string.Empty,"Invalid Login Attempt");
      }
          
        return View(model);
    }
   
    [HttpPost]
     public async Task<IActionResult> Logout()
    { 
          await _signInManager.SignOutAsync();
          return RedirectToAction("index","Home");
    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied(){
      
      return View();
    }


}