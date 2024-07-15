using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using WessleyMitchell.Web.DotNetCore.ViewRenderer;

namespace AudioStore.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;
        private IApplicationUserService _userService;
        private CartService CartService { get; set; }
        private IOrderDetailsService OrderDetailsService { get; }

        public ShoppingCartController(ShoppingCartService shoppingCartService,IOrderDetailsService orderDetailsService, IApplicationUserService userService, CartService cartService)
        {
            _userService = userService;
            OrderDetailsService = orderDetailsService;
            _shoppingCartService = shoppingCartService;
            CartService = cartService;
        }

        public IActionResult Index()
        {

            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);
            return View(cart);
        }
        public IActionResult Minus(int id)
        {
            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);
            var cartItem = cart.Find(c => c.Id == id);
            if (cartItem.Count <= 1)
            {
                cart.Remove(cartItem);
                _shoppingCartService.SetCart(cartId, cart);
            }
            else
            {
                cart.Find(c => c.Id == id).Count--;
                _shoppingCartService.SetCart(cartId, cart);
            }
            return RedirectToAction(nameof(Index));
        
        }
        public IActionResult Plus(int id)
        {
            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);
            var cartItem = cart.Find(c => c.Id == id);
            if (cartItem != null)
            {
                cartItem.Count++;
                _shoppingCartService.SetCart(cartId, cart);
            }
            return RedirectToAction(nameof(Index));
        }

        //GET
        [HttpGet]
        public IActionResult Checkout()
        {
            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);
            Tuple<ApplicationUser, IEnumerable<ShoppingCart>> tuple = new Tuple<ApplicationUser, IEnumerable<ShoppingCart>>(new ApplicationUser(), cart);
            return View(tuple);
        }

        // POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutAsync(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }
            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);

            await _userService.CreateUser(user);
            int id =await _userService.GetApplicationUserId();
            OrderDetails orderDetails = new OrderDetails()
            {
                ApplicationUser = user,
                Carts = cart,
                OrderDate = DateTime.Now,
                ApplicationUserID = id
            };
            foreach(var c in cart)
            {
                c.Id = 0;
            }
          
            await OrderDetailsService.CreateOrderDetails(orderDetails);
           // SendOrderEmailAsync(orderDetails);
           

            _shoppingCartService.SetCart(cartId.ToString(), new List<ShoppingCart>());
            return RedirectToAction("OrderConfirmation");
        } 

        public IActionResult OrderConfirmation()
        {
            return View();
        }
        #region Email
        private async Task SendOrderEmailAsync(OrderDetails orderDetails)
        {
            string senderEmail = "nkola.icic@gmail.com";
            string senderPassword = ("pcij ikvb fiyk omfq");
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            try
            {
                string path = "EmailTemplates/OrderDetails";
                string body = await HttpContext.RenderViewAsync(path, orderDetails, false);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(orderDetails.ApplicationUser.Email);
                mail.Subject = "Order Details";
                mail.Body=body;
                mail.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mail);
                Console.WriteLine("Order email sent successfully.");
              
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error sending email: " + ex.Message);

            }

        }
        #endregion
    }
}

