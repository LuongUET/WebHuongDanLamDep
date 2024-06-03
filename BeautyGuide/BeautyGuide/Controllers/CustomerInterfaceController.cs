using BeautyGuide.Models.Queries;
using BeautyGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BeautyGuide.Controllers
{
    public class CustomerInterfaceController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Tạo một đối tượng CustomerInterfaceViewModel duy nhất
            CustomerInterfaceViewModel customerInterfaceViewModel = new CustomerInterfaceViewModel
            {
                ContentDetailList = new List<ListContent>(),
                CategoryListDetail = new List<ListCategory>()
            };

            // Lấy dữ liệu và gán vào ContentDetailList
            var data = new CustomerInterfaceQuery().ContentInterface(null);
            foreach (var item in data)
            {
                customerInterfaceViewModel.ContentDetailList.Add(new ListContent
                {
                    Id_Guide = item.Id_Guide,
                    Description = item.Description,
                    NameCategoryGuide = item.NameCategoryGuide,
                    NameVideo = item.NameVideo,
                    Name= item.Name,   
                    CategoryIdGuide = item.CategoryIdGuide,
                });
            }

            // Lấy dữ liệu và gán vào CategoryListDetail
            var dataCourse = new CustomerInterfaceQuery().GetCategory();
            foreach (var item in dataCourse)
            {
                customerInterfaceViewModel.CategoryListDetail.Add(new ListCategory
                {
                    CategoryId = item.Id,
                    NameCategory = item.Name,
                });
            }
            //Enumerable<ListCategory> category = customerInterfaceViewModel.ContentDetailList;
            // Gán CategoryListDetail vào ViewBag
            ViewBag.category = customerInterfaceViewModel.CategoryListDetail;

            // Trả về view với model
            return View(customerInterfaceViewModel);
        }
        [HttpGet]
        public IActionResult Category(int id)
        {
            CustomerInterfaceViewModel customerInterfaceViewModel = new CustomerInterfaceViewModel
            {
                ContentDetailList = new List<ListContent>(),
                CategoryListDetail = new List<ListCategory>()
            };

            // Lấy dữ liệu và gán vào ContentDetailList
            var data = new CustomerInterfaceQuery().GetGuideByCategory(id);
            foreach (var item in data)
            {
                customerInterfaceViewModel.ContentDetailList.Add(new ListContent
                {
                    Id_Guide = item.Id_Guide,
                    Description = item.Description,
                    NameCategoryGuide = item.NameCategoryGuide,
                    NameVideo = item.NameVideo,
                    Name = item.Name,
                    CategoryIdGuide = item.CategoryIdGuide,
                });
            }

            // Lấy dữ liệu và gán vào CategoryListDetail
            var dataCourse = new CustomerInterfaceQuery().GetCategory();
            foreach (var item in dataCourse)
            {
                customerInterfaceViewModel.CategoryListDetail.Add(new ListCategory
                {
                    CategoryId = item.Id,
                    NameCategory = item.Name,
                });
            }
            //Enumerable<ListCategory> category = customerInterfaceViewModel.ContentDetailList;
            // Gán CategoryListDetail vào ViewBag
            ViewBag.category = customerInterfaceViewModel.CategoryListDetail;

            // Trả về view với model
            return View(customerInterfaceViewModel);
        }
    }
}
