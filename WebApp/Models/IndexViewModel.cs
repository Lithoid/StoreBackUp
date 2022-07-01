using BL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class IndexViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public ProductViewModel Product { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
