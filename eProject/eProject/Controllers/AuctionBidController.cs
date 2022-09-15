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
    public class AuctionBidController : Controller
    {
        private readonly IAuctionBid serviceBid;       
        private readonly IAuctionServices service;
        public AuctionBidController(IAuctionBid serviceBid, IAuctionServices service)
        {
            this.serviceBid = serviceBid;
            this.service = service;
        }


        [HttpPost]
        public IActionResult Create(int AuctionId, int amount)
        {
            try
            {
                if (HttpContext.Session.GetString("acc") == null)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("acc"));
                    var auction = service.findOne(AuctionId);
                    if (amount <= auction.SalePrice)
                    {
                        TempData["Message"] = "Bid cannot be less than current price.";
                        return RedirectToAction("Details", "PageAuction", new { id = AuctionId });
                    }
                    var x = (amount - auction.MinimumBid) % (auction.BidIncremenent);
                    if (x != 0)
                    {
                        TempData["Message"] = "Bid does not match the Incremenent.";
                        return RedirectToAction("Details", "PageAuction", new { id = AuctionId });
                    }
                    if (user.UserId.Equals(auction.UserId))
                    {
                        TempData["Message"] = "You cannot bid on your own products.";
                        return RedirectToAction("Details", "PageAuction", new { id = AuctionId });
                    }
                    if (auction.EndDate > DateTime.Now)
                    {
                        auction.SalePrice = amount;
                        AuctionBid auctionBid = new AuctionBid
                        {
                            UserId = user.UserId,
                            AuctionId = AuctionId,
                            BidAmount = amount,
                            Time = DateTime.Now
                        };
                        serviceBid.CreateBid(auctionBid);
                        service.UpdateSalePrice(auction);
                        return RedirectToAction("Details", "PageAuction", new { id = AuctionId });
                    }
                    else
                    {
                        TempData["Message"] = "You cannot bid on expired products.";
                        return RedirectToAction("Details", "PageAuction", new { id = AuctionId });
                    }
                }
                
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return RedirectToAction("Details", "PageAuction", new { id = AuctionId });
        }

    }
}
