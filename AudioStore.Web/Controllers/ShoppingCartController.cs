using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailService;

        public ShoppingCartController(IUnitOfWork unitOfWork, IEmailSender emailService, ShoppingCartService shoppingCartService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _shoppingCartService = shoppingCartService;
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
            var cartItem = cart?.Find(c => c.Id == id);
            if (cartItem != null)
            {
                if (cartItem.Count <= 1)
                {
                    cart.Remove(cartItem);
                    _shoppingCartService.SetCart(cartId, cart);
                }
                else
                {
                    cartItem.Count--;
                    _shoppingCartService.SetCart(cartId, cart);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Plus(int id)
        {
            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);
            var cartItem = cart?.Find(c => c.Id == id);
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
            if (cart == null)
            {
                TempData["error"] = "Cart is empty.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var c in cart)
            {
                if (!InStock(c))
                {
                    TempData["error"] = $"{c.Product.Name} out of stock";
                    return RedirectToAction(nameof(Index));
                }
            }

            Tuple<Customer, IEnumerable<ShoppingCartItem>> tuple = new Tuple<Customer, IEnumerable<ShoppingCartItem>>(new Customer(), cart);
            return View(tuple);
        }

        // POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutAsync(Customer user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);
            if (cart == null)
            {
                TempData["error"] = "Cart is empty.";
                return RedirectToAction(nameof(Index));
            }
            double totalPrice = 0;

            foreach (var c in cart)
            {
                if (!InStock(c))
                {
                    TempData["error"] = $"{c.Product.Name} out of stock";
                    return RedirectToAction(nameof(Index));
                }
                totalPrice += c.Total;
            }

            foreach (var c in cart)
            {
                UpdateQuantity(c);
            }
            await _unitOfWork.SaveAsync();
            int id = 0;
            var existingUser = await _unitOfWork.Customer.GetAllAsync(u=>u.Email==user.Email);
            if (existingUser == null)
            {
                await _unitOfWork.Customer.AddAsync(user);
                await _unitOfWork.SaveAsync();
                id = _unitOfWork.Customer.GetApplicationUserID(user);
            }
            else
            {
                id = existingUser?.First().CustomerID ?? 0;

            }

            OrderDetails orderDetails = new OrderDetails()
            {
                Customer = user,
                CartItems = cart,
                OrderDate = DateTime.Now,
                CustomerID = id,
                OrderTotal = totalPrice
            };

            foreach (var c in cart)
            {
                c.Id = 0;
            }

            await _unitOfWork.OrderDetails.AddAsync(orderDetails);
            await _unitOfWork.SaveAsync();

            SendOrderEmailAsync(orderDetails);

            _shoppingCartService.SetCart(cartId.ToString(), new List<ShoppingCartItem>());
            return RedirectToAction("OrderConfirmation");
        }

      

        public IActionResult OrderConfirmation()
        {
            return View();
        }

        #region Email
        private async Task SendOrderEmailAsync(OrderDetails orderDetails)
        {
            try
            {
                string path = "EmailTemplates/OrderDetails";
                string body = await HttpContext.RenderViewAsync(path, orderDetails, false);
                string subject = "Order Details";
                string to = orderDetails.Customer.Email;
                await _emailService.SendEmailAsync(to, subject, body);
                Console.WriteLine("Order email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
        #endregion

        #region Check/Update Stock numbers
        private bool InStock(ShoppingCartItem c)
        {
            if (c.Product.StockQuantity >= c.Count)
                return true;
            return false;
        }
        private void UpdateQuantity(ShoppingCartItem c)
        {
            var p = c.Product;
            p.StockQuantity -= c.Count;
            _unitOfWork.Product.UpdateProduct(p);
        }
        #endregion
    }
}

