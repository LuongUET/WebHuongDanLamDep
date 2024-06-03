using BeautyGuide.Helper;
using BeautyGuide.Models;
using BeautyGuide.Models.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGuide.Controllers
{
    public class CategoryController : Controller
    {
       
        [HttpGet]
        public IActionResult Add()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (HttpContext.Session.GetString("SessionRoleId") == "2")
            {
                return RedirectToAction(nameof(CustomerInterfaceController.Index), "CustomerInterface");
            }
            CategoryDetail model = new CategoryDetail();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryDetail category)
        {
            if (ModelState.IsValid)
            {
                // khong co loi tu phia nguoi dung
                // upload file va lay dc ten file save database

                try
                {
                    int idInsetCate = new CategoryQuery().InsertItemCategory(category.Name);
                    if (idInsetCate > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                }
                catch
                {
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(CategoryController.Index), "Category");
            }
            return Ok(category);
        }
        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (HttpContext.Session.GetString("SessionRoleId") == "2")
            {
                return RedirectToAction(nameof(CustomerInterfaceController.Index), "CustomerInterface");
            }

            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.CategoryDetailList = new List<CategoryDetail>();
            var dataCategory = new CategoryQuery().GetAllCategories(SearchString);
            foreach (var item in dataCategory)
            {
                categoryViewModel.CategoryDetailList.Add(new CategoryDetail
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                });
            }
            ViewData["keyword"] = SearchString;
            return View(categoryViewModel);
        }
        [HttpGet]
        public IActionResult Edit(int id = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (HttpContext.Session.GetString("SessionRoleId") == "2")
            {
                return RedirectToAction(nameof(CustomerInterfaceController.Index), "CustomerInterface");
            }
            CategoryDetail categoryDetail = new CategoryQuery().GetDataCategoryById(id);
            return View(categoryDetail);
        }

        [HttpPost]
        public IActionResult Edit(CategoryDetail categoryDetail)
        {
            try
            {
                var detail = new CategoryQuery().GetDataCategoryById(categoryDetail.Id);

                bool update = new CategoryQuery().UpdateCategoryById(
                    categoryDetail.Name,
                    categoryDetail.Id);
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(CategoryController.Index), "Category");
            }
            catch (Exception ex)
            {
                //return Ok(ex.Message);
                return View(categoryDetail);
                //return Ok(uniquePosterImage);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new CategoryQuery().DeleteItemCategory(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(CustomerController.Index), "Category");
        }
    }
}
