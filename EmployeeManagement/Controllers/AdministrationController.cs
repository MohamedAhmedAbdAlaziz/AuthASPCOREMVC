using System.Security.Claims;
using Core.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
  //[Authorize(Roles="Admin")]
 //[Authorize(Policy="Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
         private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }

    [HttpGet]
  public IActionResult ListUsers()
    {
        var users = _userManager.Users;
        return View(users); 
    }

    [HttpGet]
    public IActionResult CreateRole()
    {
      //  User.Claims.Where(x=>x.Type==ClaimTypes.);
        return View();
    }

    [HttpPost] 
    public async Task<IActionResult> CreateRole(CreateRoleModel model)
    {
        if (ModelState.IsValid)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = model.RoleName
            };
            IdentityResult result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return RedirectToAction("index", "Home");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

          
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ListRoles()
    {
        var roles = _roleManager.Roles;
        return View(roles);
    }
    [HttpGet]
    public async Task<IActionResult> EditRole(string id)
    {
        var role = await  _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            ViewBag.ErrorMessage = $"Role with Id ={id} can not founded";
            return View("Notfound");
        }
        var model = new EditRoleViewModel
        {
            Id = role.Id,
            RoleName = role.Name
        };
          foreach (var user in _userManager.Users)
            {
                if(await _userManager.IsInRoleAsync(user,role.Name)){
                     model.users.Add(user.UserName);
                }
            }

        return View(model);

    }
     [HttpPost]
    public async Task<IActionResult> EditRole(EditRoleViewModel model)
    {
        var role = await  _roleManager.FindByIdAsync(model.Id);
        if (role == null)
        {
            ViewBag.ErrorMessage = $"Role with Id ={model.Id} can not founded";
            return View("Notfound");
        }

        role.Name=model.RoleName;
        var result= await _roleManager.UpdateAsync(role);

        if(result.Succeeded){
            return RedirectToAction("ListRoles");
        }
        foreach (var item in result.Errors)
        {
            ModelState.AddModelError("", item.Description);
        }
 await Refresh(User);
        return View(model);

    }

     [HttpGet]
    public async Task<IActionResult> EditUserInRole(string roleId){
        ViewBag.roleId= roleId;
        var role= await _roleManager.FindByIdAsync(roleId);
        if(role==null){
            ViewBag.ErrorMessage=$"Role with Id = {roleId} cannot be found ";
            return View("NotFound"); 
        }
         var model = new List<UserRoleViewModel>();
      
          foreach (var user in _userManager.Users)
            {
                 var userRoleViewModel= new UserRoleViewModel
                 {
                       UserId=user.Id,
                       UserName= user.UserName
                 };

                 if(await _userManager.IsInRoleAsync(user,role.Name)){
                    userRoleViewModel.IsSelected=true;
                 }
                 else{
                    userRoleViewModel.IsSelected=false;
                 }
                 model.Add(userRoleViewModel);
            }

           return View(model);

    }
    [HttpPost]
    public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model , string roleId)
    {
          
      var role= await _roleManager.FindByIdAsync(roleId);
        if(role==null){
            ViewBag.ErrorMessage=$"Role with Id = {roleId} cannot be found ";
            return View("NotFound"); 
        }
        for (int i = 0; i < model.Count; i++)
        {
             var user = await _userManager.FindByIdAsync(model[i].UserId);
             IdentityResult result= null;
             if(model[i].IsSelected && !(await _userManager.IsInRoleAsync(user,role.Name))){
                result= await _userManager.AddToRoleAsync(user,role.Name);
                  
             }
             else  if(!model[i].IsSelected && (await _userManager.IsInRoleAsync(user,role.Name))){
                result= await _userManager.RemoveFromRoleAsync(user,role.Name);
                  
             }
             else{
                continue;
             }
             if(result.Succeeded){
                if(i<(model.Count-1)){
                    continue;
                }
                else{
                    return RedirectToAction("EditRole", new {Id=roleId});
                }
             }
        }

         await Refresh(User);

        return RedirectToAction("EditRole", new {Id=roleId});
    
    
    }

 [HttpGet]
    public async Task<IActionResult> EditUser(string userId){
        
        var user= await _userManager.FindByIdAsync(userId);
        if(user==null){
            ViewBag.ErrorMessage=$"Role with Id = {userId} cannot be found ";
            return View("NotFound"); 
        }
        var usersCliams= await _userManager.GetClaimsAsync(user);
        var usersRoles= await _userManager.GetRolesAsync(user);
       var model = new EditUserViewModel
       {
        Id=user.Id ,
        Email=user.Email ,
        UserName=user.UserName,
        City=user.City,     
      //  Claims=  usersCliams.Select(x=>x.Value).ToList(),
        Claims=  usersCliams.Select(x=> x.Type +" : "+ x.Value).ToList(),
        Roles= usersRoles


       };
        await Refresh(User);
           return View(model);

    }
[HttpPost]
    public async Task<IActionResult> EditUser(EditUserViewModel model){
        
        var user= await _userManager.FindByIdAsync(model.Id);
        if(user==null){
            ViewBag.ErrorMessage=$"Role with Id = {model.Id} cannot be found ";
            return View("NotFound"); 
        }
        user.Email= model.Email;
        user.UserName= model.UserName;
        user.City= model.City;
        var result= await _userManager.UpdateAsync(user);
       if(result.Succeeded){
        return RedirectToAction("ListUsers");
       }
       foreach (var item in result.Errors)
       {
        ModelState.AddModelError("", item.Description);
       }

           return View(model);

    }
    
