 using BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult List()
        {
            return View(CategoryViewModel.GetCategoryList(_categoryRepository));
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection formCollection)
        {
            string name = formCollection["Name"];
           
            
            CategoryViewModel category = new CategoryViewModel() { Name = name };
            if (await _categoryRepository.AddItemAsync(category))
            {
                Redirect("~/Category/List");
            }

            return Redirect("~/Category/List"); ;
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id.HasValue)
            {
                await _categoryRepository.DeleteItemAsync(id.Value);
            }
            return Redirect("~/Category/List");
        }
        public IActionResult Edit()
        {
            return Redirect("~/Category/List");
        }
    }
}
