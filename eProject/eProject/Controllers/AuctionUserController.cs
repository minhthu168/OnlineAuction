using eProject.Models;
using eProject.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Controllers
{
    public class AuctionUserController : Controller
    {
        private readonly IAuctionServices serviceAuction;
        private readonly ICategoryServices serviceCat;
        private readonly IWinner serviceWin;
        private readonly IAuctionBid serviceBid;

        public AuctionUserController(IAuctionServices serviceAuction, ICategoryServices serviceCat, IWinner serviceWin, IAuctionBid serviceBid)
        {
            this.serviceAuction = serviceAuction;
            this.serviceCat = serviceCat;
            this.serviceWin = serviceWin;
            this.serviceBid = serviceBid;
        }
        public IActionResult MyAuction()
        {
            if (HttpContext.Session.GetString("acc") == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("acc"));
                ViewBag.user = user;
                var model = serviceAuction.GetAuctionUser(user.UserId);
                return View(model);
            }
        }
        public IActionResult WonAuction()
        {
            if (HttpContext.Session.GetString("acc") == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("acc"));
                ViewBag.user = user;
                var won = serviceWin.AuctionWins(user.UserId);
                return View(won);
            }
        }
        public IActionResult JoinAuction()
        {
            if (HttpContext.Session.GetString("acc") == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("acc"));
                ViewBag.user = user;
                var join = serviceBid.BidJoin(user.UserId);    
                var model=join.GroupBy(p => p.Auction.AuctionId).Select(grp => grp.FirstOrDefault());
                return View(model);
            }
        }

        [Route("CreateAuction")]
        public IActionResult Create()
        {
            var category = serviceCat.GetCategories();
            ViewBag.Category = new SelectList(category, "CategoryId", "CategoryName");
            return View();
        }
        [Route("CreateAuction")]
        [HttpPost]
        public IActionResult Create(Auction auction, IFormFile[] images, IFormFile document)
        {
            var category = serviceCat.GetCategories();
            ViewBag.Category = new SelectList(category, "CategoryId", "CategoryName");
            try
            {
                if (ModelState.IsValid)
                {
                    if (auction.StartDate> auction.EndDate || auction.EndDate < DateTime.Now)
                    {
                        TempData["msg"] = "EndDate must be bigger than StartDate and now!";
                        return View();
                    }
                    if (images != null)
                    {
                        string photo = "";
                        foreach (IFormFile item in images)
                        {
                            var path = Path.Combine("wwwroot/images", item.FileName);
                            var stream = new FileStream(path, FileMode.Create);
                            item.CopyToAsync(stream);
                            photo = photo + "," + item.FileName;
                        }
                        auction.Image = photo.TrimStart(',');
                    }
                    else
                    {
                        TempData["msg"] = "You have not selected a image!";
                        return View();

                    }

                    if (document == null)
                    {
                        auction.Document = null;
                    }
                    else
                    {
                        var path = Path.Combine("wwwroot/Document", document.FileName);
                        var stream = new FileStream(path, FileMode.Create);
                        document.CopyToAsync(stream);
                        auction.Document = document.FileName;
                    }
                    var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("acc"));
                    auction.UserId = user.UserId;
                    serviceAuction.CreateAuction(auction);
                    return RedirectToAction("MyAuction", new { id = user.UserId });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }
        [Route("EditAuction")]
        public IActionResult Edit(int id)
        {
            var model = serviceAuction.findOne(id);
            if (model.Status=="Approval" || model.StartDate < DateTime.Now)
            {
                var category = serviceCat.GetCategories();
                ViewBag.Category = new SelectList(category, "CategoryId", "CategoryName");
                return View(model);
            }
            else
            {
                TempData["msg"] = "You cannot edit an auction while the item is being auctioned";
                return RedirectToAction("MyAuction");
            }
            
        }
        [Route("EditAuction")]
        [HttpPost]
        public IActionResult Edit(Auction auction, IFormFile[] images, IFormFile document)
        {
            var category = serviceCat.GetCategories();
            ViewBag.Category = new SelectList(category, "CategoryId", "CategoryName");
            try
            {
                var auc = serviceAuction.findOne(auction.AuctionId);
                auction.Image = auc.Image;
                auction.Document = auc.Document;
                if (ModelState.IsValid)
                {
                    if (document != null)                   
                    {
                        var path = Path.Combine("wwwroot/Document", document.FileName);
                        var stream = new FileStream(path, FileMode.Create);
                        document.CopyToAsync(stream);
                        auction.Document = document.FileName;
                    }
                    if (images != null)
                    {
                        string photo = "";
                        foreach (IFormFile item in images)
                        {
                            var path = Path.Combine("wwwroot/images", item.FileName);
                            var stream = new FileStream(path, FileMode.Create);
                            item.CopyToAsync(stream);
                            photo = photo + "," + item.FileName;
                        }
                        auction.Image = photo.TrimStart(',');
                    }                    
                    serviceAuction.UpdateAuction(auction);
                    var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("acc"));
                    return RedirectToAction("MyAuction", new { id = user.UserId });
                }
                else
                {
                    return BadRequest();
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
                var model = serviceAuction.findOne(id);
                if (model.Status == "Approval" || model.StartDate < DateTime.Now)
                {
                    if (serviceAuction.DeleteAuction(id) == true)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["msg"] = "Delete failed!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["msg"] = "You cannot edit an auction while the item is being auctioned";
                    return RedirectToAction("MyAuction");
                }
                
            }
            catch (Exception e)
            {
                TempData["msg"] = e.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
