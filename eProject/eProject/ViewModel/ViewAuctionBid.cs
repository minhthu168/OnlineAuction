using eProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.ViewModel
{
    public class ViewAuctionBid
    {
        public Auction Auction { get; set; }
        public AuctionBid AuctionBid { get; set; }
        public User User { get; set; }
    }
}
