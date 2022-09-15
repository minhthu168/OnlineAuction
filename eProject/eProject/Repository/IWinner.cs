using eProject.Models;
using eProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Repository
{
    public interface IWinner
    {
        List<Winner> GetWinners();
        void UpdateWinner(int AuctionId, int winnerId);
        void IsCheckOut(int AuctionId);
        Winner findWin(int id);

        ViewAuctionWin AuctionWin(int AuctionId);
        List<ViewAuctionWin> AuctionWins(int userId);

    }
}
