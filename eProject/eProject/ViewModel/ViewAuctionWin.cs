using eProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.ViewModel
{
    public class ViewAuctionWin
    {
        public User Seller { get; set; }
        public Auction Auction { get; set; }
        public Winner Winner { get; set; }
        public User Buyer { get; set; }
    }
}
