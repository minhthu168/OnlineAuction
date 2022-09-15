using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eProject.Models;

namespace eProject.Repository
{
    public interface IAuctionBid
    {
        public List<AuctionBid> GetAuctionBids();
        public List<ViewModel.ViewAuctionBid> BidJoin(int id);
        public AuctionBid FindOne(int id);

        public List<AuctionBid> GetBids(int id);
        public void CreateBid(AuctionBid newAuctionBid);
        

       
    }
}
