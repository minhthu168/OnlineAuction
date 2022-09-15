using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using System.IO;
namespace eProject.Areas.Admin.Controllers
{
    [Area("Admin")]   
    public class NewsController : Controller
    {
        private Repository.INews services;
        public NewsController(Repository.INews _services)
        {
            services = _services;
        }
      
        public IActionResult Index()
        {
            return View(services.GetNews());
        }

        //them moi
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]      
        public IActionResult Create(Models.News news, IFormFile file)
        {           
            try
            {
                if (ModelState.IsValid)
                {
                    if (file.Length > 0) //neu file co ton tai
                    {
                        var filePath = Path.Combine("wwwroot/images", file.FileName);//luu ten file upload theo dung duong dan wwwroot/images (neu co file trung ten thi ghi de file cu)
                        var stream = new FileStream(filePath, FileMode.Create);
                        file.CopyToAsync(stream);//copy stream luu vo file 
                        news.Image = "images/" + file.FileName;
                        services.CreateNews(news);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Fail");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }

        public IActionResult Details(int id)
        {
            return View(services.OneNews(id));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {        
            return View(services.OneNews(id));
        }

        [HttpPost]
        public IActionResult Edit(Models.News news, IFormFile file)
        {        
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        var filePath = Path.Combine("wwwroot/images", file.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        file.CopyToAsync(stream);
                        news.Image = "images/" + file.FileName;
                        services.UpdateNews(news);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        news.Image = services.OneNews(news.NewsId).Image;
                        services.UpdateNews(news);
                        return RedirectToAction("Index");                      
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Fail");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            try
            {
                services.RemoveNews(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View();
        }
    }
}
