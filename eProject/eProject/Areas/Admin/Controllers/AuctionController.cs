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

namespace eProject.Areas.Admin.Controllers
{
    [Area("Admin")]
  
    public class AuctionController : Controller
    {
        private readonly IAuctionServices service;       
        private readonly IWinner serviceWin;
        private readonly ICategoryServices serviceCat;
        public AuctionController(IAuctionServices service,IWinner serviceWin, ICategoryServices serviceCat)
        {
            this.service = service;          
            this.serviceWin = serviceWin;
            this.serviceCat = serviceCat;
           
        }  
       
        public IActionResult Index(string list)
        {            
            var model = service.GetAuctions();           
            if (list=="Approval")
            {
                model= model.Where(x => x.Auction.Status == "Approval").OrderByDescending(x=>x.Auction.StartDate).ToList();
            }else if (list == "Active")
            {
                model= model.Where(x => x.Auction.Status == "Active").OrderByDescending(x => x.Auction.StartDate).ToList();
            }else if (list == "Inactive")
            {
                model= model.Where(x => x.Auction.Status.Equals("Inactive")).OrderByDescending(x=>x.Auction.EndDate).ToList();
            }
            else if (list == "Lock")
            {
                model= model.Where(x => x.Auction.Status == "Lock").ToList();
            }
            else
            {
                model = service.GetAuctions();
            }
            //search            
            return View(model);
        }       
        public IActionResult Details(int id)
        {
            var model = service.DetailsAuction(id);
            if (serviceWin.AuctionWin(id)!=null)
            {
                ViewBag.win = serviceWin.AuctionWin(id);
                return View(model);
            }
            return View(model);
        }        
        public IActionResult Delete(int id)
        {
            try
            {
                if (service.DeleteAuction(id) == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = "Delete failed!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["msg"] = e.Message;
            }
            return RedirectToAction("Index");
        }
        public IActionResult ActiveAuction(int id)
        {
            try
            {
                if (service.ActiveAuction(id) == true)
                {
                    return RedirectToAction("Index",new { list="Approval"});
                }
                else
                {
                    ViewData["msg"] = "Active failed!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ViewData["msg"] = e.Message;
            }
            return RedirectToAction("Index");
        }
        public IActionResult InactiveAuction(int id)
        {
            try
            {
                if (service.ActiveAuction(id) == true)
                {
                    return RedirectToAction("Index", new { list = "Active" });
                }
                else
                {
                    ViewData["msg"] = "Lock failed!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ViewData["msg"] = e.Message;
            }
            return RedirectToAction("Index");
        }                 

    }
}
