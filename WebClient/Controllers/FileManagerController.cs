using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Helper;

namespace WebClient.Controllers
{
    public class FileManagerController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public FileManagerController (IWebHostEnvironment env)
        {
            _environment = env;              
        }
        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(file.FileName);
                var rootPath = _environment.WebRootPath;
                var folder = "asset/img/product-img";
                //var filePath = Path.Combine("~/asset/img/product-img", fileName);
                try
                {
                    var filePath = Path.Combine(rootPath, folder);
                    var returnUrl = "/" + folder + "/" + fileName;
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = Path.Combine(filePath, fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        return Ok(new DataResult<string>("file already existed!",true, returnUrl));  
                    }
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                    
                    return Ok(new DataResult<string>("upload file successfully", true, returnUrl));
                }
                catch (Exception ex)
                {
                    return Ok(new DataResult<string>("upload file failed", false, null));
                }
            }
            return BadRequest();
        }
    }
}
