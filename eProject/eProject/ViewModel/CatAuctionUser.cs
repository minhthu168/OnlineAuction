using eProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.ViewModel
{
    public class CatAuctionUser
    {
        public Category Category { get; set; }
        public Auction Auction { get; set; }
        public User User { get; set; }
        //public Rating Rating { get; set; }
             
    }
}
