using eProject.Repository;
using eProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Controllers
{
    public class ProfileSellerController : Controller
    {
        private readonly IUserServices serviceUser;
        private readonly IMessageServices serviceMess;
        private Data.DatabaseContext db;
        private IAuctionServices serviceAuction;
        public ProfileSellerController(IUserServices serviceUser, IMessageServices serviceMess, Data.DatabaseContext db, IAuctionServices serviceAuction)
        {
            this.db = db;
            this.serviceUser = serviceUser;
            this.serviceMess = serviceMess;
            this.serviceAuction = serviceAuction;
        }
        public IActionResult Index(int id)
        {
            ViewBag.user = serviceUser.GetUser(id);
            var rating = db.Ratings.Where(x => x.ReceiverId.Equals(id));
            ViewBag.positive = rating.Where(x => x.Star >= 7).Count();
            ViewBag.neutral = rating.Where(x => x.Star < 7 && x.Star >= 5).Count();
            ViewBag.negative = rating.Where(x => x.Star < 5).Count();
            var model = serviceAuction.GetAuctionUser(id);
            return View(model);         
        }
        public IActionResult Feedback(int id)
        {
            ViewBag.user = serviceUser.GetUser(id);
            var rating = db.Ratings.Where(x => x.ReceiverId.Equals(id));
            ViewBag.positive = rating.Where(x => x.Star >= 7).Count();
            ViewBag.neutral = rating.Where(x => x.Star < 7 && x.Star >= 5).Count();
            ViewBag.negative = rating.Where(x => x.Star < 5).Count();
            //noi 3 table Auction & User & Rating
            var result = from a in db.Users
                         join b in db.Auctions on a.UserId equals b.UserId
                         join c in db.Ratings on b.AuctionId equals c.AuctionId
                         join d in db.Users on c.ReviewerId equals d.UserId
                         where b.UserId==id
                         select new RatingViewModel
                         {
                             Receiver = a,
                             Auction = b,
                             Rating = c,
                             Reviewer = d
                         };
            var stars = result;
            if (stars.Count() > 0)
            {
                ViewBag.RatingAvg = stars.Average(d => d.Rating.Star);
                ViewBag.RatingCount = stars.Count();
            }
            else
            {
                ViewBag.RatingAvg = 0;
                ViewBag.RatingCount = 0;
            }
            return View(result.ToList());
        }
    }
}
