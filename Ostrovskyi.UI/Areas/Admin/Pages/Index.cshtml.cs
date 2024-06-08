using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ostrovskyi.UI.Data;
using Ostrovskyi.UI.Entities;
using Cloth.Domain.Entities;
using Ostrovskyi.UI.Services;
using Microsoft.AspNetCore.Authorization;

namespace Ostrovskyi.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
       

        private readonly IProductService _productService;
        public IndexModel(IProductService productService)
        {
            //_context = context;
            _productService = productService;
        }
        public List<Kosuhi> Kosuhis{ get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public async Task OnGetAsync(int? pageNo = 1)
        {
            var response = await _productService.GetProductListAsync(null, pageNo.Value);
            if (response.Success)
            {
                Kosuhis = response.Data.Items;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
        }
    }
}