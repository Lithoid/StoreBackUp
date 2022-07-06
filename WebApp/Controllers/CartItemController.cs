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

namespace WebApp.Controllers
{

    [Authorize]
    public class CartItemController : Controller
    {

        private ICartItemRepository _cartItemRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private readonly UserManager<AppUser> _userManager;


        public CartItemController(ICartItemRepository cartItemRepository, IOrderRepository orderRepository,
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


        public async Task<IActionResult> Add(Guid id)
        {
            var userId = Guid.Parse(GetCartId());
            if (User.Identity.IsAuthenticated)
            {
                userId = Guid.Parse(_userManager.GetUserId(User));
            }


            ProductViewModel product = new ProductViewModel();
            product = ProductViewModel.GetProductById(id, _productRepository);

            if (product == null)
                return Redirect("~/CartItem/List");

            CartViewModel cartView = new CartViewModel();
            cartView = CartViewModel.GetCartItemByProdId(product.Id, _cartItemRepository);
            if (cartView == null || cartView.CartId != userId)
            {
                cartView = new CartViewModel
                {
                    Id = Guid.NewGuid(),
                    CartId = userId,
                    Quantity = 1,
                    DateCreated = DateTime.Now,
                    ProductId = product.Id,
                    ProductName = product.Name
                };
                if (await _cartItemRepository.AddItemAsync(cartView))
                {

                }

            }
            return Redirect("~/CartItem/List");
        }

        public async Task<IActionResult> List()
        {
            var userId = Guid.Parse(GetCartId());
            if (User.Identity.IsAuthenticated)
            {
                userId = Guid.Parse(_userManager.GetUserId(User));
            }
            var items = CartViewModel.GetCartItemList(_cartItemRepository, userId);

            ViewBag.Subtotal = 0;
            foreach (var item in items)
            {
                ViewBag.Subtotal += item.Quantity * item.Price;
            }
            



            if (CartViewModel.GetCartItemList(_cartItemRepository, userId).Count()==0)
            {
                return Redirect("~/Shop/List");

            }
            return View(CartViewModel.GetCartItemList(_cartItemRepository, userId));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {

            if (id.HasValue)
            {
                await _cartItemRepository.DeleteItemAsync(id.Value);
            }
            return Redirect("~/CartItem/List");
        }

        public async Task<IActionResult> DeleteAll()
        {
            var userId = Guid.Parse(GetCartId());
            if (User.Identity.IsAuthenticated)
            {
                userId = Guid.Parse(_userManager.GetUserId(User));
            }

            List<CartItem> items = new List<CartItem>();

            foreach (var item in CartViewModel.GetCartItemList(_cartItemRepository, userId))
            {
                items.Add(item);
            }
            await _cartItemRepository.DeleteItemsAsync(items);           
            return Redirect("~/Shop/List");
        }

      

        private string GetCartId()
        {

            if (HttpContext.Session.Keys.Contains("CartId"))
            {

                return HttpContext.Session.GetString("CartId");
            }
            else
            {
                // Generate a new random GUID using System.Guid class.     
                Guid tempCartId = Guid.NewGuid();
                HttpContext.Session.SetString("CartId", tempCartId.ToString());
            }

            return HttpContext.Session.GetString("CartId");
        }



    }
}
