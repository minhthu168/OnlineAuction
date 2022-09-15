using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using eProject.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using eProject.ViewModel;

namespace eProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RatingController : Controller
    {
        private Data.DatabaseContext db;
        public RatingController(Data.DatabaseContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            //noi 3 table Auction & User & Rating
            var result = from a in db.Users
                         join b in db.Auctions on a.UserId equals b.UserId
                         join c in db.Ratings on b.AuctionId equals c.AuctionId
                         join d in db.Users on c.ReviewerId equals d.UserId
                         select new RatingViewModel
                         {
                             Receiver = a,
                             Auction = b,
                             Rating = c,
                             Reviewer = d
                         };
            return View(result.ToList());
        }       
    }
}
