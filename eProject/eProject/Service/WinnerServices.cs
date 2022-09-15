using eProject.Models;
using eProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Service
{
    public class WinnerServices : Repository.IWinner
    {
        private readonly Data.DatabaseContext context;
        public WinnerServices(Data.DatabaseContext context)
        {
            this.context = context;
        }
        public void IsCheckOut(int AuctionId)
        {
            var win = context.Winners.SingleOrDefault(a => a.AuctionId.Equals(AuctionId));
            var m = context.Winners.Find(AuctionId);
            if (win != null)
            {
                win.IsCheckOut = true;
                context.SaveChanges();
            }

        }

        public Winner findWin(int id)
        {
            return context.Winners.Find(id);
        }


        public void UpdateWinner(int AuctionId, int winnerId)
        {
            var auc = context.Auctions.SingleOrDefault(a => a.AuctionId.Equals(AuctionId));

            var win = context.Winners.SingleOrDefault(a => a.AuctionId.Equals(AuctionId));
            if (auc.EndDate <= DateTime.Now && win == null)
            {
                Winner winner = new Winner
                {
                    AuctionId = AuctionId,
                    WinnerId = winnerId,
                    IsCheckOut = false
                };
                context.Winners.Add(winner);
                context.SaveChanges();
            }
        }

        public ViewAuctionWin AuctionWin(int AuctionId)
        {
            var win = context.Winners.Where(a => a.AuctionId.Equals(AuctionId)).ToList();
            if (win != null)
            {
                var result = from a in context.Users
                             join b in context.Auctions on a.UserId equals b.UserId
                             join c in context.Winners on b.AuctionId equals c.AuctionId
                             join d in context.Users on c.WinnerId equals d.UserId
                             select new ViewAuctionWin
                             {
                                 Seller=a,
                                 Auction = b,
                                 Winner = c,
                                 Buyer=d
                             };
                result = result.Where(x => x.Auction.AuctionId.Equals(AuctionId));
                if (result != null)
                {
                    return result.SingleOrDefault();
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

        public List<Winner> GetWinners()
        {
            return context.Winners.ToList();
        }

        public List<ViewAuctionWin> AuctionWins(int userId)
        {
            var win = context.Winners.Where(a => a.WinnerId.Equals(userId)).ToList();
            if (win != null)
            {
                var result = from a in context.Users
                             join b in context.Auctions on a.UserId equals b.UserId
                             join c in context.Winners on b.AuctionId equals c.AuctionId
                             join d in context.Users on c.WinnerId equals d.UserId
                             select new ViewAuctionWin
                             {
                                 Seller = a,
                                 Auction = b,
                                 Winner = c,
                                 Buyer = d
                             };
                result = result.Where(x => x.Winner.WinnerId.Equals(userId));
                if (result != null)
                {
                    return result.ToList();
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }
}
