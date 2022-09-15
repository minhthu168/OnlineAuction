using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;
using Stripe.Checkout;
using System.IO;

using log4net;

using System.Web;
using eProject.Models;
using eProject.Repository;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace eProject.Controllers
{
    public class CheckoutController : Controller
    {
        private ILog log = LogManager.GetLogger("HomeController");

        public Stream InputStream { get; private set; }
       
        private readonly IWinner serviceWin;
        private readonly IMessageServices serviceMess;
        private readonly IUserServices serviceUser;

        public CheckoutController(IWinner serviceWin, IMessageServices serviceMess, IUserServices serviceUser)
        {           

            this.serviceWin = serviceWin;
            this.serviceMess = serviceMess;
            this.serviceUser = serviceUser;
        }
        
        public IActionResult Index()
        {
            //var auction = serviceAu.findOne(id);
            //HttpContext.Session.SetString("auction", JsonConvert.SerializeObject(auction));
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = "sk_test_51KFFdMBgF6PMJrwmXm7MRrAXg55KQcY66YJq0knyrmjBise7O6lQsQhuL1Me2UXmWfAwyvrBblvuJ7zNDcBAYLAq00ii3gVt2P";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
                "card",
            },
                LineItems = new List<SessionLineItemOptions>
            {
                // products
                new SessionLineItemOptions {
                    Name ="Shoes" /*auction.Title*/,
                    Description ="color:white" /*auction.Description*/,
                    Amount =3000 /*auction.SalePrice*/,
                    Currency = "usd",
                    Quantity = 1,
                    // Product Images
                    Images = new List<string>
                    {
                        HttpUtility.UrlPathEncode("https://upload.wikimedia.org/wikipedia/commons/thumb/e/ee/.NET_Core_Logo.svg/1200px-.NET_Core_Logo.svg.png"),
                        HttpUtility.UrlPathEncode("https://miro.medium.com/max/2728/1*MfOHvI5b1XZKYTXIAKY7PQ.png")
                    }
                }
            },
                SuccessUrl = "http://localhost:21227/Checkout/Thanks", // Your website, Stripe will redirect to this URL
                CancelUrl = "http://localhost:21227/cancel", // Your websute, If user cancel payment, Stripe will redirect to this URL
                // Your configurations
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    // Maybe is Order ID, Customer ID, Descriptions,...
                    Metadata = new Dictionary<string, string>
                    {
                        // For example: Order ID, Description
                        { "Order_ID", "123456" }, // Order ID in your database
                        { "Description", "Comfortable cotton t-shirt - TD Shop - 2019" }
                    }
                }
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return View(session);
        }

        public ActionResult Thanks()
        {
            var auction = JsonConvert.DeserializeObject<Auction>(HttpContext.Session.GetString("auction"));
            var Seller = serviceUser.GetUser(auction.UserId);
            //cap nhat da checkout
            serviceWin.IsCheckOut(auction.AuctionId);
            //tim thong tin winner
            var win = serviceWin.GetWinners().SingleOrDefault(x => x.AuctionId.Equals(auction.AuctionId));
            var winner  = serviceUser.GetUser(win.WinnerId);
            //send mail to winner
            Message message = new Message
            {
                Title = string.Format("<h2>Successfully paid!</h2>"),
                Body = string.Format("You have just successfully paid for the product:<br/>" +
                " Auction products:<a href='~/PageAuction/Details/{0}'>{1}</a>.<br/>" +
                " Sale Price: {2} </br>" +
                " Please contact the seller to receive the goods !<br/>" +
                " Contact:<br/>" +
                " seller{3}<br/>Phone:{4}<br/>Email:{5}  ", auction.AuctionId,auction.Title, auction.SalePrice, Seller.FullName, Seller.Phone, Seller.Email),
                UserId = win.WinnerId,
                Date=DateTime.Now
            };
            serviceMess.SendMail(message, winner.Email);
            //save mail into DB
            serviceMess.AddMail(message);                  
            HttpContext.Session.Remove("auction");
            return View();
        }

        public ActionResult UpdatePaymentStatus()
        {
            try
            {                             
                StripeConfiguration.ApiKey = "sk_test_51KFFdMBgF6PMJrwmXm7MRrAXg55KQcY66YJq0knyrmjBise7O6lQsQhuL1Me2UXmWfAwyvrBblvuJ7zNDcBAYLAq00ii3gVt2P";
                Stream req = InputStream;
                req.Seek(0, System.IO.SeekOrigin.Begin);
                string json = new StreamReader(req).ReadToEnd();
                log.Info("Stripe live callback :" + json);

                // Get all Stripe events.
                var stripeEvent = EventUtility.ParseEvent(json);
                string stripeJson = stripeEvent.Data.RawObject + string.Empty;
                var childData = Charge.FromJson(stripeJson);
                var metadata = childData.Metadata;

                //int orderID = -1;
                //string strOrderID = string.Empty;
                //if (metadata.TryGetValue("Order_ID", out strOrderID))
                //{
                //    int.TryParse(strOrderID, out orderID);
                //    // Find your order from database.
                //    // For example:
                //    // Order order = db.Order.FirstOrDefault(x=>x.ID == orderID);

                //}

                switch (stripeEvent.Type)
                {
                    case Events.ChargeCaptured:
                    case Events.ChargeFailed:
                    case Events.ChargePending:
                    case Events.ChargeRefunded:
                    case Events.ChargeSucceeded:
                    case Events.ChargeUpdated:
                        var charge = Charge.FromJson(stripeJson);
                        string amountBuyer = ((double)childData.Amount / 100.0).ToString();
                        if (childData.BalanceTransactionId != null)
                        {
                            long transactionAmount = 0;
                            long transactionFee = 0;
                            if (childData.BalanceTransactionId != null)
                            {
                                // Get transaction fee.
                                var balanceService = new BalanceTransactionService();
                                BalanceTransaction transaction = balanceService.Get(childData.BalanceTransactionId);
                                transactionAmount = transaction.Amount;
                                transactionFee = transaction.Fee;
                            }

                            // My status, it is defined in my system.
                            int status = 0; // Wait

                            double transactionRefund = 0;

                            // Set order status.
                            if (stripeEvent.Type == Events.ChargeFailed)
                                status = -1; // Failed
                            if (stripeEvent.Type == Events.ChargePending)
                                status = -2; // Pending
                            if (stripeEvent.Type == Events.ChargeRefunded)
                            {
                                status = -3; // Refunded
                                transactionRefund = ((double)childData.AmountRefunded / 100.0);
                            }
                            if (stripeEvent.Type == Events.ChargeSucceeded)
                                status = 1; // Completed

                            // Update data
                            // For example: database
                            // order.Status = status;
                            // db.SaveChanges();
                                                                             
                        }
                        break;
                    default:
                        log.Warn("");
                        break;
                }
                return Json(new
                {
                    Code = -1,
                    Message = "Update failed."
                });
            }
            catch (Exception e)
            {
                log.Error("UpdatePaymentStatus: " + e.Message);
                return Json(new
                {
                    Code = -100,
                    Message = "Error."
                });
            }
        }


    }
}
