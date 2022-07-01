using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using WebApp.Models;

namespace WebApp.Controllers
{

    
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            return View(ProductViewModel.GetProductList(_productRepository));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductViewModel model)
        {

            
            if (model.IsEmpty)
            {
                return View(model);
            }

            if (await _productRepository.AddItemAsync(model))
            {
                return Redirect("List");
            }

            return Redirect("Error");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            return View(ProductViewModel.GetProductById(id, _productRepository));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptEdit(ProductViewModel model)
        {
            if (!model.IsEmpty)
            {
                await _productRepository.ChangeItemAsync(model);
            }
            return Redirect("~/Product/List");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return View(ProductViewModel.GetProductById(id, _productRepository));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptDelete(Guid? id)
        {
            if (id.HasValue)
            {
                await _productRepository.DeleteItemAsync(id.Value);
            }
            return Redirect("~/Product/List");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            IndexViewModel model = new IndexViewModel();
            model.Product = ProductViewModel.GetProductById(id, _productRepository);
            model.Products = ProductViewModel.GetProductList(_productRepository,model.Product.CategoryId);

            return View(model);
        }
        
    }
}