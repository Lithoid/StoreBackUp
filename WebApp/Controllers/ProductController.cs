using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Repositories;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{

    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult List()
        {
            return View(ProductViewModel.GetProductList(_productRepository));
        }

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

        public IActionResult Edit(Guid id)
        {
            return View(ProductViewModel.GetProductById(id, _productRepository));
        }

        public async Task<IActionResult> AcceptEdit(ProductViewModel model)
        {
            if (!model.IsEmpty)
            {
                await _productRepository.ChangeItemAsync(model);
            }
            return Redirect("~/Product/List");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            return View(ProductViewModel.GetProductById(id, _productRepository));
        }

        public async Task<IActionResult> AcceptDelete(Guid? id)
        {
            if (id.HasValue)
            {
                await _productRepository.DeleteItemAsync(id.Value);
            }
            return Redirect("~/Product/List");
        }

        public IActionResult Details(Guid id)
        {
            return View(ProductViewModel.GetProductById(id, _productRepository));
        }
    }
}