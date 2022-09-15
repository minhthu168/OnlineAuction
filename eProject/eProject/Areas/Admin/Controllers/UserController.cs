using eProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eProject.Repository;

namespace eProject.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class UserController : Controller
    {
        private readonly IUserServices service;
      
        public UserController(IUserServices service)
        {
            this.service = service;
        }
        
        public IActionResult Index()
        {
            var model = service.GetUsers().Where(a => a.Role == "user").ToList();
            return View(model);            
        }
        public IActionResult LockList()
        {
            return View(service.LockedAccount());
        }
        public IActionResult AdminAccount()
        {
            var model = service.GetUsers().Where(a => a.Role == "admin").ToList();
            return View(model);
        }       
        public IActionResult AccountLock(int id)
        {
            if (service.AccountLock(id) == true)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ViewData["msg"] = "Lock failed!";
                return RedirectToAction("Index");
            }            
        }
        public IActionResult UnLock(int id)
        {
            if (service.unLock(id) == true)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ViewData["msg"] = "Unlock failed!";
                return RedirectToAction("LockList");
            }
        }
        public IActionResult EditRole(int id)
        {
            if (service.EditRole(id) == true)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ViewData["msg"] = "Unlock failed!";
                return RedirectToAction("Index");
            }
        }
      
    }
}
