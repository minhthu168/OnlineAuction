using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eProject.Areas.Admin.Controllers
{
   // [Authorize(Roles = "Admin")]
   [Area("Admin")]
 
    public class CategoryController : Controller
    {
        private Repository.ICategoryServices services;
        public CategoryController(Repository.ICategoryServices _services)
        {
            services = _services;
        }
      
        public IActionResult Index()
        {
            var model = services.GetCategories();
            ViewBag.Cat = model;
            ViewBag.Category = new SelectList(model, "CategoryId", "CategoryName");
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    services.CreateCategory(category);
                    return RedirectToAction("Index");
                } else
                {
                    ViewBag.msg = "All input is reqired";
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(services.FindOne(id));
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    services.UpdateCategory(category);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update Fail");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            try
            {
                services.RemoveCategory(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }
    }
}
