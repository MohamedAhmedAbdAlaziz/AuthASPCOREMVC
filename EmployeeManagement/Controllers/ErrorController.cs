using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Core.Models;
using EmployeeManagement.ViewModels;
 
namespace EmployeeManagement.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [Route("Error/{statusCode}")]
  public IActionResult HttpStatusCodeHander(int statusCode){
  var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

     switch(statusCode)
{
  case 404: ViewBag.ErrorMessage="Soory, the resources you requested could not be found";
  ViewBag.Path= statusCodeResult.OriginalPath;
  ViewBag.Qs= statusCodeResult.OriginalQueryString;
    break;

}
return View("NotFound");
  } 

  [Route("Error")]
  [AllowAnonymous]
  public IActionResult Error(){
    var exceptionDetails= HttpContext.Features.Get<IExceptionHandlerPathFeature>();
     
      _logger.LogError($".ExceptionPath-{ exceptionDetails.Path} ... ExceptionMessage-{exceptionDetails.Error.Message}...   Stacktrace-{exceptionDetails.Error.StackTrace} ");


    ViewBag.ExceptionPath=exceptionDetails.Path;
    ViewBag.ExceptionMessage=exceptionDetails.Error.Message;
    ViewBag.Stacktrace=exceptionDetails.Error.StackTrace;

     return View("Error");
  }

 
}