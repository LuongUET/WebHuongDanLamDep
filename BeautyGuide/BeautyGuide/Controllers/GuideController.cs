using BeautyGuide.Helper;
using BeautyGuide.Models;
using BeautyGuide.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.NetworkInformation;
using static BeautyGuide.Models.GuideViewModel;

namespace BeautyGuide.Controllers
{
    public class GuideController : Controller
    {
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
            GuideViewModel guideViewModel = new GuideViewModel();
            guideViewModel.GuideDetailList = new List<GuideDetail>();
            var dataCategory = new GuideQuery().GetAllGuides(SearchString);
            foreach (var item in dataCategory)
            {
                guideViewModel.GuideDetailList.Add(new GuideDetail
                {
                    Id = item.Id,
                    Description = item.Description,
                    NameCategory = item.NameCategory,
                    NameVideo = item.NameVideo,
                    Name = item.Name,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                });
            }
            ViewData["keyword"] = SearchString;
            
            return View(guideViewModel);
        
    }
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
            GuideViewModel guideViewModel = new GuideViewModel();
            guideViewModel.GuideDetailList = new List<GuideDetail>();
            var dataCourse = new GuideQuery().GetCategory();
            foreach (var item in dataCourse)
            {
                guideViewModel.GuideDetailList.Add(new GuideDetail
                {
                    CategoryId = item.CategoryId,
                    NameCategory = item.NameCategory

                });
            }
            IEnumerable<GuideDetail> topicDetails = guideViewModel.GuideDetailList;
            ViewBag.topicViewModel = topicDetails;
            GuideDetail model = new GuideDetail();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(GuideDetail guide, IFormFile VideoFile)
        {
            string fileVideo = string.Empty;

            if (ModelState.IsValid)
            {
               
                       fileVideo = await UploadFileHelper.UploadFile(VideoFile);
                // khong co loi tu phia nguoi dung
                // upload file va lay dc ten file save database
                try
                {
                    int idInsetCate = new GuideQuery().InsertItemGuide(guide.CategoryId, fileVideo, guide.Description, guide.Name);
                    if (idInsetCate > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                }
                catch (Exception ex)
                {

                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(CategoryController.Index), "Guide");
            }
            if (VideoFile == null)
            {
                GuideViewModel guideViewModel = new GuideViewModel();
                guideViewModel.GuideDetailList = new List<GuideDetail>();
                var dataCourse = new GuideQuery().GetCategory();
                foreach (var item in dataCourse)
                {
                    guideViewModel.GuideDetailList.Add(new GuideDetail
                    {
                        CategoryId = item.CategoryId,
                        NameCategory = item.NameCategory

                    });
                }
                IEnumerable<GuideDetail> guideDetails = guideViewModel.GuideDetailList;
                ViewBag.topicViewModel = guideDetails;
                return View(guide);
            }

            return View(guide);
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
            GuideViewModel guideViewModel = new GuideViewModel();
            guideViewModel.GuideDetailList = new List<GuideDetail>();
            var dataCourse = new GuideQuery().GetCategory();
            foreach (var item in dataCourse)
            {
                guideViewModel.GuideDetailList.Add(new GuideDetail
                {
                    CategoryId = item.CategoryId,
                    NameCategory = item.NameCategory

                });
            }
            IEnumerable<GuideDetail> guideDetails = guideViewModel.GuideDetailList;
            ViewBag.topicViewModel = guideDetails;
            GuideDetail guideDetail = new GuideQuery().GetDataGuideById(id);
            return View(guideDetail);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(GuideDetail guideDetail, IFormFile VideoFile)
        {
                try
                {
                    var detail = new GuideQuery().GetDataGuideById(guideDetail.Id);
                string uniqueNameImage = string.Empty;
                if (VideoFile == null)
                {
                     uniqueNameImage = detail.NameVideo;
                }

                    
                if(VideoFile == null) {
                    ModelState.Remove("VideoFile");
                }

                if (VideoFile != null)
                {
                    uniqueNameImage = await UploadFileHelper.UploadFile(VideoFile);
                }
                    bool update = new GuideQuery().UpdateGuideById(

                    guideDetail.CategoryId,
                    uniqueNameImage,
                    guideDetail.Description,
                    guideDetail.Id,
                    guideDetail.Name
                    );

                    if (update)
                    {
                        TempData["updateStatus"] = true;
                    }
                    else
                    {
                        TempData["updateStatus"] = false;
                    }

                    return RedirectToAction(nameof(CategoryController.Index), "Guide");
                }
                catch (Exception ex)
                {
                GuideViewModel guideViewModel = new GuideViewModel();
                guideViewModel.GuideDetailList = new List<GuideDetail>();
                var dataCourse = new GuideQuery().GetCategory();
                foreach (var item in dataCourse)
                {
                    guideViewModel.GuideDetailList.Add(new GuideDetail
                    {
                        CategoryId = item.CategoryId,
                        NameCategory = item.NameCategory

                    });
                }
                IEnumerable<GuideDetail> guideDetails = guideViewModel.GuideDetailList;
                ViewBag.topicViewModel = guideDetails;

                return Ok(ex.Message);
                }
            }
        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (HttpContext.Session.GetString("SessionRoleId") == "2")
            {
                return RedirectToAction(nameof(CustomerInterfaceController.Index), "CustomerInterface");
            }
            bool del = new GuideQuery().DeleteItemGuide(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(CustomerController.Index), "Guide");
        }
    }
}
