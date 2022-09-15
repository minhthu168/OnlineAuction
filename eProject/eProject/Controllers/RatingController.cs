using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using eProject.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using eProject.ViewModel;

namespace eProject.Controllers
{
    public class RatingController : Controller
    {
        private Data.DatabaseContext db;
        public RatingController(Data.DatabaseContext db)
        {
            this.db = db;
        }

       
        public IActionResult Create(int id)
        {
            ViewBag.AuctionId = id;
            return View();
        }
        //create rate star + comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rating rating)
        {
            var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("acc"));
            var auction = db.Auctions.Find(rating.AuctionId);

            var comment = rating.Comment;           
            var star = rating.Star;
            if (auction.EndDate < DateTime.Now)//EXPIRED AUCTION
            {
                List<string> s =new List<string> { "deo", "dit", "dit me", "deo me","du me" };
                foreach (var item in s)
                {
                    if (comment.Contains(item))
                    {
                        comment = comment.Replace(item, "");
                    }
                }                      
                Rating ra = new Rating()
                {
                    AuctionId = auction.AuctionId,
                    Comment = comment,
                    Star = star,
                    CreateAt = DateTime.Now,
                    ReviewerId = user.UserId,
                    ReceiverId=auction.UserId
                };
                                          
                db.Ratings.Add(ra);
                db.SaveChanges();
                return View();
            }
            else
            {
                TempData["Message"] = "You cannot review bidding products.";
                return View();
            }

        }
    }
}
