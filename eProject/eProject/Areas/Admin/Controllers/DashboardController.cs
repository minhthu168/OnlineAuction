using eProject.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IAuctionServices serviceAuction;
        private readonly ICategoryServices serviceCat;        
        private readonly IUserServices serviceUser;      
        private readonly IWinner serviceWin;

        public DashboardController(IAuctionServices serviceAuction, ICategoryServices serviceCat,IUserServices serviceUser, IWinner serviceWin)
        {
            this.serviceAuction = serviceAuction;
            this.serviceCat = serviceCat;          
            this.serviceUser = serviceUser;         
            this.serviceWin = serviceWin;

        }
        public IActionResult Index()
        {
            //count all catergorys
            ViewBag.cat = serviceCat.GetCategories().Count();
            //count all auctions
            ViewBag.auction = serviceAuction.GetAuctions().Count();
            //count all users
            ViewBag.user = serviceUser.GetUsers().Count();
            //count all auctions has checkout
            ViewBag.Checkout = serviceWin.GetWinners().Where(x => x.IsCheckOut == true).Count();
            return View();
        }
    }
}
