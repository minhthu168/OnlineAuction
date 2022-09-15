using eProject.Models;
using eProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eProject.ViewModel;

namespace eProject.Service
{
    public class AuctionServices : IAuctionServices
    {
        private readonly Data.DatabaseContext context;
        public AuctionServices(Data.DatabaseContext context)
        {
            this.context = context;
        }

        public bool ActiveAuction(int id)
        {
            var auction = context.Auctions.SingleOrDefault(a => a.AuctionId.Equals(id));            
            if (auction != null)
            {
                auction.Status = "Active";
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool InactiveAuction(int id)
        {
            var auction = context.Auctions.SingleOrDefault(a => a.AuctionId.Equals(id));
            if (auction != null)
            {
                auction.Status = "Inactive";
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateAuction(Auction auction)
        {
            auction.Status = "Approval";
            auction.SalePrice = auction.MinimumBid;         
            context.Auctions.Add(auction);  
            context.SaveChanges();
        }

        public bool DeleteAuction(int id)
        {
            var auction = context.Auctions.SingleOrDefault(a => a.AuctionId.Equals(id));
            if (auction != null)
            {
                context.Auctions.Remove(auction);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public CatAuctionUser DetailsAuction(int id)
        {
            var result = from a in context.Categories
                         join b in context.Auctions on a.CategoryId equals b.CategoryId
                         join c in context.Users on b.UserId equals c.UserId
                         select new CatAuctionUser
                         {
                             Category = a,
                             Auction = b,
                             User = c
                         };
            var auction = result.Where(x => x.Auction.AuctionId.Equals(id)).SingleOrDefault();
            return auction;
        }

        public Auction findOne(int id)
        {
            return context.Auctions.Find(id);
        }

        public List<CatAuctionUser> GetAuctions()
        {
            var result = from a in context.Categories
                         join b in context.Auctions on a.CategoryId equals b.CategoryId
                         join c in context.Users on b.UserId equals c.UserId
                         select new CatAuctionUser
                         {
                             Category = a,
                             Auction = b,
                             User = c
                         };
            return result.ToList();
        }      
        public void UpdateAuction(Auction auction)
        {
            var auc = context.Auctions.SingleOrDefault(a => a.AuctionId.Equals(auction.AuctionId));
            if (auc != null)
            {               
                auc.Description = auction.Description;
                if (auction.Image!="")
                {
                    auc.Image = auction.Image;
                }                               
                auc.Document = auction.Document;
                auc.StartDate = auction.StartDate;
                auc.EndDate = auction.EndDate;
                auc.BidIncremenent = auction.BidIncremenent;
                context.SaveChanges();
            }

        }
     

        public void UpdateSalePrice(Auction auction)
        {
            var auc = context.Auctions.SingleOrDefault(a => a.UserId.Equals(auction.AuctionId));
            if (auc != null)
            {
                auc.SalePrice = auction.SalePrice;
                context.SaveChanges();
            }
        }

        

        public List<CatAuctionUser> GetAuctionUser(int id)
        {
            var auc = context.Auctions.Where(a => a.UserId.Equals(id)).ToList();
            if (auc!= null)
            {
                var result = from a in context.Categories
                             join b in context.Auctions on a.CategoryId equals b.CategoryId
                             join c in context.Users on b.UserId equals c.UserId
                             select new CatAuctionUser
                             {
                                 Category = a,
                                 Auction = b,
                                 User = c
                             };
                result= result.Where(x => x.Auction.UserId.Equals(id));
                if (result!=null)
                {
                    return result.ToList();
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                return null;
            }

        }

        public List<CatAuctionUser> ListAuctions()
        {
            var result = from a in context.Categories
                         join b in context.Auctions on a.CategoryId equals b.CategoryId
                         join c in context.Users on b.UserId equals c.UserId
                         where (b.Status=="Active" || b.Status=="Inactive")
                         select new CatAuctionUser
                         {
                             Category = a,
                             Auction = b,
                             User = c
                         };
            return result.ToList();
        }
    }
}
