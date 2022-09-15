using eProject.Models;
using eProject.Repository;
using eProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Service
{
    public class AuctionBidServices : Repository.IAuctionBid
    {
        private Data.DatabaseContext context;
        public AuctionBidServices(Data.DatabaseContext _context)
        {
            context = _context;
        }
        public void CreateBid(AuctionBid newAuctionBid)
        {      
            context.AuctionBids.Add(newAuctionBid);
            context.SaveChanges();
        }

        public AuctionBid FindOne(int id)
        {
            AuctionBid auctionBid = context.AuctionBids.SingleOrDefault(x => x.AuctionId.Equals(id));
            if(auctionBid != null)
            {
                return auctionBid;
            } else
            {
                return null;
            }
        }

        public List<AuctionBid> GetAuctionBids()
        {
            var res = context.AuctionBids.ToList();
            if(res != null)
            {
                return res;
            } else
            {
                return null;
            }
        }       
        public List<AuctionBid> GetBids(int id)
        {
            var auctionBid = from a in context.AuctionBids               
                             where a.AuctionId==id
                             orderby a.Time descending
                             select a;           
            if (auctionBid != null)
            {
                return auctionBid.ToList();
            }
            else
            {
                return null;
            }
        }

   
       public List<ViewAuctionBid> BidJoin(int id)
        {
            var result = from a in context.Auctions
                         join b in context.AuctionBids on a.AuctionId equals b.AuctionId
                         join c in context.Users on b.UserId equals c.UserId    
                         where b.UserId==id                        
                         select new ViewAuctionBid
                         {
                             Auction = a,
                             AuctionBid = b,
                             User = c
                         };           
            return result.ToList();
        }
    }
}
