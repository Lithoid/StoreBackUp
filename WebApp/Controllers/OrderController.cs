using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Entities;
using Microsoft.AspNetCore.Authorization;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class OrderController : Controller
    {
        private ICartItemRepository _cartItemRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private readonly UserManager<AppUser> _userManager;


        public OrderController(ICartItemRepository cartItemRepository, IOrderRepository orderRepository,
            IProductRepository productRepository, UserManager<AppUser> userManager)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> MakeOrder(Guid? cartId)
        {
            /*if (cartId.HasValue)
            {
                var items = CartViewModel.GetCartItemList(_cartItemRepository, cartId);
                OrderViewModel model = new OrderViewModel()
                {
                    Status = "Waiting",
                    ClientName = _userManager.GetUserName(User),
                    ClientPhone = "123",//_userManager.GetPhoneNumberAsync(user),
                    OrderDate = DateTime.Now,
                    CartItemIds = items.Select(c => c.Id).ToList()

                };
                if (await _orderRepository.AddItemAsync(model))
                {
                    return Redirect("~/Shop/List");

                }
            }*/

            if (cartId.HasValue)
            {
                ViewData["cartId"] = cartId;
            }

            CartOrderViewModel model = new CartOrderViewModel();
            model.Order = new OrderViewModel();
            model.CartItems = CartViewModel.GetCartItemList(_cartItemRepository, cartId);

            ViewBag.Subtotal = 0;
            foreach (var item in model.CartItems)
            {
                ViewBag.Subtotal += item.Quantity * item.Price;
            }

            return View("MakeOrder", model);
        }
        public async Task<IActionResult> AcceptOrder(CartOrderViewModel model, Guid? cartId)
        {
            var items = CartViewModel.GetCartItemList(_cartItemRepository, cartId);
          
            model.Order.OrderDate = DateTime.Now;
            model.Order.Status = "Waiting";
            model.Order.CartItemIds = items.Select(c => c.Id).ToList();

            if (await _orderRepository.AddItemAsync(model.Order))
            {
                return Redirect("~/Shop/List");

            }
            return Redirect("~/Shop/List");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {


            return View(OrderViewModel.GetOrderList(_orderRepository));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id.HasValue)
            {
                return View(OrderViewModel.GetOrderById(id.Value, _orderRepository));//return Edit view

            }
            return Redirect("~/Order/List");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptEdit(OrderViewModel model)
        {
            if (await _orderRepository.ChangeItemAsync(model))
            {

            }
            return Redirect("~/Order/List");
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id.HasValue)
            {
                await _orderRepository.DeleteItemAsync(id.Value);

            }
            return Redirect("~/Order/List");
        }
    }
}
