using EntityFramework_Slider.Areas.Admin.ViewModels;
using EntityFramework_Slider.Helpers;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index( int page = 1, int take = 5)  // page = 1 deafult deyer veriemki her defe acilanda 1 ci gosdersin 1 ci page gosdersin
        {
            List<Product> products = await _productService.GetPaginatedDatas(page,take); //page ve take gonderirik icine hemin methoda yazilibdi Servicde orda qebul edecik 

            List<ProdcutListVM> mappedDatas = GetMappedDatas(products); // datalari getirir mene

            int pageCount = await GetPageCountAsync(take); //paglerin sayin gosderir methodu asaqida yazmisiq 

            Paginate<ProdcutListVM> paginatedDatas = new(mappedDatas, page, pageCount);  /// methodumuz bir generice cixartmisiq Paginate bunda her yerde istifade edecik methoda bizden 1 ci datani isdeyir mappedDatas, 2 ci page yeni curet page  3 cu ise totalPage paglerin sayini gosderen methodu gonderirik icine

            ViewBag.take = take;

            return View(paginatedDatas);
        }

        //paglerin sayini veren method

        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();  // bu methoda mene productlarin countunu verir
            return (int)Math.Ceiling((decimal)productCount / take);     /// burda bolurki  product conutumzun nece dene take edirikse o qederde gosdersin yeni asqqidaki 1 2 3 yazir onlarin sayini tapmaq ucun 

            //Math.Ceiling() methodu bizden decimal isdeyir bu neynir tutaqki geldi 3.5 eledi bunu yuvarlasdirsin 4 elesin (int)Math ise biz decimal yazmisiq methdmuzun tipi int di ona casstitng elesin

        }


        private List<ProdcutListVM> GetMappedDatas(List<Product> products)
        {
            List<ProdcutListVM> mappedDatas = new();

            foreach (var product in products)
            {
                ProdcutListVM prodcutVM = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Count = product.Count,
                    CategoryName = product.Category.Name,
                    MainImage = product.Images.Where(m => m.IsMain).FirstOrDefault()?.Image


                };

                mappedDatas.Add(prodcutVM);

            }


            return mappedDatas;

        }


    }
}
