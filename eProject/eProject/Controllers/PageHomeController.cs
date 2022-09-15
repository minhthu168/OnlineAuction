using eProject.Models;
using eProject.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Controllers
{
    public class PageHomeController : Controller
    {
        private readonly IAuctionServices serviceAuction;       
        private readonly IAuctionBid serviceBid;
        private readonly IUserServices serviceUser;
        private readonly IMessageServices serviceMess;
        private readonly Data.DatabaseContext db;
        public PageHomeController(IAuctionServices serviceAuction, IAuctionBid serviceBid, IUserServices serviceUser, IMessageServices serviceMess, Data.DatabaseContext db)
        {
            this.serviceAuction = serviceAuction;            
            this.serviceBid = serviceBid;
            this.serviceUser = serviceUser;
            this.serviceMess = serviceMess;
            this.db = db;
        }       
        public IActionResult Index()
        {
            var model = serviceAuction.GetAuctions();
            ViewBag.End = model;
            ViewBag.Hot = model;
            //ViewBag.End = model.Where(x => (x.Auction.EndDate - DateTime.Now).TotalHours < 3).ToList().OrderBy(x=> (x.Auction.EndDate - DateTime.Now).TotalHours);
            //ViewBag.Rare = model.Where(x => x.Auction.Condition.Equals("Rare")).ToList();           
            //ViewBag.Hot = model.OrderByDescending(x => serviceBid.GetBids(x.Auction.AuctionId).Count());         
            ViewBag.Review = db.Ratings.ToList();
            ViewBag.User = serviceUser.GetUsers();
            return View();
        }
        [Route("Login")]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string user, string pass)
        {
            try
            {
                User account = serviceUser.CheckLogin(user, pass);
                if (account != null)
                {
                    HttpContext.Session.SetString("acc", JsonConvert.SerializeObject(account));
                    if (account.Role == "admin")
                    {
                        return RedirectToAction("Index", "Dashboard",new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.msg = "Incorrect account or password";
                }
            }
            catch (Exception e)
            {

                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();


        }
        [Route("Register")]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    serviceUser.Add(user);
                    return RedirectToAction("Login");
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
        public IActionResult Logout()
        {
            var session = HttpContext.Session.GetString("acc");
            if (session != null)
            {
                HttpContext.Session.Remove("acc");
                return RedirectToAction("Index", "PageHome");
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
