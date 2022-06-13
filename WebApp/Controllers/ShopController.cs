using BL;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ShopController : Controller
    {
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;

        public ShopController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(Guid? id, Guid? categoryId, int page = 1)
        {
           
            int pageSize = 3;   // количество элементов на странице
            IQueryable<ProductViewModel> source = ProductViewModel.GetProductList(_productRepository, categoryId);
          
            var categories = CategoryViewModel.GetCategoryList(_categoryRepository);
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Products = items,
                Categories = categories
                
            };


            return View(viewModel);
        }
    }
}
