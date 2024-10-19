using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace AudioStore.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailService;

        public OrderController(IUnitOfWork unitOfWork, IEmailSender emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            OrderDetails order = await _unitOfWork.OrderDetails.GetSingleOrDefaultAsync(o => o.OrderID == id, includeProperties: "Customer,CartItems");
        
            foreach(var item in order.CartItems)
            {
                var product = await _unitOfWork.Product.GetSingleOrDefaultAsync(p => p.ProductID == item.ProductID);
                item.Product = product;
            }
            

            if (order == null)
            {
                return NotFound();
            }
            OrderDetailEditVM orderVM = new OrderDetailEditVM()
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                CustomerName = order.Customer.Name,
                Email = order.Customer.Email,
                StreetAddress = order.Customer.StreetAddress,
                City = order.Customer.City,
                PostalCode = order.Customer.PostalCode,
                OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
                SelectedOrderStatus = order.OrderStatus,
                OrderTotal = order.OrderTotal,
                CartItems = order.CartItems
    
            };
            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderDetailEditVM orderVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Error updating order details!";
                return View(orderVM);
            }
            OrderDetails order = await _unitOfWork.OrderDetails.GetSingleOrDefaultAsync(o => o.OrderID == orderVM.OrderID,includeProperties:"Customer");
            if(orderVM.Email != order.Customer.Email)
            {
                _emailService.SendEmailAsync(orderVM.Email, "Email changed", $"Dear {order.Customer.Name},\nYou have successfully changed your email address!");
                order.Customer.Email = orderVM.Email;
                _unitOfWork.OrderDetails.Update(order);
                await _unitOfWork.SaveAsync();
            }
            if (orderVM.SelectedOrderStatus != order.OrderStatus)
            {
                string newStatus = orderVM.SelectedOrderStatus.ToString();
                _emailService.SendEmailAsync(orderVM.Email, "Order status change", $"Dear {orderVM.CustomerName},\nYour order status is: {newStatus}");
                order.OrderStatus = orderVM.SelectedOrderStatus;
                _unitOfWork.OrderDetails.Update(order);
                await _unitOfWork.SaveAsync();
            }
           
            TempData["success"] = "Order details updated successfully!";
            return RedirectToAction("Index");
        }

 

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<OrderDetails> orderDetails = await _unitOfWork.OrderDetails.GetAllAsync(includeProperties: "Customer");
            IEnumerable<OrderVM> orders = ConvertToOrderVM(orderDetails);
            return Json(new { data = orders });
        }
   
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            var obj = await _unitOfWork.OrderDetails.GetSingleOrDefaultAsync(c => c.OrderID == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _unitOfWork.OrderDetails.Remove(obj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
        //Convert OrderDetails to OrderVM
        private static IEnumerable<OrderVM> ConvertToOrderVM(IEnumerable<OrderDetails> orderDetailsList)
        {
            return orderDetailsList.Select(order => new OrderVM
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                CustomerName = order.Customer.Name,
                OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
                OrderStatus = order.OrderStatus.ToString(),
                OrderTotal = order.OrderTotal
            }).ToList();
        }
    }
}