[HttpPost]
  public async Task<IActionResult> DeleteUser(string userId)
  {
     
  var user= await _userManager.FindByIdAsync(userId);
         if(user==null){
            ViewBag.ErrorMessage=$"Role with Id = {userId} cannot be found ";
            return View("NotFound"); 
        }
        try
        {
              var result = await _userManager.DeleteAsync(user);
        if(result.Succeeded){
          return RedirectToAction("ListUsers");
        }
  foreach (var item in result.Errors)
       {
        ModelState.AddModelError("", item.Description);
       }
  return View("ListUsers");
        }
        catch (System.Exception)
        {
            
            ViewBag.ErrorTitle=$"{user.UserName} role is in use "; 
           ViewBag.ErrorMessage=$"{user.UserName} role  can't be delete as there are users  "+
           $"in this role. If you want to delete this role. plz remove the users "; 
         return View("~/Views/Error/Error.cshtml");
        }
      
  }
 

[HttpPost]
[Authorize(Policy ="DeleteRolePolicy")]
  public async Task<IActionResult> DeleteRole(string roleId)
  {  
  var role= await _roleManager.FindByIdAsync(roleId);
         if(role==null){
            ViewBag.ErrorMessage=$"Role with Id = {roleId} cannot be found ";
            return View("NotFound"); 
        }
        try
        {
         var result = await _roleManager.DeleteAsync(role);
         if(result.Succeeded){
          return RedirectToAction("ListRoles");
          }
       foreach (var item in result.Errors)
       {
        ModelState.AddModelError("", item.Description);
       }
        await Refresh(User);
         return View("ListRoles");
        }
        catch (System.Exception)
        {
           ViewBag.ErrorTitle=$"{role.Name} role is in use "; 
           ViewBag.ErrorMessage=$"{role.Name} role  can't be delete as there are users  "+
           $"in this role. If you want to delete this role. plz remove the users "; 
          return View("~/Views/Error/Error.cshtml");
           
        }
     
  }
  

 [HttpGet]
    public async Task<IActionResult> ManageUserRoles(string userId){
        ViewBag.userId=userId;
            var user= await _userManager.FindByIdAsync(userId);
          if(user==null){
            ViewBag.ErrorMessage=$"Role with Id = {userId} cannot be found ";
            return View("NotFound"); 
        }
        var model = new List<UserRolesViewModel>();
        foreach (var role in _roleManager.Roles)
        {
             var userRoleViewModel= new UserRolesViewModel{
                RoleId=role.Id,
                RoleName= role.Name
             };
             if(await _userManager.IsInRoleAsync(user,role.Name)){
                userRoleViewModel.IsSelected=true;
             }
             else{
            userRoleViewModel.IsSelected=false;
             }
             model.Add(userRoleViewModel);
        }

     return  View(model);

    }
    [HttpPost]
    public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model , string userId)
    {
          
      var user= await _userManager.FindByIdAsync(userId);
        if(user==null){
            ViewBag.ErrorMessage=$"Role with Id = {userId} cannot be found ";
            return View("NotFound"); 
        }
        var roles=await _userManager.GetRolesAsync(user);
        var result=await _userManager.RemoveFromRolesAsync(user,roles);
        if(!result.Succeeded){
            ModelState.AddModelError("","Cannot remove user existing roles");
            return View(model);
        }
        var selectedRoles= model.Where(x=>x.IsSelected==true).Select(x=>x.RoleName);
        result= await _userManager.AddToRolesAsync(user,selectedRoles);
        if(!result.Succeeded){
            ModelState.AddModelError("","Cannot remove user existing roles");
            return View(model);
        }
        
       await Refresh(User);

        return RedirectToAction("EditUser", new {userId=userId});
    }

     [HttpGet]
    public async Task<IActionResult> ManageUserClaims(string userId) {
          
            var user= await _userManager.FindByIdAsync(userId);
          if(user==null){
            ViewBag.ErrorMessage=$"Role with Id = {userId} cannot be found ";
            return View("NotFound"); 
        }
        var existingUserCliams= await _userManager.GetClaimsAsync(user);
        var model = new UserClaimsViewModel
        {
            userId=userId

        };
       foreach (var claim in ClaimsStore.AllClaims)
       {
        UserClaim userClaim= new UserClaim
        {
            ClaimType=claim.Type
        };
        if(existingUserCliams.Any(x=>x.Type==claim.Type && x.Value=="true")){
            userClaim.IsSelected=true;
        }
        model.Claims.Add(userClaim);
       }
      return View(model);
    }
     [HttpPost]
    public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model) {
     //    User.Claims.Where(x=>x.Type==ClaimTypes.); 
            var user= await _userManager.FindByIdAsync(model.userId);
          if(user==null){
            ViewBag.ErrorMessage=$"Role with Id = {model.userId} cannot be found ";
            return View("NotFound"); 
        }

        var claims= await _userManager.GetClaimsAsync(user);
        var result= await _userManager.RemoveClaimsAsync(user,claims);
     
       if(!result.Succeeded){
            ModelState.AddModelError("","Cannot remove user existing claims");
            return View(model);
        }
       // var selectedClaims= model.Claims.Where(x=>x.IsSelected==true).Select(x => new Claim( x.ClaimType, x.ClaimType));
         var selectedClaims= model.Claims
         .Select(x => new Claim( x.ClaimType, x.IsSelected ? "true" :"false"));
        
        result= await _userManager.AddClaimsAsync(user,selectedClaims);
        if(!result.Succeeded){
            ModelState.AddModelError("","Cannot Add Selected Cliams");
            return View(model);
        }
          await Refresh(User);
        

        return RedirectToAction("EditUser", new {userId=model.userId});
       
    }

    public  async  Task Refresh(ClaimsPrincipal user){
          var user1 = await _userManager.GetUserAsync(User);
        await _signInManager.RefreshSignInAsync(user1);
    }

}


}