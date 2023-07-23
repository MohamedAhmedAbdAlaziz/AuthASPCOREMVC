using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Core.Models;
using EmployeeManagement.ViewModels;
 
namespace EmployeeManagement.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
public class HomeController : Controller
{
 
 
     private readonly IEmployeeRepository _employeeRepository;
     [System.Obsolete("This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.", false)]
    private readonly IHostingEnvironment  _hostEnvironment;
[System.Obsolete("This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.", false)]
    public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment  hostEnvironment)
        {
            _employeeRepository = employeeRepository;
        _hostEnvironment = hostEnvironment;
    }
       

[AllowAnonymous]
    public IActionResult Index()
    {
        var models =_employeeRepository.GetAllEmployees();
                return View(models);
    }
[AllowAnonymous]

    public IActionResult Details(int? id)
    {

        var model =_employeeRepository.GetEmployee(id?? 1);
        if(model==null){
          Response.StatusCode=404;
          return View("EmployeeNotfound", id.Value);
        }  
                return View(model);
    }
    [HttpGet]
    [Authorize]
  public IActionResult Create()
    {
      return View();
    }
    [HttpPost ]
    [Obsolete]
   

    public IActionResult Create(EmployeeCreateViewModel model)
    {
        // string uniqueFileName ="";
        //  if(ModelState.IsValid){
        //     string uploadFolder= Path.Combine(_hostEnvironment.WebRootPath,"images"); 
        //     uniqueFileName=Guid.NewGuid().ToString()+" " +"_" +model.Photo.FileName;
        //     string filePath= Path.Combine(uploadFolder,uniqueFileName);
        //     model.Photo.CopyTo(new FileStream(filePath,FileMode.Create));

                 
        //  }
          string uniqueFileName ="";
        if(model.Photos.Count>0){
     foreach (IFormFile Photo in model.Photos)
         {
         
         if(ModelState.IsValid){
            string uploadFolder= Path.Combine(_hostEnvironment.WebRootPath,"images"); 
            uniqueFileName=Guid.NewGuid().ToString()+" " +"_" +Photo.FileName;
            string filePath= Path.Combine(uploadFolder,uniqueFileName);
            Photo.CopyTo(new FileStream(filePath,FileMode.Create));
                 
         }       
            }
         
        }
        
         Employee newEmployee= new Employee{
            Name=model.Name,
            Email=model.Email,
            Department= model.Department,
            PHotoPath=uniqueFileName,
         };
         _employeeRepository.AddEmployee(newEmployee);

    return RedirectToAction("Details",new {id=newEmployee.Id});
    }

      [HttpGet]
      [Authorize]
  public IActionResult Edit(int id)
    {
        Employee employee= _employeeRepository.GetEmployee(id);
        EmployeeEditViewModel employeeEditViewModel= new EmployeeEditViewModel{
  Id= employee.Id,
  Name= employee.Name,
  Email= employee.Email,
  Department= employee.Department,
  ExistingPhotoPath= employee.PHotoPath
        };

      return View(employeeEditViewModel);
    }
      [HttpPost ]
    [Obsolete]
    
    public IActionResult Edit(EmployeeEditViewModel model)
    {
       if(ModelState.IsValid){
        Employee emp= _employeeRepository.GetEmployee(model.Id);
        emp.Name=model.Name;
        emp.Email=model.Email;
        emp.Department=model.Department;
       
       if(model.Photo!= null){

        if(model.ExistingPhotoPath!= null){
          string filePath=  Path.Combine(_hostEnvironment.WebRootPath,"images",model.ExistingPhotoPath); 
         System.IO.File.Delete(filePath);
        }
           emp.PHotoPath= ProcessUploadedFile(model);
       }
       _employeeRepository.UpdateEmployee(emp);
          return RedirectToAction("index");
 
       }
       return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Obsolete]
    public string ProcessUploadedFile(EmployeeCreateViewModel model){
      string uniqueFileName ="";
         if(model.Photo!= null){
          
            string uploadFolder= Path.Combine(_hostEnvironment.WebRootPath,"images"); 
            uniqueFileName=Guid.NewGuid().ToString()+" " +"_" +model.Photo.FileName;
          string filePath= Path.Combine(uploadFolder,uniqueFileName) ;
          using(var fileStream = new FileStream(filePath,FileMode.Create)){

                 model.Photo.CopyTo(fileStream);

          }
          }   

      return uniqueFileName; 
     }
}
