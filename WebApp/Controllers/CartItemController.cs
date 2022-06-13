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

namespace WebApp.Controllers
{
    public class CartItemController : Controller
    {

        private ICartItemRepository _cartItemRepository;
        private IProductRepository _productRepository;
        private readonly UserManager<AppUser> _userManager;


        public CartItemController(ICartItemRepository cartItemRepository, IProductRepository productRepository, UserManager<AppUser> userManager)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
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

            if (product==null)
                return Redirect("List");

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

        public IActionResult List()
        {
            var userId = Guid.Parse(GetCartId());
            if (User.Identity.IsAuthenticated)
            {
                userId = Guid.Parse(_userManager.GetUserId(User));
            }
            return View(CartViewModel.GetCartItemList(_cartItemRepository,userId));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            
            if (id.HasValue)
            {
                await _cartItemRepository.DeleteItemAsync(id.Value);
            }
            return Redirect("~/CartItem/List");
        }

        public string GetCartId()
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
