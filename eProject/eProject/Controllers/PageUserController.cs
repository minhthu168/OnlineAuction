using eProject.Models;
using eProject.Repository;
using eProject.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Service
{
    public class PageUserController : Controller
    {       
        private readonly IUserServices serviceUser;
        private readonly IMessageServices serviceMess;

        public PageUserController(IUserServices serviceUser, IMessageServices serviceMess)
        {           
            this.serviceUser = serviceUser;
            this.serviceMess = serviceMess;

        }


        public IActionResult Index(int id)
        {
            try
            {
                if (HttpContext.Session.GetString("acc") == null)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    var model = serviceUser.GetUser(id);
                    return View(model);
                }
            }
            catch (Exception e)
            {

                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View(serviceUser.GetUser(id));
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (serviceUser.Edit(user)==true)
                    {
                        return RedirectToAction("Index",new { id = user.UserId });
                    }                                     
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Fail");
                }
            }
            catch (Exception e)
            {

                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }
        public IActionResult ResetPass()
        {           
            return View();
        }
        [HttpPost]
        public IActionResult ResetPass(int id,ResetPassword resetPassword)
        {

            var session = HttpContext.Session.GetString("acc");
            if (session==null)
            {              
                RedirectToAction("Login", "PageHome");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var model = serviceUser.GetUsers().SingleOrDefault(x=>x.Email.Equals(resetPassword.Email));                    
                    if (model!=null && id==model.UserId)
                    {
                        serviceUser.EditPass(resetPassword);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.msg = "Email does not match";
                        return View();
                    }

                ///
                }

            }

            return View();
        }
    }
}
